using CombatExtended.Modules.Api;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SweetBriana.CeModuleExample;

/// <summary>Alternative to standard-ranged.json. Example values belong to the module author.</summary>
public static class StandardRangedConsumer
{
    public static CollectionReceipt Declare() => ModuleApi.Declare(CreateManifest());
    public static ModuleManifest CreateManifest()
    {
        // Keep the entry method on API1.0 symbols so an older host can reject it cleanly.
        var metadata=typeof(ModuleApi).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToArray();
        var major=metadata.FirstOrDefault(a=>a.Key=="CE.ModuleApi.ContractMajor")?.Value;
        var minor=metadata.FirstOrDefault(a=>a.Key=="CE.ModuleApi.ContractMinor")?.Value;
        if(major=="1" && int.TryParse(minor,out var available) && available>=1)return ProfileManifest();
        return new("sweetbriana.example.standard","Ordinary ranged conversion","SweetBriana","1.0.0",
            "sweetbriana.example.content",1,1,[],requiredCapabilities:["integration.standard-ranged.v1"]);
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ModuleManifest ProfileManifest()
    {
        var target=new DefinitionId("ThingDef","OrdinaryGun");
        ModuleContribution N(string field,string unit,double value)=>new(target,field,unit,ModuleValue.Numeric(value));
        ModuleContribution I(string field,int value)=>new(target,field,"count",ModuleValue.Integer(value));
        ModuleContribution R(string field,string type,string name)=>new(target,field,"def",ModuleValue.Reference(new(type,name)));
        var data=new List<ModuleContribution>{
            N("item.mass","kg",4),N("item.bulk","CE-bulk",5),I("ranged.magazineSize",24),
            N("ranged.reloadTime","s",2),N("ranged.recoil","degree",0),I("ranged.burst",3),
            N("ranged.verbs.0.warmup","s",.6),N("ranged.verbs.0.range","cell",40),N("ranged.cooldown","s",.2),
            N("ranged.spread","degree",.1),N("ranged.sway","ratio",1),N("ranged.sightsEfficiency","ratio",1.2),
            R("ranged.ammoSet","CombatExtended.AmmoSetDef","CeSet"),R("ranged.verbs.0.defaultProjectile","ThingDef","CeBullet")};
        for(var i=0;i<2;i++)data.AddRange([
            N($"melee.tools.{i}.damage","damage",14+i),N($"melee.tools.{i}.cooldown","s",1),
            N($"melee.tools.{i}.penetrationSharp","mm-RHA",.7),N($"melee.tools.{i}.penetrationBlunt","MPa",2)]);
        return new ModuleManifest("sweetbriana.example.standard","Ordinary ranged conversion","SweetBriana","1.0.0",
            "sweetbriana.example.content",1,1,data).WithIntegrations([new(target,"ce.standard.ranged.v1")]);
    }
}
