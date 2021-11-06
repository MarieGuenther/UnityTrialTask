using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePan : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CurrentEditMode== GameManager.EditMode.Camera && GameManager.Instance.CurrentMode == GameManager.GameMode.Edit)
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
}