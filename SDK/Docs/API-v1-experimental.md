# CE managed module API 1.0 / 1.1 / 1.2 / 1.3 — experimental

Original framework author: **SweetBriana**. Combat Extended and its existing mechanics retain their upstream authorship and licence. This is an internal .NET 10 / RimWorld 1.7 development API, not a qualified release or a promise of compatibility with 1.6.

## Contract and lifecycle

Reference `CombatExtended.ModuleApi.dll`. It has no game, Harmony or private CE type dependency. `IModuleApi` exposes API/capability discovery, an immutable field catalog, declaration, structural validation, per-module status, active player configuration and resolved values with provenance. Authors cannot set player policy through that interface. DTO constructors copy input collections; results use immutable collections.

The startup lifecycle is `Collecting → Prepared → Ready` or `Failed`. Declaration records a proposal; it does not apply it. `Prepare()` seals registration and produces a complete immutable plan. The CE adapter applies that plan atomically before definition consumers, verifies that native deserialization/CE initialization retained every accepted value, then acknowledges success. Resolved-value queries return their phase and no result until successful application. A late declaration is rejected. Invalid modules contribute no fields. Duplicate module identities invalidate every declaration of that identity. Required modules must be enabled, valid and acyclic.

`ModuleApi.Declare` is the constructor-order-independent collection entry point. `ModuleApi.Current` becomes available once the native host has its content baseline. Subscribe to `ModuleApi.PhaseChanged` in your constructor; notifications run after transitions, outside the host lock. Query `Current.Phase` for an already-reached state. A broken observer produces an `Observer` diagnostic from `GetDiagnostics()` and cannot veto another module's initialization. Enumerate immutable module states through `GetModuleStatuses()`.

API major must equal 1 and minimum minor must be supported (this native-beam branch supports 0, 1, 2 and 3). Required capabilities must be advertised by the adapter. Module version is numeric `System.Version` format, for example `1.0.0`. Canonical module/package IDs use lower-case ASCII segments separated by `.`, `_` or `-`; definition type and defName remain exact and case-sensitive. Display names are not identities. A structurally valid high-damage value is accepted; range limits describe representability and mechanics, not a balance policy.

Highest player priority wins each exact `(definition type, defName, field)` key. Unlisted modules follow in ordinal ID order. Independent fields merge. Provenance retains baseline, all eligible proposals, author/version and winning reason. Disabled and dependency-blocked modules remain visible in status snapshots. Newly discovered managed modules are enabled by default when their owning content mod is active, unless explicitly disabled in player policy.

## Data-only declarations

`ModuleDocument.Parse(json, actualOwningPackageId)` feeds the same `ModuleManifest` as C#. The containing active mod determines ownership; the document cannot supply or override it. Unknown/duplicate JSON properties and unsupported schemas are errors. The format has a 1 MiB character limit and depth limit of 16. Metadata includes `schema: 1`, `id`, `name`, `author`, `version`, `apiMajor`, `minimumApiMinor`, optional `requiredCapabilities`, `requiredPackages`, `requiredModules`, and `contributions`.

Each contribution has `targetType`, `target`, `field`, `unit` and `value`. Numeric values default to kind `number`; integer-only fields require `kind: "integer"`. Boolean and `{ "type": "ThingDef", "name": "ExactDefName" }` reference values infer their kind. Explicit kinds are `number`, `integer`, `boolean` and `definition`. Units must exactly match the catalog; silent unit conversion is not accepted at the public boundary. Native CE representation conversion belongs to the adapter.

## Player policy

`ModulePolicyState` belongs to the manager, outside the author facade. Its `Active` snapshot never changes within the process. Edits update `Pending`; serialization writes that next-startup policy. A new startup loads it as active. Invalid serialized policy returns a structured `Configuration` diagnostic instead of silently selecting defaults. `DiscardPending()` returns to active values. Integration-off changes managed access only; it does not disable the content mod itself.

## Native integration qualification

The inspected 1.7.5000 loading order is `CreateModClasses → LoadModXML → ApplyPatches → ParseAndProcessXML`, with inheritance resolution inside the last step before deserialization. The adapter hooks immediately after inheritance resolution, inside that parse boundary. This preserves inherited values without a late static constructor. The existing-CE data foundation has bounded native startup, combat, lifecycle and physical manager qualification. Standard conversion has a separate exact-product matrix and additional gates; see the framework qualification documentation. Pure conformance tests are not native firing/save/reload evidence.

