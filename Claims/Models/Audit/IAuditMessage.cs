namespace Claims.Models.Audit
{
    public interface IAuditMessage
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string? HttpRequestType { get; set; }
    }
}
