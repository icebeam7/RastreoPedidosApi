namespace RastreoPedidosApi.Models
{
    public class SeguimientoOrdenes
    {
        [Key]
        public int SeguimientoOrdenesId { get; set; }
        public int OrdenesId { get; set; }
        public int Estatus { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
