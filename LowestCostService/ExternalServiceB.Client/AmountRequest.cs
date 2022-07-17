namespace ExternalServiceB.Client
{
    public class TotalAmountRequest
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public Carton[] Cartons { get; set; }
    }

    public class Carton
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }
    }
}
