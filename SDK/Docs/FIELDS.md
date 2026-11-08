# Standard CE data fields — experimental mapping

These mappings operate on **already CE-shaped, inheritance-resolved definitions**, after ordinary legacy patches. They do not convert an arbitrary custom verb, tool or projectile class into CE. Each startup catalog advertises only unambiguous existing fields. Missing nodes and nonconstant random ranges remain unavailable and produce a validation error when targeted. Standard CE class metadata is preserved; no class replacement or private reflection is required in an author module.

| Field | Kind / unit | Native location / conversion |
|---|---|---|
| `item.mass` | number / `kg` | `statBases/Mass` |
| `item.bulk` | number / `CE-bulk` | `statBases/Bulk` |
| `apparel.wornBulk` | number / `CE-bulk` | `statBases/WornBulk`, independent of carried bulk |
| `armour.sharp` | number / `mm-RHA` | `ArmorRating_Sharp` on apparel |
| `armour.blunt` | number / `MPa` | `ArmorRating_Blunt` on apparel |
| `armour.heat` | number / `fraction` | `ArmorRating_Heat` on apparel |
| `ranged.magazineSize` | integer / `count` | CE ammo-user `magazineSize` |
| `ranged.reloadTime` | number / `s` | controlling `ReloadTime` stat for a single stat-backed CE verb; otherwise raw ammo-user field where unambiguous |
| `ranged.recoil` | number / `degree` | shared controlling `Recoil` stat, or a single CE verb from which that stat is generated |
| `ranged.burst` | integer / `count` | shared controlling `BurstShotCount` stat, or its single-verb source |
| `ranged.ammoSet` | definition / `def` | CE ammo-user reference to `CombatExtended.AmmoSetDef` |
| `ranged.verbs.N.recoil` | number / `degree` | raw indexed CE verb, only with `useEquipmentStatValues=false` |
| `ranged.verbs.N.warmup` | number / `s` | indexed CE verb `warmupTime` |
| `ranged.verbs.N.range` | number / `cell` | indexed CE verb `range` |
| `ranged.verbs.N.burst` | integer / `count` | raw indexed CE verb with `useEquipmentStatValues=false`, at least 1 |
| `melee.tools.N.damage` | number / `damage` | indexed CE tool `power` |
| `melee.tools.N.cooldown` | number / `s` | indexed CE tool `cooldownTime` |
| `melee.tools.N.penetrationSharp` | number / `mm-RHA` | indexed CE tool `armorPenetrationSharp` |
| `melee.tools.N.penetrationBlunt` | number / `MPa` | indexed CE tool `armorPenetrationBlunt` |
| `projectile.damage` | integer / `damage` | CE projectile `damageAmountBase` |
| `projectile.speed` | number / `m/s` | CE `speed` in cells/s = public value / 5 |
| `projectile.mass` | number / `g` | CE mass in kg = public value / 1000; fixed ranges only |
| `projectile.diameter` | number / `mm` | CE diameter; fixed ranges only |
| `projectile.ballisticCoefficient` | number / `ratio` | CE coefficient; fixed ranges only |
| `projectile.penetrationSharp` | number / `mm-RHA` | CE `armorPenetrationSharp` |
| `projectile.penetrationBlunt` | number / `MPa` | CE `armorPenetrationBlunt` |
| `projectile.suppression` | number / `ratio` | CE `suppressionFactor` |

`N` is the zero-based position in the resolved tool/verb list. The catalog shows every available index; names/labels do not silently select a different member. Native classes currently accepted are exact standard `CombatExtended.ToolCE`, `CombatExtended.VerbPropertiesCE` and `CombatExtended.ProjectilePropertiesCE`. Subclasses need an audited adapter rather than being assumed compatible.

Values describe base data; CE still applies pawn, quality, attachments, projectile modifiers and fire-mode mechanics. Equipment-stat-backed recoil/burst are weapon-wide fields, not falsely independent verb settings. Recoil writes mirror the participating raw CE recoil values so a zero override also clears the native fallback. Ambiguous multi-verb generated stats and primary-verb reload selection are unavailable rather than guessed. Apparel needs an existing Bulk/WornBulk marker, matching the native apparel autopatcher exclusion; unconverted vanilla apparel is not advertised in physical CE units or allowed to have its mass silently overwritten later.

Limits describe nonnegative values, positive mass/diameter/coefficient, integer representation and native float representation. They do not prescribe author balance. Field presence alone does not certify a physically meaningful custom trajectory or a zero-speed charge-driven weapon.

Known shared-ammunition impact tracing follows CE ammo-user → ammo set → cartridge/projectile and ordinary default-projectile links, including a pending ammo-set reassignment. The manager lists indirect users such as both weapons sharing a projectile. Custom runtime links are untracked and identified as such.

No beam/shield/drone capability is advertised by this adapter. API1.1 additionally supplies the explicit [standard ranged profile](STANDARD-RANGED.md) for supported ordinary weapons. API1.2 adds the separately constrained [fixed apparel and standalone melee profiles](STANDARD-EQUIPMENT.md), with complete author data and exact ordinary shape admission. Their native qualification remains exact-product scoped. Veydran/GrimWorld special mechanics, new definition creation, random interval editing and list/curve replacement remain separate work. None is silently represented as supported by a generic numeric field.
# Native precision

Canonical `ranged.burst` and `ranged.magazineSize` are limited to 16,777,216 so every allowed integer survives native equipment-stat float storage exactly. Magazine size zero remains valid for existing CE data; a standard conversion requires at least one round, including a winning dependent override. Raw indexed `ranged.verbs.N.burst` fields use native Int32 storage and retain their separate bounds. These are structural precision limits, not balance recommendations. Ready requires exact equality for integer fields; numeric float fields use a small representation tolerance.

