namespace Api.AppScanner.BarCode.Rotary.COMMON.Models
{
    public class CodigoBarrasModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Codigo { get; set; }
        public DateTime DataLeitura { get; set; }
    }
}
