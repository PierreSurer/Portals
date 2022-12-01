using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainCamera;

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.rotation = this.transform.rotation;
        mainCamera.transform.position = this.transform.position;
    }
}
