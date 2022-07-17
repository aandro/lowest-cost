namespace ExternalServiceA.Client
{
    public class TotalRequest
    {
        public string ContactAddress { get; set; }
        public string WarehouseAddress { get; set; }
        public float[] PackageDimensions { get; set; }
    }
}
