using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderablePortal : MonoBehaviour
{
    [SerializeField]
    private Camera portalCamera;
    [SerializeField]
    private RenderablePortal targetPortal;

    public void Start()
    {
        portalCamera.enabled = false;
    }

    void OnWillRenderObject ()
    {
        if(enabled && targetPortal.enabled)
        {
            // place portal camera behind target portal
            Matrix4x4 rotate = Matrix4x4.Rotate(new Quaternion(0, 1, 0, 0)); // 180deg rotation on axis y
            Matrix4x4 localTransform = this.transform.worldToLocalMatrix * Camera.main.transform.localToWorldMatrix;
            Matrix4x4 globalTransform = (targetPortal.transform.localToWorldMatrix * rotate) * localTransform;
            portalCamera.transform.SetPositionAndRotation(globalTransform.GetColumn(3), globalTransform.rotation);

            // reset projection matrix
            portalCamera.projectionMatrix = Camera.main.projectionMatrix;

            // set portal camera clip plane to be on target portal, to avoid objects behind it to be visible.
            Vector3 normalCamSpace = portalCamera.worldToCameraMatrix.MultiplyVector(-targetPortal.transform.forward);
            Vector3 posCamSpace = portalCamera.worldToCameraMatrix.MultiplyPoint(targetPortal.transform.position);
            // posCamSpace -= normalCamSpace * 2.0f; // slight offset outwards
            float distCamSpace = -Vector3.Dot(posCamSpace, normalCamSpace);
            Vector4 clipPlaneCamSpace = new Vector4(normalCamSpace.x, normalCamSpace.y, normalCamSpace.z, distCamSpace);
            portalCamera.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCamSpace);

            portalCamera.Render();
        }
    }
}
