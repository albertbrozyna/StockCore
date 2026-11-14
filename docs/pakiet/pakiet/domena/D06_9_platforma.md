<!-- Fragment DOMENA.md (D06_9_platforma). Pełny kontekst: ref/DOMENA.md -->

### 6.9 Elementy poza domeną czystą (persystencja infrastrukturalna, ale częścią modelu danych)

| Klasa | Baza | Właściwości | Cel |
|-------|------|-------------|-----|
| `OutboxMessage` | `Entity<Guid>` (bez audytu) | `TenantId`, `string Type`, `string Payload` (JSON), `DateTimeOffset OccurredAt`, `DateTimeOffset? ProcessedAt`, `int Attempts`, `string? Error` | outbox → Service Bus |
| `IdempotencyRecord` | `Entity<Guid>` | `TenantId`, `string Key`, `string RequestHash`, `int ResponseStatus`, `string? ResponseBody`, `DateTimeOffset CreatedAt`, `DateTimeOffset ExpiresAt` | `Idempotency-Key` |
| `AuditLogEntry` | `ImmutableTenantEntity<Guid>` | `UserId? UserId`, `string Action`, `string EntityType`, `string EntityId`, `string? Changes` (JSON), `DateTimeOffset OccurredAt`, `Guid CorrelationId` | audyt zmian |
