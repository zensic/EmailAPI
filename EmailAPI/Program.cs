using EmailService;
using EmailAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Add services to the container.
// Add MySQL database service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  string connectionString;
  var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

  if (env == "Production")
  {
    // If the production environment, use connection string in heroku config vars
    var connUser = Environment.GetEnvironmentVariable("DB_USERNAME");
    var connPass = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var connHost = Environment.GetEnvironmentVariable("DB_HOST");
    var connDb = Environment.GetEnvironmentVariable("DATABASE");

    connectionString = $"server={connHost};Uid={connUser};Pwd={connPass};Database={connDb}";
  }
  else
  {
    // If development environment, use connection string in appsettings config
    connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
  }

  options.UseMySql(connectionString, serverVersion);
});

// Add mailkit service
EmailConfig emailConfig;

if (env == "Production")
{
  // If production environment, grab username + password from heroku config vars
  var mailFrom = Environment.GetEnvironmentVariable("MAIL_FROM");
  var mailSmtpServer = Environment.GetEnvironmentVariable("MAIL_STMPSERVER");
  var mailPort = Environment.GetEnvironmentVariable("MAIL_PORT");
  var mailUsername = Environment.GetEnvironmentVariable("MAIL_USERNAME");
  var mailPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

  emailConfig = new EmailConfig(mailFrom, mailSmtpServer, int.Parse(mailPort), mailUsername, mailPassword);
}
else
{
  // If development environment, grab username + password from local appsettings config
  emailConfig = builder.Configuration
  .GetSection("EmailConfiguration")
  .Get<EmailConfig>();
}

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
  {
    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
    In = ParameterLocation.Header,
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
  });
});

// Allows any origin, header, methods
builder.Services.AddCors(policy =>
{
  policy.AddPolicy("OpenCorsPolicy", options =>
      options.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
//if (app.Environment.IsDevelopment())
//{
//  app.UseSwagger();
//  app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("OpenCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
