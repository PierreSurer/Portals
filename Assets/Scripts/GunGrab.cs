using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunGrab : MonoBehaviour
{
    public UnityEvent onTrigger;

    public Collider playerCollider;

    public GameObject gunParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(playerCollider))
        {
            onTrigger.Invoke();
            GameObject gun = this.transform.GetChild(0).gameObject;
            gun.transform.parent = gunParent.transform;
            gun.transform.localPosition = new Vector3();
            gun.transform.localRotation = new Quaternion();
        }
            
    }
}
