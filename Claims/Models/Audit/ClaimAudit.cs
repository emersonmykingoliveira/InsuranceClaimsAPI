namespace Claims.Models.Audit
{
    public class ClaimAudit : IAuditMessage
    {
        public int Id { get; set; }
        public string? ClaimId { get; set; }
        public DateTime Created { get; set; }
        public string? HttpRequestType { get; set; }
    }
}
