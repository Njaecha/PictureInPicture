using KKAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PictureInPicture
{
    internal class PictureInPicture_Picture : MonoBehaviour
    {
        private Texture tex;
        Rect windowRect;
        private int ID;

        void Awake()
        {
            ID = this.GetInstanceID();
            // Create a new black texture
            Texture2D blackTexture = new Texture2D(320, 180);
            blackTexture.SetPixels32(new Color32[320 * 180]);
            blackTexture.Apply();
            SetTexture(blackTexture);
        }

        public void SetTexture(Texture2D texture)
        {
            tex = texture;
        }

        public void SetTexture(RenderTexture texture)
        {
            tex = texture;
        }

        void OnGUI()
        {
            if (windowRect == null) { return; }
            windowRect = GUI.Window(ID, windowRect, WindowFunction, "PictureInPicture", IMGUIUtils.SolidBackgroundGuiSkin.window);
        }

        private void WindowFunction(int WindowID)
        {
            if (GUI.Button(new Rect(windowRect.width-20, 0, 20,20), "X"))
            {
                Destroy(this);
            }
            if (GUI.Button(new Rect(0,0, 70, 20), "Select"))
            {
                
            }

            GUI.DrawTexture(new Rect(0,20, windowRect.width, windowRect.height - 20), tex);

            windowRect = IMGUIUtils.DragResizeEatWindow(ID, windowRect);
        }
    }
}
