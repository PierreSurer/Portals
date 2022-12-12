using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCameraObj;
    [SerializeField]
    private GameObject sourcePortalObj;
    [SerializeField]
    private GameObject targetPortalObj;

    private Camera mainCamera;
    private Camera portalCamera;

    public void updateProperties()
    {
        mainCamera = mainCameraObj.GetComponent<Camera>();
        portalCamera = GetComponent<Camera>();

    }

    void Update()
    {
        if(sourcePortalObj && targetPortalObj)
        {
            Matrix4x4 rotate = Matrix4x4.Rotate(new Quaternion(0, 1, 0, 0));
            Matrix4x4 localTransform = sourcePortalObj.transform.worldToLocalMatrix * mainCameraObj.transform.localToWorldMatrix;
            Matrix4x4 globalTransform = (targetPortalObj.transform.localToWorldMatrix * rotate) * localTransform;
            this.transform.SetPositionAndRotation(globalTransform.GetColumn(3), globalTransform.rotation);

            portalCamera.projectionMatrix = mainCamera.projectionMatrix;

            // set portal camera clip plane to be on target portal, to avoid objects behind it to be visible.
            Vector3 normalCamSpace = portalCamera.worldToCameraMatrix.MultiplyVector(-targetPortalObj.transform.forward);
            Vector3 posCamSpace = portalCamera.worldToCameraMatrix.MultiplyPoint(targetPortalObj.transform.position);
            float distCamSpace = -Vector3.Dot(posCamSpace, normalCamSpace);
            Vector4 clipPlaneCamSpace = new Vector4(normalCamSpace.x, normalCamSpace.y, normalCamSpace.z, distCamSpace);
            portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCamSpace);
        }
        
    }
}
