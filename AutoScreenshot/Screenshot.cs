using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AutoScreenshot
{
    public class Screenshot
    {
        internal static bool screenshotTaken = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath">The folder path to save screenshots to. Defaults to AutoScreenshot's config if blank.</param>
        public static void TakeScreenshot(string folderPath = "")
        {
            if (Plugin.Instance.ConfigEnabled.Value)
            {
                var now = DateTime.Now;
                string fileName = now.Year.ToString("00") + "-" 
                                + now.Month.ToString("00") + "-" 
                                + now.Day.ToString("00") + " " 
                                + now.Hour.ToString("00") + "-" 
                                + now.Minute.ToString("00") + "-" 
                                + now.Second.ToString("00") + ".png";

                if (folderPath == "")
                {
                    folderPath = Plugin.Instance.ConfigScreenshotFolder.Value;
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                ScreenCapture.CaptureScreenshot(Path.GetFullPath(Path.Combine(folderPath, fileName)));
                screenshotTaken = true;

                //Plugin.LogInfo("Screenshot saved to: " + Path.Combine(Plugin.Instance.ConfigScreenshotFolder.Value, fileName));
            }
        }
    }
}
