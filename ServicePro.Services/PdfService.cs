using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using ServicePro.Core.Interfaces;
using ServicePro.Core.DTOs;

public class PdfService : IPdfService
{
    public byte[] GenerateContactPdf(List<ContactResponseDto> contacts)
    {
        using (var stream = new MemoryStream())
        {
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Fonts
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // ================= TITLE =================
            document.Add(new Paragraph("S.R ENTERPRISE")
                .SetFont(boldFont)
                .SetFontSize(22)
                .SetFontColor(ColorConstants.BLACK)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("Contact Records Report")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph($"Generated: {DateTime.Now:dd MMM yyyy}")
                .SetFont(normalFont)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.RIGHT));

            document.Add(new Paragraph("\n"));

            // ================= TABLE =================
            float[] columnWidths = { 90, 90, 140, 90, 140, 110 };
            var table = new Table(UnitValue.CreatePointArray(columnWidths))
                            .UseAllAvailableWidth();

            // ---------- HEADER STYLE ----------
            Color headerBg = new DeviceRgb(255, 140, 0); // Dark Orange
            string[] headers = { "Name", "Phone", "Email", "Product", "Message", "Created At" };

            foreach (var header in headers)
            {
                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph(header).SetFont(boldFont).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBg)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(8)
                );
            }

            // ---------- DATA ROWS ----------
            bool isAlternate = false;

            foreach (var contact in contacts)
            {
                Color rowColor = isAlternate
                    ? new DeviceRgb(245, 245, 245)   // light grey
                    : ColorConstants.LIGHT_GRAY;

                table.AddCell(CreateCell(contact.Name, normalFont, rowColor));
                table.AddCell(CreateCell(contact.PhoneNumber, normalFont, rowColor));
                table.AddCell(CreateCell(contact.Email, normalFont, rowColor));
                table.AddCell(CreateCell(contact.Product, normalFont, rowColor));
                table.AddCell(CreateCell(contact.Message, normalFont, rowColor));
                table.AddCell(CreateCell(contact.CreatedAt.ToString("dd-MM-yyyy HH:mm"), normalFont, rowColor));

                isAlternate = !isAlternate;
            }

            document.Add(table);

            document.Close();
            return stream.ToArray();
        }
    }

    private Cell CreateCell(string text, PdfFont font, Color bgColor)
    {
        return new Cell()
            .Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9))
            .SetBackgroundColor(bgColor)
            .SetPadding(6);
    }
}