# Ordinary ranged integration — experimental API1.1

An author requests `ce.standard.ranged.v1` on an existing `ThingDef` using schema2 `integrations` or `ManagedIntegrationRequest`. The request requires API major1, minimum minor1 and capability `integration.standard-ranged.v1`. The containing active content mod supplies JSON ownership. The framework supplies audited combat mechanics; the author supplies every starting value and all balance decisions. Player priority and integration opt-out remain player policy.

Use [the schema2 example](../Examples/standard-ranged/standard-ranged.json) **or** `StandardRangedConsumer.Declare()` in [the public-only C# example](../Examples/CSharpConsumer/StandardRangedConsumer.cs). They intentionally declare the same module identity and data and are alternatives. Loading both creates a duplicate identity and invalidates both. The C# entry guards older public contract assemblies before reaching new types; it does not bundle an API DLL or reference CE/game/private types.

These examples target `OrdinaryGun` in `Tests/ModuleApi/Fixtures/OrdinaryResolvedContent.xml`. It is original synthetic resolved XML for offline tests, not a playable content mod. Replace its identities with your own content and existing supported CE ammunition, then qualify that actual mod in the game.

## Supported shape

The target must be an unambiguous existing ordinary `ThingWithComps` definition with one standard `Verb_Shoot` verb, exactly one ordinary CompEquippable and optional ordinary tools. Existing quality, art and forbiddable components are accepted where supported by the planner. Unknown components, custom/subclassed verbs or tools, platforms, one-use weapons, turrets, beams and other special behaviour require a separately audited adapter. Multiple or duplicate structural nodes are rejected.

The profile adds `Verb_ShootCE`, CE ammo-user and fire-modes components and converts every ordinary tool to ToolCE. It preserves the outer definition identity and unrelated label, texture, cost and recipe data. It creates no new definition identities. An invalid module contributes nothing; preparation operates on virtual clones and application preflights all original nodes before committing.

## Required author data

Supply every row below on each requested target. Supplying a field on a different target does not complete the profile. These are mechanics/representation requirements, not recommended balance numbers.

| Field | Kind / exact unit | Requirement |
|---|---|---|
| `item.mass` | number / `kg` | Nonnegative |
| `item.bulk` | number / `CE-bulk` | Nonnegative |
| `ranged.magazineSize` | integer / `count` | 1 through 16,777,216, exactly representable by the native magazine stat |
| `ranged.reloadTime` | number / `s` | Nonnegative |
| `ranged.ammoSet` | definition / `def` | Existing non-mortar CE ammo set; every member must be supported |
| `ranged.recoil` | number / `degree` | Nonnegative; zero clears the native fallback too |
| `ranged.burst` | integer / `count` | 1 through 16,777,216, exactly representable by the native controlling float stat |
| `ranged.verbs.0.warmup` | number / `s` | Nonnegative |
| `ranged.verbs.0.range` | number / `cell` | Nonnegative |
| `ranged.verbs.0.defaultProjectile` | definition / `def` | Existing standard CE bullet in the selected effective ammo set |
| `ranged.cooldown` | number / `s` | Nonnegative; controls RangedWeapon_Cooldown |
| `ranged.spread` | number / `degree` | Nonnegative; controls ShotSpread |
| `ranged.sway` | number / `ratio` | Nonnegative; controls SwayFactor |
| `ranged.sightsEfficiency` | number / `ratio` | At least 0.01, matching the native stat minimum |
| `melee.tools.N.damage` | number / `damage` | Nonnegative; required for every existing tool index N |
| `melee.tools.N.cooldown` | number / `s` | Nonnegative; required for every tool |
| `melee.tools.N.penetrationSharp` | number / `mm-RHA` | Nonnegative; required for every tool |
| `melee.tools.N.penetrationBlunt` | number / `MPa` | Nonnegative; required for every tool |

The first profile accepts ordinary single-round CE cartridges with the standard AmmoThing class and only an ordinary forbiddable component, and exact standard BulletCE/ProjectilePropertiesCE projectiles. Each cartridge has one projectile mapping in its set. Mortar sets, packed/multi-round cartridge units, charged and custom projectile mechanics remain separate adapters. Final native initialization verifies every link again, including changes made by generic ammunition. A field being numerically valid does not qualify a custom trajectory or cartridge-return mechanic.

## Original values, overrides and opt-out

Original same-unit values, such as mass, range, warmup and ordinary tool power/cooldown, remain true source baselines. Newly introduced CE capacity, bulk or penetration fields report `ModuleValueKind.Unavailable` with a reason. Unavailable is never a valid author contribution and must never be displayed as a real zero or a converted vanilla armour percentage. A ready query for an absent unapplied field returns `Result=null`.

A balance-only module editing a field introduced by conversion must declare `requiredModules` containing a provider for that target. This also applies to known semantic range/tool baselines whose original XML is outside the existing-CE editable bindings. The complete provider carries its own mandatory starting data. Disabling it blocks dependent overrides. Priority resolves each field through the same resolver, preserving all sources/authors even when two authors supply equal numbers.

Manager edits affect the pending restart preview. The active native weapon does not change mid-session. Turning managed integration off leaves the content mod enabled. Legacy XML patches and arbitrary Harmony code remain unmanaged; this cooperation API cannot revoke another mod's unrestricted code.

The preview also validates the effective ammunition set/projectile pair after priority resolution. If individually valid modules produce an incompatible pair, the entire managed application is withheld. Every affected module reports an application failure with the cause before restart; an unrelated managed edit is withheld too. Disabling or changing priority can repair the pending plan.

## Save and qualification boundary

The framework owns the saved origin/recovery payload, not the module author. Paid magazines can survive managed component removal; free magazines are logical state and must never create cartridge items. Unresolved weapon/cartridge/destination or ownership ambiguity stays retained with diagnostics rather than guessed. Paid recovery waits while the global ammo option is off, because base CE deliberately destroys ordinary loose ammo under that setting. This does not preserve all loose ammunition against upstream cleanup.

Committed conversion ownership is independent of public readiness. Saving observes actual serialized weapons and writes the framework ledger after their native data, including supported freight holders. For an exactly matching paid magazine, changing calibre ejects incompatible cartridges once; reducing capacity retains the allowed rounds and returns the exact excess. Empty current/selected ammunition is normalized against the final set while a valid alternative selection is preserved.

An existing native count or cartridge that differs from its saved mirror, including an already unloaded zero, is ambiguous. The framework retains that mirror without issuing another stack. Logical magazines carry non-resource count/cartridge/selection witnesses; an observable replacement before reconciliation is retained rather than erased. Identical external replacements cannot be distinguished from the serialized state. These conservative diagnostics may require explicit investigation; they are not a promise of universal third-party save repair or recovery of ammunition lost before a framework ledger existed. The current implementation still requires its exact-build native qualification gates below.

Native qualification is bound to the exact product, game, source modules, fixture and input/output saves. Current results and remaining gates are in the framework's native conversion qualification document. A pure/validator PASS proves the author contract and offline plan, not game firing, save safety, a selected mod stack or public API stability. Candidate01 does not contain this framework.

Validate the alternative JSON example independently of the top-level schema1 examples:

```text
dotnet tools/validator/ModuleValidator.dll Tests/ModuleApi/Fixtures/OrdinaryResolvedContent.xml SDK/Examples/standard-ranged sweetbriana.example.content
```

The validator uses the same profile-aware planner and applies the plan to an in-memory XML document only. It never launches the game or rewrites the supplied content file.
