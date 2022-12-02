using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour
{
    [SerializeField]
    private GameObject redPortalObject;
    [SerializeField]
    private GameObject bluePortalObject;

    //public GameObject RedPortalObject
    //{
    //    get { return redPortalObject; }
    //}

    //public GameObject BluePortalObject
    //{
    //    get { return bluePortalObject; }
    //}

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



