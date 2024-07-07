using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("pizzas") ?? "Data Source=pizzas.db";

//builder.Services.AddDbContext<PizzaContext>(c => c.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<PizzaContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "", Version = "v1" });
});

// 1) define a unique string
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 2) define allowed domains, in this case "http://example.com" and "*" = all
//    domains, for testing purposes only.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder =>
      {
          builder.WithOrigins(
            "http://localhost:3000", "*");
      });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", async (PizzaContext _ctx) => await _ctx.Pizzas.ToListAsync());
app.MapGet("/pizzas/:id", async (PizzaContext _ctx, int id) => await _ctx.Pizzas.FindAsync(id));
app.MapPost("/pizzas", async (PizzaContext _ctx, Pizza pizza) => {
    _ctx.Pizzas.Add(pizza);
    await _ctx.SaveChangesAsync();
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});
app.MapPut("/pizzas", async (PizzaContext _ctx, Pizza _pizza) =>
{
    var pizza = _ctx.Pizzas.Find(_pizza.Id);
    if (pizza is null)
    {
        return Results.NotFound();
    }
    pizza.Name = _pizza.Name;
    pizza.Description = _pizza.Description;
    _ctx.Pizzas.Update(pizza);
    await _ctx.SaveChangesAsync();
    return Results.Ok();
});
app.MapDelete("/pizzas", async (PizzaContext _ctx, int id) => {
    var pizza = _ctx.Pizzas.Find(id);
    if (pizza is null)
    {
        return Results.NotFound();
    }
    _ctx.Pizzas.Remove(pizza);
    await _ctx.SaveChangesAsync();
    return Results.Ok();

});

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
    });
}

app.Run();
