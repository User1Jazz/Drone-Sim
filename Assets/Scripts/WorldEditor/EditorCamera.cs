using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorCamera : MonoBehaviour
{
    // Camera control
    Vector3 dragOrigin;
    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        MoveCamera();
        LookAround();
    }

    #region camera functions

    void MoveCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            transform.Translate(-difference * 0.1f * Time.deltaTime, Space.Self);
        }
        if (Input.GetMouseButtonUp(2))
            dragOrigin = Vector3.zero;
    }

    void LookAround()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse Y") * -1f;
            rotationY += Input.GetAxis("Mouse X") * 1f;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
        }
    }

    #endregion
}
