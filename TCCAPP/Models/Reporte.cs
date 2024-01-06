namespace MarketingCORE.Models
{
    public class Reporte
    {
        public int Id { get; set; }
        public Campana campana { get; set; }
        public KPI kpi { get; set; }
        public KPIXCampana KPIXCampana { get; set; }
    }
}
