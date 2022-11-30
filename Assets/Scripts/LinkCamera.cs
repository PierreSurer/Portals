using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camera;

    // Update is called once per frame
    void Update()
    {
        camera.transform.rotation = this.transform.rotation;
        camera.transform.position = this.transform.position;
    }
}
