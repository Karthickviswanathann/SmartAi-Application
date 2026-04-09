using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Smart_Project_Capacity___Effort_Analyzer.Context;
using Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos;
using Smart_Project_Capacity___Effort_Analyzer.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataFlow, DataFlow>();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.RegisterServiceDefaultDependencies(builder.Configuration);
builder.Services.AddScoped<IDataFlow,DataFlow>();
var app = builder.Build();


app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
    {
        RespModel newResponsemodel = new RespModel();

        newResponsemodel.respCode = "401";
        newResponsemodel.respDesc = "UnAuthorized Access Error";
        newResponsemodel.respType = "Error";
        newResponsemodel.Data = null;
        string response = JsonConvert.SerializeObject(newResponsemodel).ToString();
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(response);
    }
});



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();




app.Run();
