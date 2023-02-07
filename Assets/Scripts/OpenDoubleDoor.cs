using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoubleDoor : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;

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

    private Vector3 rightInitialPos;
    private Vector3 leftInitialPos;
    void Start()
    {
        rightInitialPos = rightDoor.transform.localPosition;
        leftInitialPos = leftDoor.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Min(elapsed / openingTime, 1.0f);
        t = t * t * (3 - 2 * t); //easing
        if (open)
        {
            rightDoor.transform.localPosition = Vector3.Lerp(rightInitialPos, rightInitialPos + new Vector3(0.0f, 0.0f, -openOffset), t);
            leftDoor.transform.localPosition = Vector3.Lerp(leftInitialPos, leftInitialPos + new Vector3(0.0f, 0.0f, openOffset), t);
        } else
        {
            rightDoor.transform.localPosition = Vector3.Lerp(rightInitialPos + new Vector3(0.0f, 0.0f, -openOffset), rightInitialPos, t);
            leftDoor.transform.localPosition = Vector3.Lerp(leftInitialPos + new Vector3(0.0f, 0.0f, openOffset), leftInitialPos, t);
        }
    }
}
