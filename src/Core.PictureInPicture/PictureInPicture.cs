using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace PictureInPicture
{
    [BepInPlugin(GUID, PluginName, Version)]
    internal class PictureInPicture : BaseUnityPlugin
    {
        public const string PluginName = "PictureInPicture";
        public const string GUID = "org.njaecha.plugins.pictureinpicture";
        public const string Version = "0.0.1";

        ConfigEntry<KeyboardShortcut> addPip { get; set; }

        List<KeyValuePair<GameObject, PictureInPicture_Picture>> pips = new List<KeyValuePair<GameObject, PictureInPicture_Picture>>();
        public static GameObject pipZoo;

        void Awake()
        {
            pipZoo = new GameObject("PiP Zoo");
            pipZoo.transform.SetParent(transform);
            addPip = Config.Bind("Keybinds", "add PiP", new KeyboardShortcut(KeyCode.P, KeyCode.LeftAlt), "Press this add open a Picture in Picture window");

            Harmony harmony = Harmony.CreateAndPatchAll(typeof(PictureInPicture_Hooks));
        }

        void Update()
        {
            if (addPip.Value.IsDown())
            {
                GameObject pipObject = new GameObject("PiP");
                pipObject.transform.SetParent(pipZoo.transform);
                pipObject.AddComponent<PictureInPicture_Picture>();
            }
        }
    }
}
