var builder = DistributedApplication.CreateBuilder(args);

//var smtpserver = builder.AddParameter("SmtpServer");
//var smtpport = builder.AddParameter("SmtpPort");
//var smtpusername = builder.AddParameter("SmtpUsername");
//var smtppassword = builder.AddParameter("SmtpPassword");
//var smtpsender = builder.AddParameter("SmtpSender");
//var smtpsendername = builder.AddParameter("SmtpSenderName");

var abc = builder.AddPostgres("whatever")
    //.WithDataVolume()
    .WithPgWeb();
var db = abc.AddDatabase("db");

var api = builder.AddProject<Projects.API>("api")
    .WithReference(db)
    .WaitFor(db)
    //.WithEnvironment("MailSettings__SmtpServer", smtpserver)
    //.WithEnvironment("MailSettings__SmtpPort", smtpport)
    //.WithEnvironment("MailSettings__SmtpUsername", smtpusername)
    //.WithEnvironment("MailSettings__SmtpPassword", smtppassword)
    //.WithEnvironment("MailSettings__FromEmail", smtpsender)
    //.WithEnvironment("MailSettings__FromName", smtpsendername)
    ;



var flutter = builder.AddFlutterApp("pawshare", "../../flutter_app")
    .WithArgs("-d", "web-server")
    .WithDartDefine("API_URL_HTTP", api.GetEndpoint("http"))
    .WithDartDefine("API_URL_HTTPS", api.GetEndpoint("https"))
    .WithReference(api)
    .WithExplicitStart();

builder.Build().Run();
