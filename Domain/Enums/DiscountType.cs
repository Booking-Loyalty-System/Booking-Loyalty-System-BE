namespace Domain.Enums;

public enum DiscountType
{
    /// <summary>DiscountValue is a percentage (0–100) of the subtotal.</summary>
    Percentage,

    /// <summary>DiscountValue is a fixed currency amount subtracted from the subtotal.</summary>
    FixedAmount
}
