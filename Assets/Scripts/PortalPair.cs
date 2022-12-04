using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour
{
    [SerializeField]
    private GameObject redPortalObject;
    [SerializeField]
    private GameObject bluePortalObject;

    private GameObject redPortalInstance;
    private GameObject bluePortalInstance;

    public GameObject getPortalObject(int id)
    {
        switch (id)
        {
            case (0):
                return redPortalObject;
            case (1):
                return bluePortalObject;
            default:
                return null;
        }
    }

    public bool createPortal(int id, Transform parentTransform, Vector3 position, Quaternion rotation)
    {
        switch (id)
        {
            case (0):
                if (redPortalInstance) Destroy(redPortalInstance);
                redPortalInstance = Instantiate(redPortalObject, parentTransform);

                PortalCamera redPortalCamera = redPortalInstance.GetComponentInChildren<PortalCamera>();
                redPortalCamera.mainCameraObj = this.gameObject;
                redPortalCamera.sourcePortalObj = redPortalInstance;
                if(bluePortalInstance)
                {
                    redPortalCamera.targetPortalObj = bluePortalInstance;
                    PortalCamera other = bluePortalInstance.GetComponentInChildren<PortalCamera>();
                    other.targetPortalObj = redPortalInstance;
                }
                    
                redPortalCamera.updateProperties();

                redPortalInstance.transform.position = position;
                redPortalInstance.transform.rotation = rotation;
                if (bluePortalInstance && parentTransform == bluePortalInstance.transform.parent)
                {
                    MeshRenderer thisMesh = redPortalInstance.GetComponent<MeshRenderer>();
                    MeshRenderer otherMesh = bluePortalInstance.GetComponent<MeshRenderer>();
                    if (thisMesh.bounds.Intersects(otherMesh.bounds)) Destroy(bluePortalInstance);
                }
                break;
            case (1):
                if (bluePortalInstance) Destroy(bluePortalInstance);
                bluePortalInstance = Instantiate(bluePortalObject, parentTransform);

                PortalCamera bluePortalCamera = bluePortalInstance.GetComponentInChildren<PortalCamera>();
                bluePortalCamera.mainCameraObj = this.gameObject;
                bluePortalCamera.sourcePortalObj = bluePortalInstance;
                if (redPortalInstance)
                {
                    bluePortalCamera.targetPortalObj = redPortalInstance;
                    PortalCamera other = redPortalInstance.GetComponentInChildren<PortalCamera>();
                    other.targetPortalObj = bluePortalInstance;
                }
                bluePortalCamera.updateProperties();

                bluePortalInstance.transform.position = position;
                bluePortalInstance.transform.rotation = rotation;
                if(redPortalInstance && parentTransform == redPortalInstance.transform.parent)
                {
                    MeshRenderer thisMesh = bluePortalInstance.GetComponent<MeshRenderer>();
                    MeshRenderer otherMesh = redPortalInstance.GetComponent<MeshRenderer>();
                    if(thisMesh.bounds.Intersects(otherMesh.bounds)) Destroy(redPortalInstance);
                }
                break;
            default:
                return false;
        }
        return true;
    }

    public bool deletePortal(int id)
    {
        switch (id)
        {
            case (0):
                if (redPortalInstance) Destroy(redPortalInstance);
                break;
            case (1):
                if (bluePortalInstance) Destroy(bluePortalInstance);
                break;
            default:
                return false;
        }
        return true;
    }
}



