using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScreenshot.Patches
{
    internal class HighScoreScreenshotPatch
    {

        [HarmonyPatch(typeof(ResultPlayer))]
        [HarmonyPatch(nameof(ResultPlayer.Update))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool ResultPlayer_Update_Prefix(ResultPlayer __instance)
        {
            // Later, check to see if it's a record or not
            if (!Screenshot.screenshotTaken && __instance.State == ResultPlayer.dispState.Wait)
            {
                var isHighScore = __instance.localResults.ensoPlayerResult[__instance.playerNo].isHiScore;
                var isSilver = __instance.localResults.ensoPlayerResult[__instance.playerNo].crown == DataConst.CrownType.Silver;
                var isGold = __instance.localResults.ensoPlayerResult[__instance.playerNo].crown == DataConst.CrownType.Gold;
                var isRainbow = __instance.localResults.ensoPlayerResult[__instance.playerNo].crown == DataConst.CrownType.Rainbow;
                var isNewSilver = __instance.ensoParam.EnsoResult.ensoPlayerResult[__instance.playerNo].isNewCrown[2];
                var isNewGold = __instance.ensoParam.EnsoResult.ensoPlayerResult[__instance.playerNo].isNewCrown[3];
                var isNewRainbow = __instance.ensoParam.EnsoResult.ensoPlayerResult[__instance.playerNo].isNewCrown[4];

                bool toScreenshot = false;
                toScreenshot |= Plugin.Instance.ConfigScreenshotAllPlays.Value;
                toScreenshot |= Plugin.Instance.ConfigScreenshotAllHighScore.Value && (isHighScore);
                toScreenshot |= Plugin.Instance.ConfigScreenshotFirstClear.Value && (isNewSilver);
                toScreenshot |= Plugin.Instance.ConfigScreenshotFirstFC.Value && (isNewGold);
                toScreenshot |= Plugin.Instance.ConfigScreenshotFirstDFC.Value && (isNewRainbow);
                toScreenshot |= Plugin.Instance.ConfigScreenshotHighScoreClear.Value && (isHighScore && isSilver);
                toScreenshot |= Plugin.Instance.ConfigScreenshotHighScoreFC.Value && (isHighScore && isGold);
                toScreenshot |= Plugin.Instance.ConfigScreenshotHighScoreDFC.Value && (isHighScore && isRainbow);
                toScreenshot |= Plugin.Instance.ConfigScreenshotAllClear.Value && (isSilver);
                toScreenshot |= Plugin.Instance.ConfigScreenshotAllFC.Value && (isGold);
                toScreenshot |= Plugin.Instance.ConfigScreenshotAllDFC.Value && (isRainbow);

                if (toScreenshot)
                {
                    Screenshot.TakeScreenshot();
                }
            }
            else if (Screenshot.screenshotTaken && __instance.State == ResultPlayer.dispState.Start)
            {
                Screenshot.screenshotTaken = false;
            }
            return true;
        }
    }
}
