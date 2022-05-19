var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RastreoPedidosContext>(options =>
{
    var server = Environment.GetEnvironmentVariable("SERVERNAME");
    var database = Environment.GetEnvironmentVariable("DATABASER");
    var username = Environment.GetEnvironmentVariable("USERNAME");
    var password = Environment.GetEnvironmentVariable("PASSWORD");

    var connectionString = $"Server={server};Initial Catalog={database};Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/seguimientoordenes", async (RastreoPedidosContext db) =>
    await db.SeguimientoOrdenes.ToListAsync()
)
.Produces<List<SeguimientoOrdenes>>(StatusCodes.Status200OK)
.WithName("GetSeguimientoOrdenes").WithTags("SeguimientoOrdenes");

app.MapGet("/api/seguimientoordenes/{id}", async (RastreoPedidosContext db, int id) =>
{
    try
    {
        var x = await db.SeguimientoOrdenes.FindAsync(id) is SeguimientoOrdenes seguimientoOrden ? Results.Ok(seguimientoOrden) : Results.NotFound();
        return x;
    }
    catch (System.Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.NotFound();
    }
}
)
.Produces<SeguimientoOrdenes>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetSeguimientoOrdenByID").WithTags("SeguimientoOrdenes");

app.MapPost("/api/seguimientoordenes",
    async ([FromBody] SeguimientoOrdenes nuevoSeguimientoOrden, [FromServices] RastreoPedidosContext db, HttpResponse response) =>
    {
        db.SeguimientoOrdenes.Add(nuevoSeguimientoOrden);
        await db.SaveChangesAsync();
        return Results.Ok(nuevoSeguimientoOrden);
    })
.Accepts<SeguimientoOrdenes>("application/json")
.Produces<SeguimientoOrdenes>(StatusCodes.Status201Created)
.WithName("AddNewSeguimientoOrden").WithTags("SeguimientoOrdenes");

app.MapPut("/api/seguimientoordenes/{id}", async (int id, [FromBody] SeguimientoOrdenes updatedSeguimientoOrden, [FromServices] RastreoPedidosContext db, HttpResponse response) =>
{
    if (id != updatedSeguimientoOrden.SeguimientoOrdenesId)
        return Results.BadRequest();

    db.Entry(updatedSeguimientoOrden).State = EntityState.Modified;

    try
    {
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.WithName("UpdateSeguimientoOrden").WithTags("SeguimientoOrdenes");

app.MapDelete("/api/seguimientoordenes/{id}", async (int id, [FromServices] RastreoPedidosContext db, HttpResponse response) =>
{
    var seguimientoOrden = await db.SeguimientoOrdenes.FindAsync(id);

    if (seguimientoOrden == null)
        return Results.NotFound();

    db.SeguimientoOrdenes.Remove(seguimientoOrden);

    await db.SaveChangesAsync();
    return Results.Ok(seguimientoOrden);
})
.Produces<SeguimientoOrdenes>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("DeleteSeguimientoOrden").WithTags("SeguimientoOrdenes");

app.UseHttpsRedirection();

app.Run();
