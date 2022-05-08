using Homework.Adapters;
using Homework.Adapters.Jsons;
using Homework.Adapters.Xmls;
using Homework.Brokers.Loggings;
using Homework.Constants;
using Homework.Registration;
using Homework.Services.Converts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ILoggingBroker, LoggingBroker>();
builder.Services.AddScoped<IConvertService, ConvertService>();

builder.Services.UseConvertAdapters()
                 .AddAdapter<XmlConvertAdapter>(AdapterKeyConstants.XML)
                 .AddAdapter<JsonConvertAdapter>(AdapterKeyConstants.JSON);
//               .AddAdapter<ProtoConvertAdapter>(AdapterKeyConstants.PROTO);

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
