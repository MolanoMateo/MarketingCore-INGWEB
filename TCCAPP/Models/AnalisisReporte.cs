namespace MarketingCORE.Models
{
    public class AnalisisReporte
    {
        public string Id { get; set; }
        public int Resultado { get; set; }
        public string Nombre { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaVencimiento { get; set; }
    }
}
