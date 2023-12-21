using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using HarmonyLib;
using Studio;
using UnityEngine;

namespace PictureInPicture
{
    internal class PictureInPicture_Hooks
    {
        [HarmonyPatch(typeof(AddObjectCamera), nameof(AddObjectCamera.Load), 
            new[] {typeof(OICameraInfo), typeof(ObjectCtrlInfo), typeof(TreeNodeObject), typeof(bool), typeof(int)})]
        [HarmonyPostfix]
        public static void CameraPostfix(OCICamera __result)
        {
            __result.objectItem.AddComponent<PictureInPicture_Cam>().ociCamera = __result;
            __result.objectItem.layer = 30; // idk why, but I have to force this now
        }
    }
}
