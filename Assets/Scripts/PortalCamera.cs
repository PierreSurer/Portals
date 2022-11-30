using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public GameObject mainCameraObj;
    public GameObject mainPortalObj;
    public GameObject targetPortalObj;
    private Camera mainCamera;
    private Camera portalCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCameraObj.GetComponent<Camera>();
        portalCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = mainCamera.transform.position - mainPortalObj.transform.position;
        transform.position = targetPortalObj.transform.position + offset;
        transform.rotation = mainCamera.transform.rotation;
        portalCamera.nearClipPlane = 5;
    }
}
