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

    void Update()
    {

    }

    bool portalCompatibility(GameObject portal)
    {
        return true;
    }


    bool createPortal(int id, RaycastHit hit)
    {
        if(portalCompatibility(pair.getPortalObject(id)))
        {
            Transform newTransform = hit.transform;
            newTransform.position = hit.point + 0.01f * hit.normal;
            transform.forward = -hit.normal;
            Debug.Log(id);
            pair.createPortal(id, newTransform);
        }
        return true;
    }
}
