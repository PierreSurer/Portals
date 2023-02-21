using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class PortalTransfer : MonoBehaviour
{
    [SerializeField]
    private PortalPair pair;

    private GameObject clone;
    private GameObject clonePortal;
    private GameObject otherPortal;
    private Collider[] portals;
    private Collider selfCollider;
    // private CharacterController charcontroller;

    private bool hit;

    void Start()
    {
        Destroy(clone);
        portals = new Collider[2];
        portals[0] = pair.getPortalObject(0).transform.GetComponentInChildren<BoxCollider>();
        portals[1] = pair.getPortalObject(1).transform.GetComponentInChildren<BoxCollider>();
        selfCollider = GetComponentInChildren<Collider>();
        // charcontroller = GetComponent<CharacterController>();

        hit = false;
    }

    private void FixedUpdate()
    {
        if (clone) // move clone
        {
            Matrix4x4 rotate = Matrix4x4.Rotate(new Quaternion(0, 1, 0, 0));
            Matrix4x4 localTransform = otherPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix;
            Matrix4x4 globalTransform = (clonePortal.transform.localToWorldMatrix * rotate) * localTransform;
            clone.transform.SetPositionAndRotation(globalTransform.GetColumn(3), globalTransform.rotation);

            Vector3 relativePos = otherPortal.transform.worldToLocalMatrix * (otherPortal.transform.position - transform.position);

            // swap object and clone if it went through the portal
            if (relativePos.z < 0.0f)
            {
                // charcontroller.SimpleMove(clonePortal.transform.localToWorldMatrix * (otherPortal.transform.worldToLocalMatrix * -charcontroller.velocity));
                (transform.position, clone.transform.position) = (clone.transform.position, transform.position);
                (transform.rotation, clone.transform.rotation) = (clone.transform.rotation, transform.rotation);
                (clonePortal, otherPortal) = (otherPortal, clonePortal);
                // transform.rotation = Quaternion.Euler(0.0f, transform.rotation.y, 0.0f);
            }


            if (!hit)
            {
                DestroyImmediate(clone);
            }
        }
    }

    void Update()
    {
        hit = false;
        for (int i = 0; i < portals.Length; i++)
        {
            if (portals[i].bounds.Intersects(selfCollider.bounds))
            {
                hit = true;
                if (!clone && pair.isActive())
                {
                    clonePortal = pair.getPortalObject(1 - i);
                    otherPortal = pair.getPortalObject(i);

                    // clone only the gameobject that hold the mesh
                    clone = new GameObject();
                    var child = Instantiate(selfCollider.GetComponentInChildren<MeshRenderer>().gameObject);
                    child.transform.parent = clone.transform;
                }
            }
        }
    }
}
