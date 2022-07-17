namespace ExternalServiceB.Client
{
    public class TotalAmountRequest
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public float[] Cartons { get; set; }
    }
}
