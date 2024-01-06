using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketingCORE.Models
{
    public class Logic
    {
        public int Id { get; set; }
        public Campana campana { get; set; }
        public KPI kpi { get; set; }
        public KPIXCampana kPIXCampana { get; set;}
        public Buyer buyer { get; set; }
        public  List<SelectListItem> listkpi { get; set; }
        
        

    }
    
}
