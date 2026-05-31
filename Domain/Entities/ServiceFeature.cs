namespace Domain.Entities;

public class ServiceFeature
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string FeatureDescription { get; set; } = null!;
    public int SortOrder { get; set; }

    // Navigation
    public Service Service { get; set; } = null!;
}
