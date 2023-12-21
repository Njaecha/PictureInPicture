using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PictureInPicture
{
    internal class PictureInPicture_Cam : MonoBehaviour
    {
        public static List<PictureInPicture_Cam> cameras = new List<PictureInPicture_Cam>();

        public readonly Camera cam;
        public readonly RenderTexture renderTexture;

        public PictureInPicture_Cam()
        {
            this.cam = this.gameObject.AddComponent<Camera>();
            this.renderTexture = new RenderTexture(Screen.width, Screen.height, 32);
            this.cam.CopyFrom(Camera.main);
            this.cam.transform.rotation = Quaternion.identity;
            this.cam.transform.position = Vector3.zero;
            this.cam.transform.localScale = Vector3.one;
            this.cam.targetTexture = renderTexture;
            this.cam.backgroundColor = Color.clear;
            cameras.Add(this);
        }

        void OnDestroy()
        {
            cameras.Remove(this);
        }
    }
}
