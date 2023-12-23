using KKAPI.Utilities;
using System.IO;
using UnityEngine;

namespace PictureInPicture
{
    internal class PictureInPicture_Picture : MonoBehaviour
    {
        private Texture texture;
        Rect windowRect = new Rect(25,25,360,200);
        private int ID;
        private string title = "Picture In Picture";
        private bool selecting = false;

        private PictureInPicture_Cam DisplayedPiPCam = null;

        void Awake()
        {
            ID = this.GetInstanceID();
            SetDefaultTexture();
        }

        private void SetDefaultTexture()
        {
            // Create a new black texture
            Texture2D tx = new Texture2D(320, 180);
            tx.SetPixels32(new Color32[320 * 180]);
            tx.Apply();
            SetTexture(tx);
        }

        public void SetTexture(Texture2D newTexture)
        {
            texture = newTexture;
            windowRect.size = new Vector2(windowRect.width, newTexture.height * (windowRect.width/ newTexture.width)+20);
        }

        public void SetTexture(RenderTexture newTexture)
        {
            texture = newTexture;
            windowRect.size = new Vector2(windowRect.width, newTexture.height * (windowRect.width / newTexture.width)+20);
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public void resetAspect()
        {
            windowRect.size = new Vector2(windowRect.width, texture.height * (windowRect.width / texture.width)+20);
        }

        internal static OpenFileDialog.OpenSaveFileDialgueFlags SingleFileFlags =
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_FILEMUSTEXIST |
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_LONGNAMES |
              OpenFileDialog.OpenSaveFileDialgueFlags.OFN_EXPLORER;

        void OnGUI()
        {
            if (windowRect == null) { return; }
            windowRect = GUI.Window(ID, windowRect, WindowFunction, title, IMGUIUtils.SolidBackgroundGuiSkin.window);

        }

        void OnDestroy()
        {
            if (DisplayedPiPCam != null)
            {
                DisplayedPiPCam.CamDestroyed -= PiPCamDestroyedEventHandler;
            }
        }

        private void WindowFunction(int WindowID)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.padding = new RectOffset(0,0,0,0);
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            if (GUI.Button(new Rect(windowRect.width-18, 2, 15,15), "X", buttonStyle))
            {
                Destroy(this.gameObject);
            }

            if (GUI.Button(new Rect(windowRect.width - 35, 2, 15, 15), "R", buttonStyle))
            {
                resetAspect();
            }
            if (DisplayedPiPCam == null) GUI.enabled = false;
            if (GUI.Button(new Rect(windowRect.width - 52, 2, 15, 15), "E", buttonStyle))
            {
                if (DisplayedPiPCam != null) DisplayedPiPCam.updateACE();
            }
            GUI.enabled = true;

            if (GUI.Button(new Rect(2,2, 70, 15), "Source", buttonStyle))
            {
                selecting = !selecting;
            }

            GUI.DrawTexture(new Rect(1,21, windowRect.width-2, windowRect.height - 22), texture);

            if (selecting)
            {
                int height = PictureInPicture_Cam.cameras.Count * 24 + 20;
                GUI.Box(new Rect(new Vector2(2, 20), new Vector2(74, height+4)), "");
                GUILayout.BeginArea(new Rect(4, 22, 70, height));
                GUILayout.BeginVertical();
                if (GUILayout.Button("Image"))
                {
                    string[] file = OpenFileDialog.ShowDialog("Open Image", UserData.Path,
                        "Image files (*.png; *.jpg) |*.png; *.jpg | All files (*.*)|*.*",
                        "png", SingleFileFlags);
                    if (file != null && File.Exists(file[0]))
                    {
                        Texture2D tex = new Texture2D(2, 2);
#if KK
                        tex.LoadImage(File.ReadAllBytes(file[0]));
#elif KKS
                        ImageConversion.LoadImage(tex, File.ReadAllBytes(file[0]));
#endif
                        SetTexture(tex);
                        SetTitle(Path.GetFileName(file[0]));
                        selecting = false;
                        if (DisplayedPiPCam != null)
                        {
                            DisplayedPiPCam.CamDestroyed -= PiPCamDestroyedEventHandler;
                        }
                        DisplayedPiPCam = null;
                    }
                }
                foreach (PictureInPicture_Cam cam in PictureInPicture_Cam.cameras)
                {
                    if (GUILayout.Button(cam.ociCamera.name, GUILayout.Height(20)))
                    {
                        if(DisplayedPiPCam != null)
                        {
                            DisplayedPiPCam.CamDestroyed -= PiPCamDestroyedEventHandler;
                        }
                        SetTexture(cam.renderTexture);
                        SetTitle(cam.ociCamera.name);
                        selecting = false;
                        cam.CamDestroyed += PiPCamDestroyedEventHandler;
                        DisplayedPiPCam = cam;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }

            windowRect = IMGUIUtils.DragResizeEatWindow(ID, windowRect);
        }

        private void PiPCamDestroyedEventHandler(object sender, CamDestroyedEvent e)
        {
            SetDefaultTexture();
        }
    }
}
