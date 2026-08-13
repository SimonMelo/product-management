namespace Products.WebAPI.Features.StockMovement.RegisterStockIn;

public record RegisterStockInRequest(
    List<StockInItemRequest> Items
);

public record StockInItemRequest(string Barcode, int Quantity);