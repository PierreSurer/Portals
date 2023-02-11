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

    // press de boutons
    private bool trigger;
    private bool menu;

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
        GameObject portal = pair.getPortalObject(id);
        RaycastHit hit;
        Physics.Raycast(position, direction, out hit, distance, hitLayer);
        if(hit.collider != null)
        {
            // remove portal from collider
            if (portal.transform.parent == hit.transform)
            {
                pair.deletePortal(id);
            }
        
            Vector3 portalPos = hit.point + 0.01f * hit.normal;
            Quaternion portalRot = Quaternion.LookRotation(-hit.normal, Vector3.up); //TODO portal orientation with camera

            //Shoot rays on the corners
            Vector3 offset = portal.transform.lossyScale / 2.0f;
            Vector3 xProj = portalRot * new Vector3(offset.x, 0.0f, 0.0f);
            Vector3 yProj = portalRot * new Vector3(0.0f, offset.y, 0.0f);

            bool bottomLeftValid = false;
            bool topLeftValid = false;
            bool bottomRightValid = false;
            bool topRightValid = false;
            int correctionIt = 0;

            LayerMask shootLayer = hitLayer;
            if(id == 0)
                shootLayer = shootLayer | LayerMask.GetMask("Blue Portal Blocker");
            else
                shootLayer = shootLayer | LayerMask.GetMask("Red Portal Blocker");

            while ((!bottomLeftValid || !topLeftValid || !bottomRightValid || !topRightValid) && correctionIt < errorCorrectionSteps)
            {
                RaycastHit bottomLeftHit, topLeftHit, bottomRightHit, topRightHit;
                Physics.Raycast(portalPos + hit.normal * 0.1f - xProj - yProj, -hit.normal, out bottomLeftHit, 0.5f, shootLayer);
                Physics.Raycast(portalPos + hit.normal * 0.1f - xProj + yProj, -hit.normal, out topLeftHit, 0.5f, shootLayer);
                Physics.Raycast(portalPos + hit.normal * 0.1f + xProj - yProj, -hit.normal, out bottomRightHit, 0.5f, shootLayer);
                Physics.Raycast(portalPos + hit.normal * 0.1f + xProj + yProj, -hit.normal, out topRightHit, 0.5f, shootLayer);

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
            if (correctionIt < errorCorrectionSteps)
                pair.createPortal(id, hit.transform, portalPos, portalRot);
        }

    }

    void Update()
    {
        bool lastTrigger = trigger;
        controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out trigger);

        bool lastMenu = menu;
        controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out menu);

        if (!lastTrigger && trigger || !lastMenu && menu)
        {
            Vector3 pos = controller.transform.position;
            Vector3 dir = controller.transform.forward;
            ShootPortal(trigger ? 0 : 1, pos, dir);
            Debug.Log("shoot portal");
        }
    }
}
