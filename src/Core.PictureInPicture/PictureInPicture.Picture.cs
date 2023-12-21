using KKAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace PictureInPicture
{
    internal class PictureInPicture_Picture : MonoBehaviour
    {
        private Texture tex;
        Rect windowRect = new Rect(25,25,360,180);
        private int ID;
        private string title;
        private bool selecting = false;

        void Awake()
        {
            ID = this.GetInstanceID();
            // Create a new black texture
            Texture2D tx = new Texture2D(320, 180);
            tx.SetPixels32(new Color32[320 * 180]);
            tx.Apply();
            SetTexture(tx);
        }

        public void SetTexture(Texture2D texture)
        {
            tex = texture;
        }

        public void SetTexture(RenderTexture texture)
        {
            tex = texture;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        internal static OpenFileDialog.OpenSaveFileDialgueFlags SingleFileFlags =
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_FILEMUSTEXIST |
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_LONGNAMES |
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_EXPLORER;

        void OnGUI()
        {
            if (windowRect == null) { return; }
            windowRect = GUI.Window(ID, windowRect, WindowFunction, title, IMGUIUtils.SolidBackgroundGuiSkin.window);
            if (selecting)
            {
                int height = PictureInPicture_Cam.cameras.Count * 22 + 40;
                GUI.Box(new Rect(windowRect.position + new Vector2(0, 20), new Vector2(70, height)), "");
                GUILayout.BeginArea(new Rect(0, 0, 70, height));
                GUILayout.BeginVertical();
                if (GUILayout.Button("Choose Image"))
                {
                    string[] file = OpenFileDialog.ShowDialog("Open Image", UserData.Path,
                        "Image files (*.png; *.jpg) |*.png; *.jpg | All files (*.*)|*.*",
                        "png", SingleFileFlags);
                    if (file != null && File.Exists(file[0]))
                    {
                        Texture2D tex = new Texture2D(2,2);
#if KK
                        tex.LoadImage(File.ReadAllBytes(file[0]));
#elif KKS
                        ImageConversion.LoadImage(tex, File.ReadAllBytes(file[0]));
#endif
                        SetTexture(tex);
                        SetTitle(Path.GetFileName(file[0]));
                        selecting = false;
                    }
                }
                foreach(PictureInPicture_Cam cam in PictureInPicture_Cam.cameras)
                {
                    if (GUILayout.Button(cam.ociCamera.name))
                    {
                        SetTexture(cam.renderTexture);
                        SetTitle(cam.ociCamera.name);
                        selecting = false;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        private void WindowFunction(int WindowID)
        {
            if (GUI.Button(new Rect(windowRect.width-20, 0, 20,20), "X"))
            {
                Destroy(this.gameObject);
            }
            if (GUI.Button(new Rect(0,0, 70, 20), "Select"))
            {
                selecting = true;
            }

            GUI.DrawTexture(new Rect(0,20, windowRect.width, windowRect.height - 20), tex);

            windowRect = IMGUIUtils.DragResizeEatWindow(ID, windowRect);
        }
    }
}
