using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

using GameData.Core.Collections;
using GameData.CoreLanguage;
using GameData.CoreLanguage.Collections;

namespace PreFix;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public static Plugin Instance;

    public Plugin()
    {
        Instance = this;
    }

    public override void Load()
    {
        Log.LogWarning($"Plugin {MyPluginInfo.PLUGIN_NAME}-v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
        Log.LogWarning($"制作: MetaMiku");

        try
        {
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(SellablePatch));
        }
        catch
        {
            Log.LogError("FAILED to Apply Hooks!");
        }
    }
}


[HarmonyPatch(typeof(GameData.Core.Collections.Sellable))]
public class SellablePatch
{
    [HarmonyPatch(nameof(Sellable.GetText))]
    [HarmonyPostfix]
    public static void GetText_Postfix( Sellable __instance, ref ObjectLanguageBase __result)
    {
        if (__instance.type != Sellable.SellableType.Food)
        {
            return;
        }
        __result.m_Name = __result.m_Name.Split("·")[^1];

        if (__instance.Modifier == null || __instance.Modifier.Count == 0)
        {
            return;
        }
        
        string prefix = DataBaseLanguage.GetPrefix(__instance.Modifier);
        Plugin.Instance.Log.LogDebug($"PreFix: {__instance.modifier} -> {prefix}");
        
        __result.m_Name = string.Concat(
            __instance.Modifier
                .Select(x => x.RefIngredient().Prefix)
                .Where(x => x != -1)
                .OrderBy(x => x)
                .Distinct()
                .Where(x => DataBaseLanguage.FoodPrefixs.ContainsKey(x))
                .Select(x => DataBaseLanguage.FoodPrefixs[x] + "·")
            )
        + __result.m_Name;
    }
}
