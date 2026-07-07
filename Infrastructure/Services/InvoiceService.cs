using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Documents;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        static InvoiceService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateInvoiceBytesAsync(Booking booking)
        {
            var document = new InvoiceDocument(booking);

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);

            return await Task.FromResult(stream.ToArray());
        }
    }
}
