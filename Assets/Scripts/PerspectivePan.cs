using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePan : MonoBehaviour
{
    private Vector3 touchStart;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float groundZ = 0;
    [SerializeField]
    private float zoomOutMin = 1, zoomOutMax = 8;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentEditMode == GameManager.EditMode.Camera && GameManager.Instance.CurrentMode == GameManager.GameMode.Edit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = GetWorldPosition(groundZ);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - GetWorldPosition(groundZ);
                //cam.transform.position = new Vector3(cam.transform.position.x + direction.x, cam.transform.position.y, cam.transform.position.z + direction.z);
                cam.transform.position += direction;
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.down, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    void zoom(float increment)
    {
        //cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
        cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Clamp(cam.transform.position.y - increment, zoomOutMin, zoomOutMax), cam.transform.position.z);
    }
}