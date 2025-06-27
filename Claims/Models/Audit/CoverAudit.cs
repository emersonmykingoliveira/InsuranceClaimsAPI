namespace Claims.Models.Audit
{
    public class CoverAudit : IAuditMessage
    {
        public int Id { get; set; }
        public string? CoverId { get; set; }
        public DateTime Created { get; set; }
        public string? HttpRequestType { get; set; }
    }
}
