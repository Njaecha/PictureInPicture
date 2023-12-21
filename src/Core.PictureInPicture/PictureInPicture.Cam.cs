using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Studio;
using Illusion.Extensions;
using StrayTech;

namespace PictureInPicture
{
    internal class PictureInPicture_Cam : MonoBehaviour
    {
        public static List<PictureInPicture_Cam> cameras = new List<PictureInPicture_Cam>();

        public readonly Camera cam;
        public readonly RenderTexture renderTexture;
        public OCICamera ociCamera { get; internal set; } = null;
        public AmplifyColorEffect amplifyColorEffect { get; private set; }


        public PictureInPicture_Cam()
        {
            // camera
            this.cam = this.gameObject.AddComponent<Camera>();
            this.renderTexture = new RenderTexture(Screen.width, Screen.height, 32);

            Vector3 camPosition = this.transform.position;
            Vector3 camLocalPostion = this.transform.localPosition;
            Quaternion camRotation = this.transform.rotation;
            Vector3 camLocalScale = this.transform.localScale;

            this.cam.CopyFrom(Camera.main); // unfortunately copies the transform aswell, have to reset it

            this.cam.transform.position = camPosition;
            this.cam.transform.localPosition = camLocalPostion;
            this.cam.transform.localScale = camLocalScale;
            this.cam.transform.rotation = camRotation;

            this.cam.targetTexture = renderTexture;
            this.cam.backgroundColor = Color.clear;

            // ApmplifyColorEffect
            amplifyColorEffect = this.gameObject.AddComponent<AmplifyColorEffect>();
            amplifyColorEffect.CopyFromOther(Studio.Studio.Instance.systemButtonCtrl.amplifyColorEffect);

            cameras.Add(this);

        }

        void OnDestroy()
        {
            cameras.Remove(this);
        }

        public void updateACE()
        {
            amplifyColorEffect.CopyFromOther(Studio.Studio.Instance.systemButtonCtrl.amplifyColorEffect);
        }
    }
}
