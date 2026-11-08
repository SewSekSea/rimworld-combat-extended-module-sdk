# Original module examples

API1.2 adds `standard-equipment/standard-equipment.json` and `CSharpConsumer/StandardEquipmentConsumer.cs` as alternative author declarations for ordinary fixed apparel and standalone melee. Use `Tests/ModuleApi/Fixtures/OrdinaryEquipmentContent.xml` for offline validation. Native qualification remains separate and is in progress; see the scoped apparel/melee design.

API1.1 also includes `standard-ranged/standard-ranged.json` and `CSharpConsumer/StandardRangedConsumer.cs` as alternative declarations for one ordinary weapon with two tools. Validate that nested JSON directory against `Tests/ModuleApi/Fixtures/OrdinaryResolvedContent.xml`; the top-level schema1 examples use their original separate fixture. See [the complete standard profile contract](../Docs/STANDARD-RANGED.md).

The three top-level JSON files demonstrate author-owned existing-CE weapon/melee, armour and shared-ammunition data. Their values are examples, not a balance standard. The targets belong to the original synthetic content fixture in `Tests/ModuleApi/Fixtures/ResolvedContent.xml`; that fixture is resolved XML for conformance, **not a playable RimWorld content mod**.

For existing CE-shaped content, put data-only declarations in `<your mod root>/CE/Modules/*.json`. File enumeration is ordinal; identity and player priority determine the result. Native discovery binds the owning package from the active content pack. Use `requiredPackages` for content requirements and `requiredModules` when a managed provider is necessary. Disabling that provider blocks dependents rather than reporting them healthy.

`CSharpConsumer` builds using only the public API project. Call its declaration method from your own mod constructor. `ModuleApi.Declare` queues a proposal independently of constructor order; its receipt means **collected**, not validated or enabled. `ModuleApi.Current` is null before the native host has its content baseline. Inspect host `Phase` and query module status. Data queries remain unavailable until `Ready`. Registration after startup sealing is rejected.

When distributing your own consumer, reference the framework's API DLL with `Private=false`; do not bundle a second API assembly or game/reference DLLs. C# ownership/author metadata is cooperative and is not an authenticated identity or security boundary. Existing unrestricted mod code remains unrestricted.

This SDK and native menu are under development. Build success and pure conformance are distinct from in-game lifecycle acceptance. Candidate 01 is unchanged and does not contain these features.