Legacy XML patches and arbitrary C#/Harmony integrations remain unmanaged. This API is a cooperation boundary, not a security sandbox or an undo engine. Special beams, shields and drones require separately tested capabilities; declaring an ordinary projectile field does not qualify those mechanics.

## Local conformance

Run the package-free runner with the installed .NET 10 SDK:

```text
dotnet run --project Tests/ModuleApi/ModuleApi.Tests.csproj -p:UseSharedCompilation=false --disable-build-servers
```

Current tests use a small synthetic catalog to exercise the public boundary independently of any game build. Do not distribute game/reference binaries or private runtime-audit source with the SDK.
# Experimental API1.1 standard profile

The standard-conversion branch adds immutable `ManagedIntegrationRequest` values and `manifest.WithIntegrations(...)`, plus schema2 declarations. Requests require minor1 and `integration.standard-ranged.v1`. The host advertises that implemented planner capability; native acceptance remains exact-product scoped. The separate data-only reference build remains API1.0. See [standard ranged profiles](STANDARD-RANGED.md) for the complete author contract and example alternatives.

The API assembly keeps its name, AssemblyVersion1.0.0.0 and original constructor signatures. `CE.ModuleApi.ContractMajor`/`CE.ModuleApi.ContractMinor` assembly metadata identifies available library contracts; the host's `GetApiInfo()` remains authoritative for operational capabilities. These are separate from the framework mod/package version.

An API1.1 C# consumer running with an older API DLL must guard new symbols before JIT compilation reaches them. Keep its entry method and all type/member signatures on API1.0 types, read the public assembly metadata, and place new DTO/method usage in a separate non-inlined synchronous helper. If the contracts are unavailable, queue an ordinary API1.0 manifest requiring minor1/the capability so the older host reports a structured incompatibility. Do not bundle another API DLL. `Tests/ApiMinorConsumerFixture` and `Tests/OlderMinorHost` demonstrate and execute both paths; direct unguarded access reproduced TypeLoadException against the old binary. This uses public assembly metadata, not private CE reflection.

A converted field can have `Baseline.Kind == ModuleValueKind.Unavailable` with `UnavailableReason`, while its applied `Value` has the catalogue's ordinary numeric/reference kind. Do not display such a baseline as zero or as a CE-unit version of vanilla percentages. A ready query for an unapplied, absent native field returns `Result=null`. Unavailable values are never valid author contributions.

# Experimental API1.2 equipment profiles

The same request method and resolver now support `ce.standard.apparel.v1` and `ce.standard.melee.v1`. Each derives only its exact profile capability and a minimum API minor2. Mixed requests union their requirements; replacing requests replaces derived requirements while preserving explicit author requirements. Ranged-only requests retain minor1. See [ordinary equipment](STANDARD-EQUIPMENT.md) for required fields, shape limits and the guarded C# example.

# Experimental API1.3 native beam profile

The existing declaration and query interface also supports `ce.special.native-beam.v1`, deriving minimum minor3 and `integration.native-beam.v1`. Existing constructor signatures and AssemblyVersion1.0.0.0 remain unchanged. Read [native beam integration](NATIVE-BEAM.md) for the complete source-role, author-data, resource and qualification boundaries. A C# entry point must check public contract metadata before reaching new symbols, using the same separate non-inlined helper pattern as earlier profiles.

`apparel.wornBulk` is distinct from carried `item.bulk`. Original vanilla sharp/blunt fractions remain unavailable in physical CE units. Supported unmanaged apparel/melee base values can be observed once after native initialization, without changing the original baseline or crediting a disabled author. Pending unmanaged values are not predicted. This read-only implementation seam is absent from the public author interface and introduces no save payload or world traversal. Native combat/lifecycle, UI, SDK and regression gates remain separately evidenced; API minor metadata alone establishes none of those results.

# API1.3 beam host and qualification status

The library recognises `ce.special.native-beam.v1`, deriving minor3 and `integration.native-beam.v1`. Other profiles retain their original floors. Mixed requests union exact capabilities; replacing requests removes only derived requirements. AssemblyVersion remains1.0.0.0 and `IModuleApi` is unchanged. The implemented host advertises minor3 and the native-beam capability. A declaration still requires complete author data, an eligible source weapon and successful native verification before it can become ready. Native lifecycle, regression, physical UI, final packaged SDK and whole-feature review gates require their own exact-build evidence; the advertised capability alone is not gameplay or release acceptance.

