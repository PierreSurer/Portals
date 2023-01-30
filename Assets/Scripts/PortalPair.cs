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

        if (other.activeInHierarchy)
        {
            portalMesh.sharedMaterial.SetFloat("_isTextured", 1.0f);
            otherPortalMesh.sharedMaterial.SetFloat("_isTextured", 1.0f);
            other.transform.parent.gameObject.GetComponent<PortalCollidable>().calculateMesh();
        }
        else
        {
            portalMesh.sharedMaterial.SetFloat("_isTextured", 0.0f);
            otherPortalMesh.sharedMaterial.SetFloat("_isTextured", 0.0f);
        }

        MeshRenderer surfaceMesh = parentTransform.parent.gameObject.GetComponent<MeshRenderer>();
        if (surfaceMesh.bounds.size.x < portalMesh.bounds.size.x || surfaceMesh.bounds.size.y < surfaceMesh.bounds.size.y)
            return;

        if (portal.activeInHierarchy)
        {
            Transform oldParent = portal.transform.parent;
            portal.transform.parent = parentTransform;
            oldParent.gameObject.GetComponent<PortalCollidable>().calculateMesh();
        }
        else
            portal.transform.parent = parentTransform;
        portal.transform.position = position;

        float xPos = portal.transform.localPosition.x;
        float yPos = portal.transform.localPosition.y;
        if (xPos - portalMesh.bounds.size.x / 2.0f < -surfaceMesh.bounds.size.x / 2.0f) xPos = -surfaceMesh.bounds.size.x / 2.0f + portalMesh.bounds.size.x / 2.0f;
        if (xPos + portalMesh.bounds.size.x / 2.0f > surfaceMesh.bounds.size.x / 2.0f) xPos = surfaceMesh.bounds.size.x / 2.0f - portalMesh.bounds.size.x / 2.0f;
        if (yPos - portalMesh.bounds.size.y / 2.0f < -surfaceMesh.bounds.size.y / 2.0f) yPos = -surfaceMesh.bounds.size.y / 2.0f + portalMesh.bounds.size.y / 2.0f;
        if (yPos + portalMesh.bounds.size.y / 2.0f > surfaceMesh.bounds.size.y / 2.0f) yPos = surfaceMesh.bounds.size.y / 2.0f - portalMesh.bounds.size.y / 2.0f;
        portal.transform.localPosition = new Vector3(xPos, yPos, portal.transform.localPosition.z);
        portal.transform.rotation = rotation;
        portal.SetActive(true);
    }

    public void deletePortal(int id)
    {
        getPortalObject(id).SetActive(false);
    }

    public bool isActive()
    {
        return redPortalObject.activeInHierarchy && bluePortalObject.activeInHierarchy;
    }
}



