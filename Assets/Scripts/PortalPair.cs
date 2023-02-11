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
        GameObject other = getPortalObject(1 - id);

        MeshRenderer portalMesh = portal.GetComponent<MeshRenderer>();
        MeshRenderer otherPortalMesh = other.GetComponent<MeshRenderer>();

        // Activate texture and portal transfer for other
        if (other.activeInHierarchy)
        {
            portalMesh.sharedMaterial.SetFloat("_isTextured", 1.0f);
            otherPortalMesh.sharedMaterial.SetFloat("_isTextured", 1.0f);
            other.transform.parent.gameObject.GetComponentInChildren<PortalCollidable>().calculateMesh();
        }
        else
        {
            portalMesh.sharedMaterial.SetFloat("_isTextured", 0.0f);
            otherPortalMesh.sharedMaterial.SetFloat("_isTextured", 0.0f);
        }

        portal.transform.position = position;
        portal.transform.rotation = rotation;

        if (portal.activeInHierarchy)
        {
            Transform oldParent = portal.transform.parent;
            portal.transform.SetParent(parentTransform ,true);
            oldParent.gameObject.GetComponentInChildren<PortalCollidable>().calculateMesh();
        }
        else {
            portal.transform.SetParent(parentTransform, true);
        }

        portal.SetActive(true);
        portal.transform.parent.gameObject.GetComponentInChildren<PortalCollidable>().calculateMesh();
    }

    public void deletePortal(int id)
    {
        GameObject portal = getPortalObject(id);
        PortalCollidable collidable = portal.transform.parent.GetComponentInChildren<PortalCollidable>();

        portal.transform.SetParent(this.transform, true);
        portal.SetActive(false);

        if (collidable)
            collidable.calculateMesh();
    }

    public bool isActive()
    {
        return redPortalObject.activeInHierarchy && bluePortalObject.activeInHierarchy;
    }
}



