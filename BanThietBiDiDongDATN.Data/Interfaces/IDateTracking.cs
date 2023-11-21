namespace BanThietBiDiDongDATN.Data.Interfaces
{
    public class IDateTracking
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? ModifyBy { get; set; }
    }
}
