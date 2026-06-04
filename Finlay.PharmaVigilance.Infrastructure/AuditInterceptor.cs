using System.Text.Json;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Finlay.PharmaVigilance.Infrastructure;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IUserContextService _userContextService;

    public AuditInterceptor(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context == null)
            return;

        var now = TimeZoneHelper.GetEasternNow();

        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries<BasicEntity>())
        {
            if (entry.Entity is AuditLog)
                continue;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }


        foreach (var entry in context.ChangeTracker.Entries<User>())
        {

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }


        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not BasicEntity)
                continue;

            if (entry.Entity is AuditLog)
                continue;

            if (entry.State is not (
                    EntityState.Added or
                    EntityState.Modified or
                    EntityState.Deleted))
                continue;

            Guid? entityId = null;

            var idProperty = entry.Properties.FirstOrDefault(
                p => p.Metadata.Name == "Id");

            if (idProperty?.CurrentValue is Guid id)
                entityId = id;

            var oldValues = entry.State switch
            {
                EntityState.Modified or EntityState.Deleted =>
                    JsonSerializer.Serialize(
                        entry.Properties.ToDictionary(
                            p => p.Metadata.Name,
                            p => p.OriginalValue)),

                _ => null
            };

            var newValues = entry.State switch
            {
                EntityState.Added or EntityState.Modified =>
                    JsonSerializer.Serialize(
                        entry.Properties.ToDictionary(
                            p => p.Metadata.Name,
                            p => p.CurrentValue)),

                _ => null
            };

            auditLogs.Add(new AuditLog
            {
                UserId = _userContextService.GetUserIdOrNull(),
                IpAddress = _userContextService.IPAddress,
                Action = entry.State.ToString(),
                EntityName = entry.Entity.GetType().Name,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        if (auditLogs.Count > 0)
        {
            context.Set<AuditLog>()
                .AddRange(auditLogs);
        }
    }
}