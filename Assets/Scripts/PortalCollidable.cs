using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollidable : MonoBehaviour
{
    private GameObject parent;
    private MeshCollider _3DCollider;

    private GameObject _2DManager;
    private PolygonCollider2D _2DCollider;

    private Mesh generatedMesh;

    private void Start()
    {
        parent = this.transform.parent.gameObject;
        _3DCollider = GetComponent<MeshCollider>();

        _2DManager = new GameObject("2D Manager");
        _2DManager.transform.parent = parent.transform;
        _2DManager.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
        _2DCollider = _2DManager.AddComponent<PolygonCollider2D>();
        //TODO other shapes?
        _2DCollider.pathCount = 1;
        Vector2[] positions = {
        new Vector2(5, 5),
        new Vector2(5, -5),
        new Vector2(-5, -5),
        new Vector2(-5, 5)};
        _2DCollider.SetPath(0, positions);

        if (!_3DCollider || !_2DCollider) Debug.LogError("Missing components");
        calculateMesh();
    }

    public void calculateMesh()
    {
        _2DCollider.pathCount = 1;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if(child.GetComponent<MeshRenderer>().sharedMaterial.GetFloat("_isTextured") != 1.0f) //If other portal is inactive (dirty)
                break;

            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            if (collider)
            {
                Vector2[] positions = collider.GetPath(0);
                for (int j = 0; j < positions.Length; j++)
                {
                    positions[j].x *= child.transform.localScale.x;
                    positions[j].y *= child.transform.localScale.y;
                    positions[j] += (Vector2)collider.transform.localPosition;
                }
                _2DCollider.pathCount += 1;
                _2DCollider.SetPath(_2DCollider.pathCount - 1, positions);

            }
        }
        generatedMesh = _2DCollider.CreateMesh(true, true);
        _3DCollider.sharedMesh = generatedMesh;
    }
}
