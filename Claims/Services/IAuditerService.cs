using Claims.Models.Audit;
using System.Diagnostics.CodeAnalysis;

namespace Claims.Services
{
    public interface IAuditerService
    {
        void AuditClaim(string? id, string httpRequestType);
        void AuditCover(string? id, string httpRequestType);
        bool TryDequeue([NotNullWhen(true)] out IAuditMessage? auditMessage);
    }
}
