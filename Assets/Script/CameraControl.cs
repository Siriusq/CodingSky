using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/***************************************************************************************
*    Title: Implement scroll wheel button dragging and scroll wheel zooming under orthogonal camera
*    Author: Yukisora
*    Date: 2018
*    Code version: 1.0
*    Availability: https://www.cnblogs.com/Yukisora/p/8747167.html
*
***************************************************************************************/

public class CameraControl : MonoBehaviour
{
    //Scroll wheel to zoom, right click to drag
    private new Camera camera;
    private bool isDrag = false;//Whether it is in the dragging state
    private Vector3 startMousePosition;//The position of the mouse on the screen when you start dragging
    private Vector3 startCameraPosition;//The position of the camera on the world space when you start dragging
    [SerializeField]
    private float ScrollScale = 0.5f;
    private float temp;
    private float tempAxis;
    [SerializeField]
    private float dragScaleX = 0.001f;
    [SerializeField]
    private float dragScaleY = 0.001f;
    private Vector3 worldDir;

    private void Start()
    {
        camera = GetComponent<Camera>();
        temp = camera.orthographicSize;

        dragScaleX = 1.0f / camera.scaledPixelHeight;//Horizontal scaling value
        dragScaleY = 1.0f / camera.scaledPixelHeight;//Vertical scaling value
    }

    void Update()
    {
        Drag();
        Scale();
    }

    private void Scale()
    {
        tempAxis = Input.GetAxis("Mouse ScrollWheel");//Get scroll wheel input, -1/0/1
        if (tempAxis == 0) return;

        temp -= tempAxis * ScrollScale * temp;
        if (temp < 0)¡¡¡¡//Control does not allow the field of view to be negative, causing the content to be centered and symmetrical
        {
            temp += tempAxis * ScrollScale * temp;
            return;
        }
        camera.orthographicSize = temp;
    }

    private void Drag()
    {
        if (Input.GetMouseButtonDown(1))//Right click button
        {
            isDrag = true;
            startMousePosition = Input.mousePosition;//Record the mouse position before you start dragging
            startCameraPosition = transform.localPosition;//Record the camera position before you start dragging
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDrag = false;
        }
        MoveScene();
    }

    private void MoveScene()
    {
        if (!isDrag) return;

        worldDir = (startMousePosition - Input.mousePosition) * 2 * camera.orthographicSize;
        worldDir.x *= dragScaleX;
        worldDir.y *= dragScaleY;
        transform.localPosition = startCameraPosition + worldDir;
    }
}
