using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public GameObject mainCameraObj;
    public GameObject sourcePortalObj;
    public GameObject targetPortalObj;
    private Camera mainCamera;
    private Camera portalCamera;

    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCameraObj.GetComponent<Camera>();
        portalCamera = GetComponent<Camera>();
        MeshFilter mf = sourcePortalObj.GetComponent<MeshFilter>();
        mesh = mf.mesh;
    }

    // Update is called once per frame
    void Update()
    {
        // position and orient portal camera behind target portal
        // Vector3 localPos = mainPortalObj.transform.InverseTransformPoint(mainCamera.transform.position);
        // Quaternion localRot = mainCamera.transform.rotation * Quaternion.Inverse(mainPortalObj.transform.rotation);

        // Vector3 targetPos = targetPortalObj.transform.TransformPoint(-localPos);
        // Quaternion halfTurn = new Quaternion(0, 1, 0, 0);
        // Quaternion targetRot = targetPortalObj.transform.rotation * halfTurn * localRot;
        // this.transform.SetPositionAndRotation(targetPos, targetRot);
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
