namespace Handheld.Models
{
    public class ItemInquiryDto
    {
        public string CompanyId { get; set; }
        public string ItemId { get; set; }
        public string ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public string ItemUOM { get; set; }
        public string ItemType { get; set; }
        public bool ItemIsActive { get; set; }

        public bool IsLotTracked { get; set; }
        public bool IsSerialTracked { get; set; }
        public bool IsExpirationTracked { get; set; }

        public decimal? UnitWeight { get; set; }
        public decimal? UnitVolume { get; set; }

        public string BinId { get; set; }
        public string BinCode { get; set; }
        public string BinDescription { get; set; }
        public string BinType { get; set; }
        public bool BinIsActive { get; set; }

        public bool IsBlocked { get; set; }
        public bool AllowPicking { get; set; }
        public bool AllowPutaway { get; set; }

        public decimal StockQty { get; set; }
    }
}
