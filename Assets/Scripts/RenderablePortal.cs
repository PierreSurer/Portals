using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderablePortal : MonoBehaviour
{
    [SerializeField]
    private GameObject rightEye;
    [SerializeField]
    private GameObject leftEye;
    [SerializeField]
    private GameObject portalCameraObj;
    [SerializeField]
    private GameObject sourcePortalObj;
    [SerializeField]
    private GameObject targetPortalObj;

    private GameObject mainCameraObj;
    private Camera mainCamera;
    private Camera portalCamera;

    public void Start()
    {
        mainCamera = Camera.main;
        mainCameraObj = mainCamera.gameObject;
        portalCamera = portalCameraObj.GetComponent<Camera>();
    }

    private GameObject GetEye ()
    {
        if (mainCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
            return rightEye;
        else
            return rightEye;
    }
    
    void OnWillRenderObject ()
    {
        if(sourcePortalObj && targetPortalObj)
        {
            // GameObject eye = Camera.main;

            // place portal camera behind target portal
            Matrix4x4 rotate = Matrix4x4.Rotate(new Quaternion(0, 1, 0, 0)); // 180deg rotation on axis y
            Matrix4x4 localTransform = sourcePortalObj.transform.worldToLocalMatrix * Camera.main.transform.localToWorldMatrix;
            Matrix4x4 globalTransform = (targetPortalObj.transform.localToWorldMatrix * rotate) * localTransform;
            portalCameraObj.transform.SetPositionAndRotation(globalTransform.GetColumn(3), globalTransform.rotation);

            // reset projection matrix
            portalCamera.projectionMatrix = Camera.main.projectionMatrix;

            // set portal camera clip plane to be on target portal, to avoid objects behind it to be visible.
            Vector3 normalCamSpace = portalCamera.worldToCameraMatrix.MultiplyVector(-targetPortalObj.transform.forward);
            Vector3 posCamSpace = portalCamera.worldToCameraMatrix.MultiplyPoint(targetPortalObj.transform.position);
            // posCamSpace -= normalCamSpace * 2.0f; // slight offset outwards
            float distCamSpace = -Vector3.Dot(posCamSpace, normalCamSpace);
            Vector4 clipPlaneCamSpace = new Vector4(normalCamSpace.x, normalCamSpace.y, normalCamSpace.z, distCamSpace);
            portalCamera.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCamSpace);

            portalCamera.Render();
        }
    }
}
