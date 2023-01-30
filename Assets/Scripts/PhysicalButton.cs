using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public UnityEvent onPress;
    public UnityEvent onRelease;

    private List<Collider> colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!colliders.Contains(other))
        {
            if (colliders.Count == 0)
            {
                this.transform.localPosition -= new Vector3(0, 1.2f, 0);
                onPress.Invoke();
            }    
            colliders.Add(other);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        if(colliders.Count == 0)
        {
            onRelease.Invoke();
            this.transform.localPosition += new Vector3(0, 1.2f, 0);
        }
        
    }
}
