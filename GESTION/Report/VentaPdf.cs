using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using GESTION.Models;
using System;
using QuestPDF.Drawing;

public class VentaPdf : IDocument
{
    private readonly VentaViewModel _venta;

    public VentaPdf(VentaViewModel venta)
    {
        _venta = venta;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Header().AlignCenter().Text("Hoja de Entrega").FontSize(20).Bold();
            page.Content().Column(col =>
            {
                col.Item().Text($"Documento No.: {_venta.Encabezado.Id}").Bold();
                col.Item().Text($"Cliente: {_venta.Encabezado.Cliente_Nombre}");
                col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy}");
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();   // Producto
                        columns.ConstantColumn(60); // Cantidad
                        columns.RelativeColumn();   // Comentario/Serie
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Producto").Bold();
                        header.Cell().Text("Cantidad").Bold();
                        header.Cell().AlignLeft().Text("Serie").Bold();
                    });

                    foreach (var d in _venta.Detalles)
                    {
                        table.Cell().Text(d.Producto_Nombre);
                        table.Cell().Text(d.Cantidad.ToString());
                        table.Cell().AlignLeft().Text(d.Comentario ?? "");
                    }
                });

                // Espacio antes de las firmas
                col.Item().Height(40);

                // Sección de firmas
                // Sección de firmas
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(firmas =>
                    {
                        firmas.Item().Text("F____________________").FontSize(14).Bold();
                        firmas.Item().Text("           Entrega").FontSize(12);
                    });
                    row.RelativeItem().Column(firmas =>
                    {
                        firmas.Item().Text("F____________________").FontSize(14).Bold();
                        firmas.Item().Text("              Recibe").FontSize(12);
                    });
                });

            });
        });
    }
}
