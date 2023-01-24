using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField]
    private PortalPair pair;

    private GameObject clone;
    private GameObject clonePortal;
    private GameObject otherPortal;
    private Collider[] portals;
    private Collider selfCollider;
    private Rigidbody rigidbody;

    private bool hit;

    void Start()
    {
        Destroy(clone);
        portals = new Collider[2];
        portals[0] = pair.getPortalObject(0).transform.GetComponentInChildren<MeshCollider>();
        portals[1] = pair.getPortalObject(1).transform.GetComponentInChildren<MeshCollider>();
        selfCollider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();

        hit = false;
    }

    private void FixedUpdate()
    {
        if (clone) // move clone
        {
            Vector3 relativePos = otherPortal.transform.worldToLocalMatrix * (otherPortal.transform.position - transform.position);

            if (relativePos.z < -0.1)
            {
                rigidbody.velocity = clonePortal.transform.localToWorldMatrix * (otherPortal.transform.worldToLocalMatrix * -rigidbody.velocity);
                transform.position = clone.transform.position;
                transform.rotation = clone.transform.rotation;

                (clonePortal, otherPortal) = (otherPortal, clonePortal);
            }

            Matrix4x4 rotate = Matrix4x4.Rotate(new Quaternion(0, 1, 0, 0));
            Matrix4x4 localTransform = otherPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix;
            Matrix4x4 globalTransform = (clonePortal.transform.localToWorldMatrix * rotate) * localTransform;
            clone.transform.SetPositionAndRotation(globalTransform.GetColumn(3), globalTransform.rotation);

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

                    clone = Instantiate(this.gameObject);
                    clone.GetComponent<Rigidbody>().useGravity = false;
                    clone.GetComponent<MovableObject>().enabled = false; //disable object behaviors
                }

            }
        }
    }
}
