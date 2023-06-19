using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true;
    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
    opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("MSSQLDbConnection");
    opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
});

var app = builder.Build();

app.UseWatchDogExceptionLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = builder.Configuration["WatchDog:Username"];
    opt.WatchPagePassword = builder.Configuration["WatchDog:Password"];
});

app.Run();
