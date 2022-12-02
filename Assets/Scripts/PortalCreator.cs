using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalCreator : MonoBehaviour
{
    private PortalPair pair;

    private InputAction leftClick;
    private InputAction rightClick;
    private Camera viewCamera;
    private LayerMask hitLayer;

    void Awake()
    {
        viewCamera = GetComponent<Camera>();
        pair = GetComponent<PortalPair>();

        hitLayer = LayerMask.GetMask("Portal Surface");
        leftClick = new InputAction(binding: "<Mouse>/leftButton");
        leftClick.performed += left => {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(viewCamera.ScreenPointToRay(coor), out hit, Mathf.Infinity, hitLayer))
            {
                createPortal(0, hit);
            }
        };
        leftClick.Enable();

        rightClick = new InputAction(binding: "<Mouse>/rightButton");
        rightClick.performed += right => {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(viewCamera.ScreenPointToRay(coor), out hit, Mathf.Infinity, hitLayer))
            {
                createPortal(1, hit);
            }
        };
        rightClick.Enable();
    }

    bool portalCompatibility(MeshRenderer surface, MeshRenderer portal)
    {
        return surface.bounds.size.x >= portal.bounds.size.x &&
            surface.bounds.size.y >= portal.bounds.size.y &&
            surface.bounds.size.z >= portal.bounds.size.z;
    }

    bool createPortal(int id, RaycastHit hit)
    {
        MeshRenderer surfaceMesh = hit.transform.gameObject.GetComponent<MeshRenderer>();
        MeshRenderer portalMesh = pair.getPortalObject(id).GetComponent<MeshRenderer>();
        if (portalCompatibility(surfaceMesh, portalMesh))
        {
            Debug.Log(hit.textureCoord2);
            Vector3 position = hit.point + 0.01f * hit.normal;
            Quaternion rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);


            Vector3 offset = new Vector3();
            float xPos = (1.0f - hit.textureCoord2.x) * surfaceMesh.bounds.size.x;
            float yPos = (1.0f - hit.textureCoord2.y) * surfaceMesh.bounds.size.y;
            if (xPos - portalMesh.bounds.size.x / 2.0f < 0.0f) offset.x = portalMesh.bounds.size.x / 2.0f - xPos;
            if (xPos + portalMesh.bounds.size.x / 2.0f > surfaceMesh.bounds.size.x) offset.x = surfaceMesh.bounds.size.x - (xPos + portalMesh.bounds.size.x / 2.0f);
            if (yPos - portalMesh.bounds.size.y / 2.0f < 0.0f) offset.y = portalMesh.bounds.size.y / 2.0f - yPos;
            if (yPos + portalMesh.bounds.size.y / 2.0f > surfaceMesh.bounds.size.y) offset.y = surfaceMesh.bounds.size.y - (yPos + portalMesh.bounds.size.y / 2.0f);
            Debug.Log(offset);
            position += rotation * offset;
            rotation *= Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            pair.createPortal(id, hit.transform, position, rotation);

        }
        return true;
    }
}
