using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;
using BepInEx.Configuration;
using AutoScreenshot.Patches;
using System.IO;

#if TAIKO_IL2CPP
using BepInEx.Unity.IL2CPP.Utils;
using BepInEx.Unity.IL2CPP;
#endif

namespace AutoScreenshot
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, ModName, PluginInfo.PLUGIN_VERSION)]
#if TAIKO_MONO
    public class Plugin : BaseUnityPlugin
#elif TAIKO_IL2CPP
    public class Plugin : BasePlugin
#endif
    {
        const string ModName = "AutoScreenshot";

        public static Plugin Instance;
        private Harmony _harmony;
        public new static ManualLogSource Log;

        public ConfigEntry<bool> ConfigEnabled;
        public ConfigEntry<string> ConfigScreenshotFolder;
        
        public ConfigEntry<bool> ConfigScreenshotAllPlays;
        public ConfigEntry<bool> ConfigScreenshotAllHighScore;
        public ConfigEntry<bool> ConfigScreenshotFirstClear;
        public ConfigEntry<bool> ConfigScreenshotFirstFC;
        public ConfigEntry<bool> ConfigScreenshotFirstDFC;
        public ConfigEntry<bool> ConfigScreenshotHighScoreClear;
        public ConfigEntry<bool> ConfigScreenshotHighScoreFC;
        public ConfigEntry<bool> ConfigScreenshotHighScoreDFC;
        public ConfigEntry<bool> ConfigScreenshotAllClear;
        public ConfigEntry<bool> ConfigScreenshotAllFC;
        public ConfigEntry<bool> ConfigScreenshotAllDFC;

        public ConfigEntry<bool> ConfigLoggingEnabled;
        public ConfigEntry<int> ConfigLoggingDetailLevelEnabled;

#if TAIKO_MONO
        private void Awake()
#elif TAIKO_IL2CPP
        public override void Load()
#endif
        {
            Instance = this;

#if TAIKO_MONO
            Log = Logger;
#elif TAIKO_IL2CPP
            Log = base.Log;
#endif

            SetupConfig();
            SetupHarmony();
        }

        private void SetupConfig()
        {
            var dataFolder = Path.Combine("BepInEx", "data", ModName);

            ConfigEnabled = Config.Bind("General",
                "Enabled",
                true,
                "Enables the mod.");

            ConfigScreenshotAllPlays = Config.Bind("ScreenshotConditions",
                "ScreenshotAllPlays",
                false,
                "Will screenshot every play.");

            ConfigScreenshotAllHighScore = Config.Bind("ScreenshotConditions",
                "ScreenshotAllHighScore",
                true,
                "Will screenshot every high score.");

            ConfigScreenshotFirstClear = Config.Bind("ScreenshotConditions",
                "ScreenshotFirstClear",
                true,
                "Will screenshot the first time you get a silver crown on a song.");

            ConfigScreenshotFirstFC = Config.Bind("ScreenshotConditions",
                "ScreenshotFirstFC",
                true,
                "Will screenshot the first time you get a gold crown on a song.");

            ConfigScreenshotFirstDFC = Config.Bind("ScreenshotConditions",
                "ScreenshotFirstDFC",
                true,
                "Will screenshot the first time you get a rainbow crown on a song.");

            ConfigScreenshotHighScoreClear = Config.Bind("ScreenshotConditions",
                "ScreenshotHighScoreClear",
                true,
                "Will screenshot high scores on silver crowns.");

            ConfigScreenshotHighScoreFC = Config.Bind("ScreenshotConditions",
                "ScreenshotHighScoreFC",
                true,
                "Will screenshot high scores on gold crowns.");

            ConfigScreenshotHighScoreDFC = Config.Bind("ScreenshotConditions",
                "ScreenshotHighScoreDFC",
                true,
                "Will screenshot high scores on rainbow crowns.");

            ConfigScreenshotAllClear = Config.Bind("ScreenshotConditions",
                "ScreenshotAllClear",
                false,
                "Will screenshot every time you get a silver crown.");

            ConfigScreenshotAllFC = Config.Bind("ScreenshotConditions",
                "ScreenshotAllFC",
                false,
                "Will screenshot every time you get a gold crown.");

            ConfigScreenshotAllDFC = Config.Bind("ScreenshotConditions",
                "ScreenshotAllDFC",
                true,
                "Will screenshot every time you get a rainbow crown.");


            ConfigScreenshotFolder = Config.Bind("General",
                "ScreenshotFolder",
                Path.Combine(dataFolder, "Screenshots"),
                "Enables the example mods.");


            ConfigLoggingEnabled = Config.Bind("Debug",
                "LoggingEnabled",
                true,
                "Enables logs to be sent to the console.");

#if DEBUG
            ConfigLoggingDetailLevelEnabled = Config.Bind("Debug",
                "LoggingDetailLevelEnabled",
                0,
                "Enables more detailed logs to be sent to the console. The higher the number, the more logs will be displayed. Mostly for my own debugging.");
#endif
        }

        private void SetupHarmony()
        {
            // Patch methods
            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            if (ConfigEnabled.Value)
            {
                _harmony.PatchAll(typeof(HighScoreScreenshotPatch));
                Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
            }
            else
            {
                Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is disabled.");
            }
        }

        public static MonoBehaviour GetMonoBehaviour() => TaikoSingletonMonoBehaviour<CommonObjects>.Instance;

        public void StartCustomCoroutine(IEnumerator enumerator)
        {
#if TAIKO_MONO
            GetMonoBehaviour().StartCoroutine(enumerator);
#elif TAIKO_IL2CPP
            GetMonoBehaviour().StartCoroutine(enumerator);
#endif
        }

        public void LogDebugInstance(string value)
        {
#if DEBUG
            Log.LogInfo("[DEBUG]" + value);
#endif
        }
        public static void LogDebug(string value)
        {
            Instance.LogDebugInstance(value);
        }

        public void LogInfoInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogInfo("[" + detailLevel + "] " + value);
            }
        }
        public static void LogInfo(string value, int detailLevel = 0)
        {
            Instance.LogInfoInstance(value, detailLevel);
        }


        public void LogWarningInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogWarning("[" + detailLevel + "] " + value);
            }
        }
        public static void LogWarning(string value, int detailLevel = 0)
        {
            Instance.LogWarningInstance(value, detailLevel);
        }


        public void LogErrorInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogError("[" + detailLevel + "] " + value);
            }
        }
        public static void LogError(string value, int detailLevel = 0)
        {
            Instance.LogErrorInstance(value, detailLevel);
        }

    }
}