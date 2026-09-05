using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace EGamePlay
{
    public static class EGamePlayTools
    {
        [MenuItem("Tools/EGamePlay/使用技能Excel配置")]
        public static void UseExcel()
        {
#if UNITY_2021_2_OR_NEWER
            var nbt = NamedBuildTarget.Standalone;
            var dedfine = PlayerSettings.GetScriptingDefineSymbols(nbt);
            PlayerSettings.SetScriptingDefineSymbols(nbt, $"{dedfine};EGAMEPLAY_EXCEL");
#else
            var dedfine = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, $"{dedfine};EGAMEPLAY_EXCEL");
#endif
        }

        [MenuItem("Tools/EGamePlay/使用技能Excel配置", true)]
        public static bool IsUseExcel()
        {
#if UNITY_2021_2_OR_NEWER
            return !PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone).Contains("EGAMEPLAY_EXCEL");
#else
            var dedfine = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            return !dedfine.Contains("EGAMEPLAY_EXCEL");
#endif
        }

        [MenuItem("Tools/EGamePlay/使用技能ScripableObj配置")]
        public static void UseScripableObj()
        {
#if UNITY_2021_2_OR_NEWER
            var nbt = NamedBuildTarget.Standalone;
            var dedfine = PlayerSettings.GetScriptingDefineSymbols(nbt);
            dedfine = dedfine.Replace(";EGAMEPLAY_EXCEL", "");
            dedfine = dedfine.Replace("EGAMEPLAY_EXCEL;", "");
            PlayerSettings.SetScriptingDefineSymbols(nbt, dedfine);
#else
            var dedfine = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            dedfine = dedfine.Replace(";EGAMEPLAY_EXCEL", "");
            dedfine = dedfine.Replace("EGAMEPLAY_EXCEL;", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, dedfine);
#endif
        }

        [MenuItem("Tools/EGamePlay/使用技能ScripableObj配置", true)]
        public static bool IsUseScripableObj()
        {
#if UNITY_2021_2_OR_NEWER
            return PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone).Contains("EGAMEPLAY_EXCEL");
#else
            var dedfine = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            return dedfine.Contains("EGAMEPLAY_EXCEL");
#endif
        }
    }
}