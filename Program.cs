using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Read the content of secrets.json into a string
    var secretsJsonContent = File.ReadAllText(Path.Combine(app.Environment.ContentRootPath, "secrets.json"));

    // Deserialize the JSON string into a JObject
    var secrets = JsonConvert.DeserializeObject<JObject>(secretsJsonContent);

    // Use the JObject to get the actual connection string
    app.Configuration.GetSection("ConnectionStrings")["DefaultConnection"] =
        secrets["ConnectionStrings"]["DefaultConnection"].ToString();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
