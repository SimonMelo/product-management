namespace Products.WebAPI.Features.StockMovement.RegisterAdjustment;

public record RegisterAdjustmentRequest(string Barcode, int Quantity);
