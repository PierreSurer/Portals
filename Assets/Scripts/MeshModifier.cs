using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModifier : MonoBehaviour
{
    public PolygonCollider2D wallCollider;

    private MeshCollider selfCollider;
    Mesh generatedMesh;

    private void Awake()
    {
        selfCollider = GetComponent<MeshCollider>();
        calculateMesh();
    }

    public void calculateMesh() {
        wallCollider.pathCount = 1;
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            PolygonCollider2D collider;
            collider = child.GetComponent<PolygonCollider2D>();
            if(collider)
            {
                Debug.Log(i);
                Vector2[] positions = collider.GetPath(0);
                for(int j = 0; j < positions.Length; j++)
                {
                    positions[j] += (Vector2)collider.transform.position;
                }
                wallCollider.pathCount += 1;
                wallCollider.SetPath(wallCollider.pathCount - 1, positions);

            }
        }

        generatedMesh = wallCollider.CreateMesh(true, true);
        selfCollider.sharedMesh = generatedMesh;
    }
}
