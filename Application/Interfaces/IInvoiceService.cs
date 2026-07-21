using Domain.Entities;

namespace Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<byte[]> GenerateInvoiceBytesAsync(Booking booking);
    }
}
