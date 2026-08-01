namespace Application.DTOs.WashBay;

public class CreateWashBayRequest
{
    public string Name { get; set; } = null!;
    public Guid BranchId { get; set; }
}
