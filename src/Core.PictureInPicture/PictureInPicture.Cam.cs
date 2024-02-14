using System;
using System.Collections.Generic;
using UnityEngine;
using Studio;
using StrayTech;

namespace PictureInPicture
{
    internal class PictureInPicture_Cam : MonoBehaviour
    {
        public static List<PictureInPicture_Cam> cameras = new List<PictureInPicture_Cam>();

        public readonly Camera cam;
        public RenderTexture renderTexture;
        public OCICamera ociCamera { get; internal set; } = null;
        public AmplifyColorEffect amplifyColorEffect { get; private set; }

        /// <summary>
        /// Fired when this component is destroyed. Aslong as nothing is subscribed to this event, the camera will be disabled to reduce GPU usage.
        /// </summary>
        public EventHandler<CamDestroyedEvent> CamDestroyed { get; set; }


        public PictureInPicture_Cam()
        {
            // camera
            this.cam = this.gameObject.AddComponent<Camera>();
            this.renderTexture = new RenderTexture(PictureInPicture.Instance.pipWidth.Value, PictureInPicture.Instance.pipHeight.Value, 32);

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
            this.cam.enabled = false;

            // ApmplifyColorEffect
            amplifyColorEffect = this.gameObject.AddComponent<AmplifyColorEffect>();
            amplifyColorEffect.CopyFromOther(Studio.Studio.Instance.systemButtonCtrl.amplifyColorEffect);

            cameras.Add(this);
        }

        public void setResolution(int width, int height)
        {
            this.renderTexture = new RenderTexture(width, height, 32);
            this.cam.targetTexture = this.renderTexture;
        }

        void OnDestroy()
        {
            CamDestroyed?.Invoke(this, new CamDestroyedEvent { ociCamera = ociCamera, cam = cam });
            cameras.Remove(this);
        }

        void Update()
        {
            if (cam.enabled && CamDestroyed.IsNullOrEmpty())
            {
                cam.enabled = false;
            }
            else if (!cam.enabled && !CamDestroyed.IsNullOrEmpty())
            {
                cam.enabled = true;
            }
        }

        public void updateACE()
        {
            amplifyColorEffect.CopyFromOther(Studio.Studio.Instance.systemButtonCtrl.amplifyColorEffect);
        }


        // doesnt work
        int rotation = 0;
        public void Rotate()
        {
            if (rotation == 0) rotation = 270;
            else rotation -= 90;

            if (rotation == 90 || rotation == 270)
            {
                renderTexture = new RenderTexture(PictureInPicture.Instance.pipHeight.Value, PictureInPicture.Instance.pipWidth.Value, 32);
            }
            else
            {
                renderTexture = new RenderTexture(PictureInPicture.Instance.pipWidth.Value, PictureInPicture.Instance.pipHeight.Value, 32);
            }
            cam.targetTexture = renderTexture;
            cam.transform.SetLocalEulerAngles(new Vector3(0, 0, rotation), RotationOrder.OrderXYZ);
        }
    }

    public class CamDestroyedEvent : EventArgs
    {
        public Camera cam { get; set; }
        public OCICamera ociCamera { get; set; }

    }
}
