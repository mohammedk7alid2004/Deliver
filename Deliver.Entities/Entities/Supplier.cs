using System;
using Deliver.Entities.Unit;

namespace Deliver.Entities.Entities;
public class Supplier: TrackingBase
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string ShopName { get; set; } = string.Empty;
    public string ShopDescription { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public int ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}
