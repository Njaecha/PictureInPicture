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
            GameObject pipObj = new GameObject("PiP Camera");
            pipObj.transform.SetParent(__result.objectItem.transform, false);

            pipObj.AddComponent<PictureInPicture_Cam>().ociCamera = __result;

        }
    }
}
