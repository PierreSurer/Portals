using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalCreator : MonoBehaviour
{
    public PortalPair pair;

    private InputAction leftClick;
    private InputAction rightClick;
    private Camera viewCamera;

    private LayerMask hitLayer;
    private LayerMask blueBlockLayer;
    private LayerMask redBlockLayer;

    void Start()
    {
        viewCamera = GetComponent<Camera>();

        hitLayer = LayerMask.GetMask("Portal Surface");
        blueBlockLayer = LayerMask.GetMask("Blue Ray Blocker");
        redBlockLayer = LayerMask.GetMask("Red Ray Blocker");


        leftClick = new InputAction(binding: "<Mouse>/leftButton");
        leftClick.performed += left => {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(viewCamera.ScreenPointToRay(coor), out hit, Mathf.Infinity, hitLayer))
            {
                RaycastHit hit2;
                if (!Physics.Raycast(viewCamera.ScreenPointToRay(coor), out hit2, Mathf.Infinity, blueBlockLayer) ||
                hit2.transform.parent.gameObject != pair.getPortalObject(1))
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
                RaycastHit hit2;
                if (!Physics.Raycast(viewCamera.ScreenPointToRay(coor), out hit2, Mathf.Infinity, redBlockLayer) ||
                hit2.transform.parent.gameObject != pair.getPortalObject(0))
                    createPortal(1, hit);
            }
        };
        rightClick.Enable();
    }

    void createPortal(int id, RaycastHit hit)
    {
        Vector3 position = hit.point + 0.0001f * hit.normal;
        Quaternion rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);

        pair.createPortal(id, hit.transform, position, rotation);

        hit.transform.gameObject.GetComponent<PortalCollidable>().calculateMesh();
    }
}
