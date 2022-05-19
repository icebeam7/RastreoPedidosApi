namespace RastreoPedidosApi.Context
{
    public class RastreoPedidosContext : DbContext
    {
        public RastreoPedidosContext(DbContextOptions<RastreoPedidosContext> options) : base(options)
        {

        }

        public DbSet<SeguimientoOrdenes> SeguimientoOrdenes => Set<SeguimientoOrdenes>();
    }
}
