using BasicAPI.DbContexts;
using BasicAPI.Entities;
using BasicAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{// /// comments in Controllers are converted into an XML document and added to the swagger page for documentation
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

//Set database location through appsettings.json
builder.Services.AddDbContext<PeopleContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:PeopleDBConnectionString"]));

builder.Services.AddScoped<IRepository<Person>, PersonRepository<Person>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
