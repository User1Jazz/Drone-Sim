using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldEditor : MonoBehaviour
{
    List<WorldObject> objects;
    public GameObject selectedObject;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectObject();
    }

    #region editor functions

    public void AddObject(GameObject newObject)
    {
        objects.Add(newObject.GetComponent<WorldObject>());
        Instantiate(newObject, GetRayHitPoint(), Quaternion.identity);
        selectedObject = newObject;
    }

    public void DeleteSelectedObject()
    {
        int index = objects.IndexOf(selectedObject.GetComponent<WorldObject>());
        if (index > -1 && index < objects.Count)
        {
            objects.RemoveAt(index);
        }
        Destroy(selectedObject);
    }

    void SelectObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<Collider>() != null)
                {
                    selectedObject = hitInfo.collider.gameObject;
                    //Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                }
            }
            else if (!IsMouseOverUI())
            {
                selectedObject = null;
            }
        }
    }

    #endregion

    #region helper functions

    bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (LayerMask.LayerToName(raycastResultList[i].gameObject.layer) != "UI" || raycastResultList[i].gameObject.GetComponent<MouseClickThroughUI>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }

    Vector3 GetScreenCenter()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        return new Vector3(x, y, 0f);
    }

    Vector3 GetRayHitPoint()
    {
        Ray ray = cam.ScreenPointToRay(GetScreenCenter());

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.GetComponent<Collider>() != null)
            {
                return hitInfo.point;
            }
            else
            {
                return (cam.transform.position + cam.transform.forward);
            }
        }
        else
        {
            return (cam.transform.position + cam.transform.forward);
        }
    }

    #endregion
}
