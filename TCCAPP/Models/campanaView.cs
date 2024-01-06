using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketingCORE.Models
{
    public class campanaView
    {
        public int Id { get; set; }
        public Buyer buyer { get; set; }
        public Campana campana { get; set; }
        public List<SelectListItem>  opNomBuy { get; set; }
    }
}
