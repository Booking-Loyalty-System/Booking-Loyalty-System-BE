namespace Application.DTOs.Tier;

public class CustomerTierResponse
{
    public TierResponse CurrentTier { get; set; } = null!;
    public int LifetimePoints { get; set; }
    public NextTierInfo? NextTier { get; set; }
    public MaintenanceInfo Maintenance { get; set; } = null!;
}

public class NextTierInfo
{
    public string TierName { get; set; } = null!;
    public int MinPointsRequired { get; set; }
    public int PointsNeeded { get; set; }
    public double ProgressPercent { get; set; }
}

public class MaintenanceInfo
{
    public int RequiredPoints { get; set; }
    public int RecentPoints { get; set; }
    public int DaysRemaining { get; set; }
    public bool IsSafe { get; set; }
}
