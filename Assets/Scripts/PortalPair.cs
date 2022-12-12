using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour
{
    [SerializeField]
    private GameObject redPortalObject;
    [SerializeField]
    private GameObject bluePortalObject;

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

    public void createPortal(int id, Transform parentTransform, Vector3 position, Quaternion rotation)
    {
        GameObject portal = getPortalObject(id);
        portal.transform.parent = parentTransform;
        portal.transform.position = position;
        portal.transform.rotation = rotation;
        portal.SetActive(true);
    }



    public void deletePortal(int id)
    {
        getPortalObject(id).SetActive(false);
    }
}



