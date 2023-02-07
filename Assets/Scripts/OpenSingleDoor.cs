using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSingleDoor : MonoBehaviour
{
    public GameObject door;

    [SerializeField]
    private bool open = false;

    public bool Open
    {
        get { return open; }
        set { 
            open = value;
            elapsed = openingTime - Mathf.Min(elapsed, openingTime);
        }
    }

    [SerializeField, Range(0, 10)]
    private float openOffset;

    [SerializeField, Range(1, 10)]
    private float openingTime = 1.0f;

    private float elapsed = 10.0f;

    private Vector3 initialPos;
    void Start()
    {
        initialPos = door.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Min(elapsed / openingTime, 1.0f);
        t = t * t * (3 - 2 * t); //easing
        if (open)
        {
            door.transform.localPosition = Vector3.Lerp(initialPos, initialPos + new Vector3(0.0f, openOffset, 0.0f), t);
        } else
        {
            door.transform.localPosition = Vector3.Lerp(initialPos + new Vector3(0.0f, openOffset, 0.0f), initialPos, t);
        }
    }
}
