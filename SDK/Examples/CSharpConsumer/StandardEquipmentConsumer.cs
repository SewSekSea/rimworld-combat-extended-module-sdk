using System.Reflection;
using System.Runtime.CompilerServices;
using CombatExtended.Modules.Api;

namespace SweetBriana.CeModuleExample;

/// <summary>Alternative to standard-equipment.json; all example balance belongs to its author.</summary>
public static class StandardEquipmentConsumer
{
    public static CollectionReceipt Declare() => ModuleApi.Declare(CreateManifest());
    public static ModuleManifest CreateManifest()
    {
        // Keep the entry method and signatures on API1.0 symbols. Older contracts can report
        // the unsupported API/capabilities without touching newer DTOs or native definitions.
        var metadata = typeof(ModuleApi).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToArray();
        var major = metadata.FirstOrDefault(a => a.Key == "CE.ModuleApi.ContractMajor")?.Value;
        var minor = metadata.FirstOrDefault(a => a.Key == "CE.ModuleApi.ContractMinor")?.Value;
        if (major == "1" && int.TryParse(minor, out var available) && available >= 2) return ProfileManifest();
        return new("sweetbriana.example.equipment", "Ordinary apparel and melee", "SweetBriana", "1.0.0",
            "sweetbriana.example.content", 1, 2, [],
            requiredCapabilities: ["integration.standard-apparel.v1", "integration.standard-melee.v1"]);
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ModuleManifest ProfileManifest()
    {
        var vest = new DefinitionId("ThingDef", "OrdinaryVest");
        var mace = new DefinitionId("ThingDef", "OrdinaryMace");
        ModuleContribution N(DefinitionId target, string field, string unit, double value) => new(target, field, unit, ModuleValue.Numeric(value));
        var data = new List<ModuleContribution> {
            N(vest,"item.mass","kg",3),N(vest,"item.bulk","CE-bulk",5),N(vest,"apparel.wornBulk","CE-bulk",2),
            N(vest,"armour.sharp","mm-RHA",8),N(vest,"armour.blunt","MPa",12),N(vest,"armour.heat","fraction",.4),
            N(mace,"item.mass","kg",2),N(mace,"item.bulk","CE-bulk",3)
        };
        for (var i = 0; i < 2; i++) data.AddRange([
            N(mace,$"melee.tools.{i}.damage","damage",14+i),N(mace,$"melee.tools.{i}.cooldown","s",1),
            N(mace,$"melee.tools.{i}.penetrationSharp","mm-RHA",.7),N(mace,$"melee.tools.{i}.penetrationBlunt","MPa",2)
        ]);
        return new ModuleManifest("sweetbriana.example.equipment", "Ordinary apparel and melee", "SweetBriana", "1.0.0",
            "sweetbriana.example.content", 1, 2, data)
            .WithIntegrations([new(vest,"ce.standard.apparel.v1"),new(mace,"ce.standard.melee.v1")]);
    }
}
