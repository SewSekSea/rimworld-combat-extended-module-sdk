using System.Reflection;
using System.Runtime.CompilerServices;
using CombatExtended.Modules.Api;

namespace SweetBriana.CeModuleExample;

/// <summary>Alternative to native-beam.json; example balance belongs to SweetBriana.</summary>
public static class NativeBeamConsumer
{
    public static CollectionReceipt Declare()=>ModuleApi.Declare(CreateManifest());
    public static ModuleManifest CreateManifest()
    {
        var metadata=typeof(ModuleApi).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToArray();
        var major=metadata.FirstOrDefault(a=>a.Key=="CE.ModuleApi.ContractMajor")?.Value;
        var minor=metadata.FirstOrDefault(a=>a.Key=="CE.ModuleApi.ContractMinor")?.Value;
        if(major=="1"&&int.TryParse(minor,out var available)&&available>=3)return ProfileManifest();
        return new("sweetbriana.example.beam","Native beam author data","SweetBriana","1.0.0","sweetbriana.example.content",1,3,[],
            requiredCapabilities:["integration.native-beam.v1"]);
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ModuleManifest ProfileManifest()
    {
        var target=new DefinitionId("ThingDef","OrdinaryBeam");
        ModuleContribution N(string field,string unit,double value)=>new(target,field,unit,ModuleValue.Numeric(value));
        return new ModuleManifest("sweetbriana.example.beam","Native beam author data","SweetBriana","1.0.0","sweetbriana.example.content",1,3,
            [N("item.mass","kg",2),N("item.bulk","CE-bulk",3),new(target,"ranged.magazineSize","count",ModuleValue.Integer(5)),
             N("ranged.reloadTime","s",4),new(target,"ranged.ammoSet","def",ModuleValue.Reference(new("CombatExtended.AmmoSetDef","CeSet"))),
             N("ranged.verbs.0.warmup","s",1),N("ranged.verbs.0.range","cell",40),new(target,"ranged.verbs.0.burst","count",ModuleValue.Integer(5)),
             new(target,"beam.interval","tick",ModuleValue.Integer(22)),N("beam.totalDamage","damage",190),N("beam.width","cell",6),
             N("beam.fullWidthRange","cell",6.9),N("beam.maxDeviation","cell",1.5),N("beam.curvature","cell",.6),N("beam.startOffset","cell",.8),
             N("beam.fireStartChance","fraction",.85),N("beam.fireAttachChance","fraction",.85),N("beam.cooldown","s",1.7)],
            requiredCapabilities:["integration.native-beam.v1"])
            .WithIntegrations([new(target,"ce.special.native-beam.v1")]);
    }
}
