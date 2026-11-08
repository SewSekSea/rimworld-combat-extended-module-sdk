# Native beam integration — experimental API1.3

Request `ce.special.native-beam.v1` on an existing `ThingDef`. The public API derives minimum minor3 and capability `integration.native-beam.v1`. JSON declarations use schema2. AssemblyVersion remains1.0.0.0; capability support comes from the host's `GetApiInfo()`. Weapon data and balance belong to the module author; the framework supplies the audited mechanics. The example author is SweetBriana.

Use the JSON or guarded public C# example, choosing one declaration of the module identity. The example targets original synthetic `OrdinaryBeam` content used by offline tests. Replace its target and ammunition identities with your own existing supported definitions, and qualify that actual content in the game. The example numbers are author choices and carry no balance recommendation.

## Supported source

The source must be an existing, unique Item/Primary `ThingWithComps` with one exact native `Verse.Verb_ShootBeam`, ordinary supported components, and optional ordinary tools. Conditional structures, custom thing/verb/property/component/container classes, fuel consumption, changeable projectiles, arbitrary damage extensions, charged or packed ammunition, and buildings require separate adapters. This profile creates no definitions and exposes no arbitrary combat-code injection hook.

The v1 damage source is the ordinary Heat/AddInjury/Hediff_Injury role. Exact native role checks also permit CE's stock damage extension with its existing wall-fragmentation flag and CE's stock injury stabilization component. Other extension behavior and custom component classes are refused. The source damage definition, explicit armor penetration, native flags, minimum range, visuals, fire-size range and visual curve remain source owned. Beam damage uses that native damage worker and CE's applicable armor handling. An explicit penetration value of0.5 is retained as the native value; the framework does not reinterpret it as a percentage or replace the armor formula.

## Required values

Every requested weapon needs all18 fields below. Each existing ordinary tool also needs its indexed `melee.tools.N.damage`, `.cooldown`, `.penetrationSharp` and `.penetrationBlunt` values in `damage`, `s`, `mm-RHA` and `MPa` respectively.

| Field | Kind / exact unit | Mechanical constraint |
|---|---|---|
| `item.mass` | number / `kg` | Finite, nonnegative |
| `item.bulk` | number / `CE-bulk` | Finite, nonnegative |
| `ranged.magazineSize` | integer / `count` | Positive; exactly representable by the native magazine stat, at most16,777,216 |
| `ranged.reloadTime` | number / `s` | Finite, nonnegative base component time |
| `ranged.ammoSet` | definition / `def` | Existing ordinary non-mortar CE ammo set; every final link supported and unique |
| `ranged.verbs.0.warmup` | number / `s` | Finite, nonnegative, native tick conversion representable |
| `ranged.verbs.0.range` | number / `cell` | Finite, at least the unchanged source minimum range |
| `ranged.verbs.0.burst` | integer / `count` | 1 through2,147,483,646 |
| `beam.interval` | integer / `tick` | 1 through2,147,483,646 |
| `beam.totalDamage` | number / `damage` | Finite and strictly positive native damage budget |
| `beam.width` | number / `cell` | Finite, nonnegative |
| `beam.fullWidthRange` | number / `cell` | Finite, nonnegative |
| `beam.maxDeviation` | number / `cell` | Finite, nonnegative |
| `beam.curvature` | number / `cell` | Finite signed offset |
| `beam.startOffset` | number / `cell` | Finite, nonnegative |
| `beam.fireStartChance` | number / `fraction` | 0 through1 |
| `beam.fireAttachChance` | number / `fraction` | 0 through1 |
| `beam.cooldown` | number / `s` | Finite, nonnegative; controls the native equipment cooldown stat |

Times that native mechanics convert to integer ticks must remain representable. A base reload time is also checked against the effective ammunition metadata multiplier; the final product must fit native timing. The representability ceiling used for such seconds is35,791,392. These bounds prevent overflow, and do not prescribe a weapon's balance.

Native totalDamage0 selects DamageDef-default behavior, so v1 refuses it rather than describing it as zero damage. Native beam damage is distributed using native path cells and geometry; `beam.totalDamage` does not promise that the complete budget hits one pawn or object. The framework preserves native cadence and native damage randomness.

## Ammunition and runtime behavior

The selected ammo set supplies CE cartridges and reload metadata. Supported cartridges represent one round and have ordinary AmmoThing roles; linked projectiles use ordinary BulletCE/ProjectilePropertiesCE roles. The beam still applies direct native damage: changing `projectile.damage` does not change its beam budget. Such a change is excluded from the manager's field-specific beam-impact list. A set or projectile reload multiplier remains a genuine dependency, including final links changed by CE's generic ammunition pass.

The conversion installs the framework beam verb, CE verb properties with equipment-stat substitution disabled, and one ammo-user. It adds no fire-modes component and fabricates no projectile. A successful native pulse consumes one prepared CE round. Refused pulses consume none. Author reload time is the base component value; native ammunition and actor modifiers still affect actual reload behavior.

## Player policy and evidence

The module manager shows source authors, active and pending values, original values where meaningful, and unavailable baselines where CE introduced a new concept. For example, an unconverted source has no CE magazine capacity; that baseline is unavailable, not a real zero. A source using DamageDef-default budget mode also has an unavailable budget baseline in the new positive-budget semantics.

Players may disable managed integration and choose field priority. Changes take effect after restart. Balance-only overrides of converted fields must depend on a provider module for that target. Pending previews use the same full effective-plan validation as application. Legacy XML patches and unrestricted Harmony code remain unmanaged; this is a cooperation API, not a sandbox that can revoke arbitrary mod code.

The framework has a versioned supplement for native beam path-cell and hit-cell continuation data. Unsupported schemas, changed path/damage definitions or malformed supplements cancel the saved burst without generating ammunition credit. Native tests, full-save transitions and release acceptance are recorded separately against exact products and dependencies. Offline validator success does not establish live firing, save compatibility, built-in mech operation, building operation or selected-stack readiness.
