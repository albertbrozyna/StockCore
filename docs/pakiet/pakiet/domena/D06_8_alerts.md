<!-- Fragment DOMENA.md (D06_8_alerts). Pełny kontekst: ref/DOMENA.md -->

### 6.8 Kontekst: Alerts

#### `AlertRule` · `TenantEntity<AlertRuleId>`, `IAggregateRoot`

`AlertType Type`, `SkuId? SkuId`, `SkuCategoryId? CategoryId`, `WarehouseId? WarehouseId`, `decimal? Threshold` (ilość dla `LowStock`), `int? DaysBeforeExpiry`, `decimal? VarianceThresholdPercent`, `bool IsActive`.

#### `Alert` · `TenantEntity<AlertId>`

`AlertRuleId RuleId`, `AlertType Type`, `AlertStatus Status`, `string Message`, `SkuId? SkuId`, `LotId? LotId`, `WarehouseId? WarehouseId`, `DateTimeOffset RaisedAt`, `UserId? AcknowledgedBy`, `DateTimeOffset? AcknowledgedAt`, `DateTimeOffset? ResolvedAt`.
Metody: `Acknowledge(userId)`, `Resolve`. Tworzone przez handlery zdarzeń / job cykliczny.
