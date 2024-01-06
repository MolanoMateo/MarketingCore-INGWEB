using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketingCORE.Models
{
    public class ResultadosCamp
    {
        public int Id { get; set; }
        public List<SelectListItem> listkpi { get; set; }
        public KPI kpi { get; set; }
        public Campana campana { get; set; }
        public KPIXCampana kPIXCampana { get; set;}
    }
}
