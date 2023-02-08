using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalCreator : MonoBehaviour
{
    public PortalPair pair;
    public UnityEngine.XR.InputDevice device;
    public XRController controller;

    [SerializeField, Range(1, 100)]
    public int errorCorrectionSteps = 20;

    private InputAction leftClick;
    private InputAction rightClick;
    private Camera viewCamera;

    private LayerMask hitLayer;

    private bool trigger; // gachette
    private int portalId = 0;

    void Start()
    {
        viewCamera = GetComponent<Camera>();

        hitLayer = LayerMask.GetMask("Portal Surface");

        leftClick = new InputAction(binding: "<Mouse>/leftButton");
        leftClick.performed += left => {
            Vector3 pos = viewCamera.transform.position;
            Vector3 coor = Mouse.current.position.ReadValue();
            Vector3 dir = viewCamera.ScreenPointToRay(coor).direction;
            ShootPortal(0, pos, dir);

        };
        leftClick.Enable();

        rightClick = new InputAction(binding: "<Mouse>/rightButton");
        rightClick.performed += right => {
            Vector3 pos = viewCamera.transform.position;
            Vector3 coor = Mouse.current.position.ReadValue();
            Vector3 dir = viewCamera.ScreenPointToRay(coor).direction;
            ShootPortal(1, pos, dir);
        };
        rightClick.Enable();
    }

    void ShootPortal(int id, Vector3 position, Vector3 direction, float distance = Mathf.Infinity)
    {
        RaycastHit hit;
        Physics.Raycast(position, direction, out hit, distance, hitLayer);
        if(hit.collider != null)
        {
            Vector3 portalPos = hit.point + 0.0001f * hit.normal;
            Quaternion portalRot = Quaternion.LookRotation(-hit.normal, Vector3.up); //TODO portal orientation with camera

            //Shoot rays on the corners
            Vector3 offset = pair.getPortalObject(id).transform.localScale / 2.0f;
            Vector3 xProj = portalRot * new Vector3(offset.x, 0.0f, 0.0f);
            Vector3 yProj = portalRot * new Vector3(0.0f, offset.y, 0.0f);

            bool bottomLeftValid = false;
            bool topLeftValid = false;
            bool bottomRightValid = false;
            bool topRightValid = false;
            int correctionIt = 0;

            LayerMask shootLayer = hitLayer | LayerMask.GetMask("Portal Blocker");

            while ((!bottomLeftValid || !topLeftValid || ! bottomRightValid || !topRightValid) && correctionIt < 2 * errorCorrectionSteps)
            {
                RaycastHit bottomLeftHit;
                Physics.Raycast(portalPos + hit.normal * 0.001f - xProj - yProj, direction, out bottomLeftHit, 0.05f, shootLayer);
                RaycastHit topLeftHit;
                Physics.Raycast(portalPos + hit.normal * 0.001f - xProj + yProj, direction, out topLeftHit, 0.05f, shootLayer);
                RaycastHit bottomRightHit;
                Physics.Raycast(portalPos + hit.normal * 0.001f + xProj - yProj, direction, out bottomRightHit, 0.05f, shootLayer);
                RaycastHit topRightHit;
                Physics.Raycast(portalPos + hit.normal * 0.001f + xProj + yProj, direction, out topRightHit, 0.05f, shootLayer);

                bottomLeftValid = bottomLeftHit.collider == hit.collider;
                topLeftValid = topLeftHit.collider == hit.collider;
                bottomRightValid = bottomRightHit.collider == hit.collider;
                topRightValid = topRightHit.collider == hit.collider;

                if (!bottomLeftValid)
                    portalPos += (xProj + yProj) / (float)errorCorrectionSteps;
                if (!topLeftValid)
                    portalPos += (xProj - yProj) / (float)errorCorrectionSteps;
                if (!bottomRightValid)
                    portalPos += (-xProj + yProj) / (float)errorCorrectionSteps;
                if (!topRightValid)
                    portalPos += (-xProj - yProj) / (float)errorCorrectionSteps;
                correctionIt++;
            }

            //Inside the wall with correction
            if (correctionIt < 20)
                pair.createPortal(id, hit.transform, portalPos, portalRot);
            
        }

    }

    void Update()
    {
        //bool lastTrigger = trigger;
        //controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out trigger);

        //if (!lastTrigger && trigger)
        //{
        //    RaycastHit hit;
        //    Ray r = new Ray(controller.transform.position, controller.transform.forward);
        //    if (Physics.Raycast(r, out hit, Mathf.Infinity, hitLayer))
        //    {
        //        RaycastHit hit2;
        //        if (!Physics.Raycast(r, out hit2, Mathf.Infinity, redBlockLayer) || hit2.transform.parent.gameObject != pair.getPortalObject(0))
        //        {
        //            createPortal(portalId, hit);
        //            portalId = (portalId + 1) % 2;
        //        }
        //    }
        //}
        //Debug.Log(trigger);
    }
}
