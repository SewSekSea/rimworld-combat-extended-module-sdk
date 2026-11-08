# Combat Extended 1.7 Module SDK 1.3

Want your RimWorld mod to tell Combat Extended how an item should work? Start with one item and one small file.

**Players:** Install the matching Combat Extended 1.7 + Module API mod. You can skip this kit.

**Mod makers:** Download this kit and unzip it somewhere easy to find, like a folder on your desktop. Keep it outside RimWorld's Mods folder. Existing CE XML patches can still work without this kit.

## Make your first module

1. **Pick one item.** Open its XML file in your own mod and find the line that looks like `<defName>MyFirstRifle</defName>`. The words in the middle are the item's exact name for the game.
2. **Pick the closest example.** Start with a [gun](SDK/Examples/standard-ranged/standard-ranged.json), [armour or melee weapon](SDK/Examples/standard-equipment/standard-equipment.json), or [beam weapon](SDK/Examples/native-beam/native-beam.json). JSON is a plain text file. You can edit it with Notepad or VS Code.
3. **Copy the example into your mod.** Make a folder named `CE` in your mod's main folder. Inside it, make a folder named `Modules`. Put the copy there and give it a name ending in `.json`. For example, `MyMod/CE/Modules/my-first-rifle.json`.
4. **Put your name on it.** Change the example's `id` to a unique name like `yourname.mymod.first-rifle`. Change `name` and `author` too.
5. **Point it at your item.** Replace each relevant `target` with the matching `defName` from your XML. Some examples mention a second item, such as ammunition. Give each target its own correct name. Use the [rules for your example](SDK/Docs) to see which fields you need and what numbers they take.
6. **Try it in RimWorld.** Turn on your mod and the matching Combat Extended 1.7 + Module API build. Restart the game. Check CE's module diagnostics for your module ID, then try the item in a throwaway colony. If the module cannot find a target, check the spelling and make sure the mod that adds that item is on.

Share your own JSON with your mod. Your players get the Module API from their CE installation.

## Check a sample before opening the game

This optional check uses the [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0). Open PowerShell in the unzipped SDK folder and paste this line:

```powershell
dotnet tools/validator/ModuleValidator.dll Tests/ModuleApi/Fixtures/OrdinaryNativeBeamContent.xml SDK/Examples/native-beam sweetbriana.example.content
```

This checks the included beam example against its included practice data. For your own file, the validator needs resolved XML containing your real targets. [See the examples and which test data goes with each.](SDK/Examples/README.md)

## Want to use C#?

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), then run these commands from the unzipped SDK folder:

```powershell
$apiReference = (Resolve-Path 'ref/CombatExtended.ModuleApi.dll').Path
dotnet build SDK/Examples/CSharpConsumer/CSharpConsumer.csproj -p:ModuleApiReferencePath="$apiReference"
```

Reference `ref/CombatExtended.ModuleApi.dll` with copy-local turned off. Ship your own consumer assembly. Use either the JSON or C# version of a module when they share the same ID. [Read the full API guide.](SDK/Docs/API-v1-experimental.md)

## Version and credit

This is the experimental author SDK for API 1.3. The product build is `f9b642a6465a41a21154e66abefc4840b34823f3`. See [attribution](ATTRIBUTION.md) and [license](LICENSE.md).
