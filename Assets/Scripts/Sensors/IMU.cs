using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMU : MonoBehaviour
{
    public bool calibrateOnStart = false;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 startRotation;
    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        if (calibrateOnStart)
        {
            CalibrateRotation();
        }
        else
        {
            startRotation = new Vector3(0f, 0f, 0f);
        }
    }

    // Update is called once per cycle
    void FixedUpdate()
    {
        currentPosition = transform.position - startPosition;
        currentRotation = transform.eulerAngles - startRotation;
    }

    void CalibrateRotation()
    {
        startRotation = transform.eulerAngles;
    }
}
