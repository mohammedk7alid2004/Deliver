using Deliver.Entities.Unit;

namespace Deliver.Entities.Entities;
public class Supplier: TrackingBase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}
