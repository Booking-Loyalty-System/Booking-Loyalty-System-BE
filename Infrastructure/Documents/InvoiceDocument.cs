using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly Booking _booking;

        public InvoiceDocument(Booking booking)
        {
            _booking = booking;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30, Unit.Point);
                page.Size(PageSizes.A5); // Khổ A5 gọn đẹp
                page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10).FontColor(Colors.Grey.Darken3));

                // 1. HEADER (Tên thương hiệu & Mã hóa đơn)
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("AUTOWASH PRO").FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Hệ thống rửa xe tự động thông minh").FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                    });
                    row.ConstantItem(100).Column(col =>
                    {
                        col.Item().Text("HÓA ĐƠN VAT").FontSize(12).Bold().AlignRight();
                        col.Item().Text($"Mã: {_booking.BookingCode}").FontSize(10).FontFamily(Fonts.CourierNew).Bold().FontColor(Colors.Blue.Medium).AlignRight();
                    });
                });

                // 2. CONTENT (Thông tin chi tiết)
                page.Content().PaddingVertical(15, Unit.Point).Column(col =>
                {
                    // Đường gạch ngang phân cách
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
                    col.Item().PaddingBottom(10);

                    // Khối thông tin khách hàng & Xe
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(t => { t.Span("Khách hàng: ").FontColor(Colors.Grey.Medium); t.Span(_booking.Customer?.FullName ?? "Khách vãng lai").Bold(); });
                            c.Item().Text(t => { t.Span("Ngày đặt: ").FontColor(Colors.Grey.Medium); t.Span(_booking.BookingDate.ToString("dd/MM/yyyy")); });
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(t => { t.Span("Biển số xe: ").FontColor(Colors.Grey.Medium); t.Span(_booking.Vehicle?.LicensePlate ?? "N/A").Bold(); });

                            // SỬA LỖI ENUM: Ép kiểu Enum sang String hoặc hiển thị mặc định
                            var vehicleTypeStr = _booking.Vehicle != null ? _booking.Vehicle.Type.ToString() : "N/A";
                            c.Item().Text(t => { t.Span("Loại xe: ").FontColor(Colors.Grey.Medium); t.Span(vehicleTypeStr); });
                        });
                    });

                    col.Item().PaddingBottom(15);

                    // BẢNG CHI TIẾT DỊCH VỤ
                    col.Item().Table(table =>
                    {
                        // SỬA LỖI: Đổi từ RelativeItem sang RelativeColumn
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Tên gói
                            columns.RelativeColumn(1); // Số lượng
                            columns.RelativeColumn(2); // Đơn giá
                        });

                        // Header của Bảng
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Tên Dịch Vụ").Bold();
                            header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("SL").Bold().AlignCenter();
                            header.Cell().Background(Colors.Grey.Lighten4).Padding(5).Text("Thành Tiền").Bold().AlignRight();
                        });

                        // Row: Gói rửa chính
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text(_booking.WashPackage?.Name ?? "Dịch vụ rửa xe");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text("1").AlignCenter();
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text($"{_booking.WashPackage?.Price:N0} đ").AlignRight();

                        // Các dịch vụ đính kèm Add-Ons (Nếu có)
                        if (_booking.BookingAddOns != null)
                        {
                            foreach (var addon in _booking.BookingAddOns)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text($"+ {addon.AddOn?.Name}");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text("1").AlignCenter();
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text($"{addon.Price:N0} đ").AlignRight();
                            }
                        }
                    });

                    col.Item().PaddingBottom(15);

                    // PHẦN TÍNH TOÁN TỔNG TIỀN
                    col.Item().AlignRight().Width(180).Column(c =>
                    {
                        if (_booking.DiscountAmount > 0)
                        {
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Giảm giá:").FontColor(Colors.Grey.Medium);
                                r.ConstantItem(70).Text($"-{_booking.DiscountAmount:N0} đ").AlignRight().FontColor(Colors.Red.Medium);
                            });
                        }

                        c.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().Text("Tổng thanh toán:").Bold().FontSize(12);
                            r.ConstantItem(80).Text($"{_booking.TotalPrice:N0} đ").Bold().FontSize(12).FontColor(Colors.Blue.Medium).AlignRight();
                        });
                    });
                });

                // 3. FOOTER
                page.Footer().Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
                    col.Item().PaddingTop(5).Text("Cảm ơn quý khách. Hẹn gập lại!").AlignCenter().FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                    col.Item().Text("Hóa đơn điện tử lập tự động bởi hệ thống AutoWash Pro").AlignCenter().FontSize(8).FontColor(Colors.Grey.Lighten1);
                });
            });
        }
    }
}
