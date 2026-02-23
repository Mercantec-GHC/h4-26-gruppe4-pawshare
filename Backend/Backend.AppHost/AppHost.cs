var builder = DistributedApplication.CreateBuilder(args);

var JWTIssuer = builder.AddParameter("JWTIssuer");
var JWTAudience = builder.AddParameter("JWTAudience");
var JWTSecret = builder.AddParameter("JWTSecret");
#pragma warning disable ASPIREINTERACTION001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var JWTExpirationMinutes = builder.AddParameter("JWTExpirationMinutes").WithCustomInput(parameter => new ()
{
    Name = parameter.Name,
    InputType = InputType.Number,
    Label = "JWT Expiration Minutes",
    Value = "60"
});

var WithMigrationRun = builder.AddParameter("WithMigrationRun").WithCustomInput(parameter => new ()
{
    Name = parameter.Name,
    InputType = InputType.Choice,
    Label = "Run Migrations on Startup",
    Options = new List<KeyValuePair<string, string>>() { new("local", "Yes"), new("not-aspire", "No") },

});

var WithSeedRun = builder.AddParameter("WithSeedRun").WithCustomInput(parameter => new ()
{
    Name = parameter.Name,
    InputType = InputType.Boolean,
    Label = "Run Seed on Startup"

});
#pragma warning restore ASPIREINTERACTION001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

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
    .WithEnvironment("DEV__ENV", WithMigrationRun)
    .WithEnvironment("DEV__SEED", WithSeedRun)
    .WithEnvironment("Jwt__Issuer", JWTIssuer)
    .WithEnvironment("Jwt__Audience", JWTAudience)
    .WithEnvironment("Jwt__SecretKey", JWTSecret)
    .WithEnvironment("Jwt__ExpiryMinutes", JWTExpirationMinutes)
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
    .WithDartDefine("APP_ENV", "local")
    .WithDartDefine("API_URL_HTTP", api.GetEndpoint("http"))
    .WithDartDefine("API_URL_HTTPS", api.GetEndpoint("https"))
    .WithReference(api)
    .WithExplicitStart();

builder.Build().Run();
