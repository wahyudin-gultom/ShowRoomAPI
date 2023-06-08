using Microsoft.EntityFrameworkCore;
using ShowRoomAPI.DataAccess.Implementation;
using ShowRoomAPI.DataAccess.Interface;
using ShowRoomAPI.Models.Entitas;

var builder = WebApplication.CreateBuilder(args);

//connect to database
builder.Services.AddDbContext<ShowRoomDataContext>( op => {
    op.UseNpgsql(builder.Configuration.GetConnectionString("ShowroomDb"));
});

// Add services to the container.
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();


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
