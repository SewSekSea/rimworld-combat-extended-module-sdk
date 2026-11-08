using CombatExtended.Modules.Api;

namespace SweetBriana.CeModuleExample;

/// <summary>Call from your own Mod constructor; this original example uses only public API contracts.</summary>
public static class ExampleConsumer
{
    public static CollectionReceipt Declare() => ModuleApi.Declare(new(
        "sweetbriana.example.csharp", "Example rifle data", "SweetBriana", "1.0.0", "sweetbriana.example.content", 1, 0,
        [new(new("ThingDef", "ExampleRifle"), "ranged.magazineSize", "count", ModuleValue.Integer(24))],
        requiredCapabilities: ["combat.ammo"]));

    public static ResolvedQuery? ReadMagazine() => ModuleApi.Current?.GetResolvedValue(
        new("ThingDef", "ExampleRifle"), "ranged.magazineSize");
}
