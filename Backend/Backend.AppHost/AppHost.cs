var builder = DistributedApplication.CreateBuilder(args);

var EmailFrom = builder.AddParameter("EmailFrom");
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

var mailpit = builder.AddMailPit("mailpit");

var abc = builder.AddPostgres("whatever")
    .WithPgWeb();
var db = abc.AddDatabase("db");

var api = builder.AddProject<Projects.API>("api")
    .WithEnvironment("DEV__ENV", WithMigrationRun)
    .WithEnvironment("DEV__SEED", WithSeedRun)
    .WithEnvironment("Jwt__Issuer", JWTIssuer)
    .WithEnvironment("Jwt__Audience", JWTAudience)
    .WithEnvironment("Jwt__SecretKey", JWTSecret)
    .WithEnvironment("Jwt__ExpiryMinutes", JWTExpirationMinutes)
    .WithEnvironment("Email__From", EmailFrom)
    .WithEnvironment("Email__Smtp", () => mailpit.GetEndpoint("smtp").Host)
    .WithEnvironment("Email__Port", () => mailpit.GetEndpoint("smtp").Port.ToString())
    .WithEnvironment("Email__Local", "true")
    .WithEnvironment("Email__Username", "mailpit")
    .WithEnvironment("Email__Password", "mailpit")
    .WithReference(db)
    .WaitFor(db)
    .WaitFor(mailpit)
    ;



var flutter = builder.AddFlutterApp("pawshare", "../../flutter_app")
    .WithArgs("-d", "web-server")
    .WithDartDefine("APP_ENV", "local")
    .WithDartDefine("API_URL_HTTP", api.GetEndpoint("http"))
    .WithDartDefine("API_URL_HTTPS", api.GetEndpoint("https"))
    .WithReference(api);


api.WithEnvironment("FrontendUrl", flutter.GetEndpoint("http"));

builder.Build().Run();
