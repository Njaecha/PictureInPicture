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
        public const string Version = "1.0.1";

        public static PictureInPicture Instance { get; private set; }

        internal ConfigEntry<KeyboardShortcut> addPip { get; set; }
        internal ConfigEntry<int> pipHeight { get; set; }
        internal ConfigEntry<int> pipWidth { get; set; }

        public GameObject pipZoo;

        void Awake()
        {
            Instance = this;
            pipZoo = new GameObject("PiP Zoo");
            pipZoo.transform.SetParent(transform);
            addPip = Config.Bind("Keybinds", "add PiP", new KeyboardShortcut(KeyCode.P, KeyCode.LeftAlt), "Press this add open a Picture in Picture window");
            pipHeight = Config.Bind("Quality", "height", 720, "Resolution of the picture in picture camera.");
            pipHeight.SettingChanged += ResolutionSettingChanged;
            pipWidth = Config.Bind("Quality", "width", 1280, "Resolution of the picture in picture camera.");
            pipWidth.SettingChanged += ResolutionSettingChanged;


            Harmony harmony = Harmony.CreateAndPatchAll(typeof(PictureInPicture_Hooks));
        }

        private void ResolutionSettingChanged(object sender, EventArgs e)
        {
            foreach(PictureInPicture_Cam cam in PictureInPicture_Cam.cameras)
            {
                cam.setResolution(pipWidth.Value, pipHeight.Value);
            }
        }

        void Update()
        {
            if (addPip.Value.IsDown())
            {
                GameObject pipObject = new GameObject("PiP");
                pipObject.transform.SetParent(pipZoo.transform);
                PictureInPicture_Picture pip = pipObject.AddComponent<PictureInPicture_Picture>();
            }
        }
    }
}
