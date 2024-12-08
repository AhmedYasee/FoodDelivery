public class WarehouseTransferViewModel
{
    public int SourceWarehouseId { get; set; }
    public int DestinationWarehouseId { get; set; }
    public List<TransferItem> TransferItems { get; set; }
}

public class TransferItem
{
    public int ProductId { get; set; }
    public string BatchNumber { get; set; }
    public int Quantity { get; set; }
}
