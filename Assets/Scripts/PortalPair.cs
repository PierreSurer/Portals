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

    public bool createPortal(int id, Transform newTransform)
    {
        switch (id)
        {
            case (0):
                if (redPortalInstance) Destroy(redPortalInstance);
                redPortalInstance = Instantiate(redPortalObject, newTransform);
                break;
            case (1):
                if (bluePortalInstance) Destroy(bluePortalInstance);
                bluePortalInstance = Instantiate(bluePortalObject, newTransform);
                break;
            default:
                return false;
        }
        return true;
    }
}
