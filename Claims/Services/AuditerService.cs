using Claims.Models.Audit;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Claims.Services
{
    public class AuditerService : IAuditerService
    {
        private readonly ConcurrentQueue<IAuditMessage> _auditMessageQueue = new();

        public void AuditClaim(string? id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now.Date,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditMessageQueue.Enqueue(claimAudit);
        }
        
        public void AuditCover(string? id, string httpRequestType)
        {
                var coverAudit = new CoverAudit()
                {
                    Created = DateTime.Now.Date,
                    HttpRequestType = httpRequestType,
                    CoverId = id
                };

            _auditMessageQueue.Enqueue(coverAudit);
        }

        public bool TryDequeue([NotNullWhen(true)] out IAuditMessage? auditMessage)
        {
            return _auditMessageQueue.TryDequeue(out auditMessage);
        }
    }
}
