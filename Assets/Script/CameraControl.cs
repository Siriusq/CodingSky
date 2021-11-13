using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    //cite: https://www.cnblogs.com/Yukisora/p/8747167.html
    //改成滚轮缩放，右键拖动
    private new Camera camera;
    private bool isDrag = false;//是否处在拖动状态
    private Vector3 startMousePosition;//开始拖动的时候鼠标在屏幕上的位置
    private Vector3 startCameraPosition;//开始拖动的时候相机在世界空间上的位置
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

        dragScaleX = 1.0f / camera.scaledPixelHeight;//横向缩放值
        dragScaleY = 1.0f / camera.scaledPixelHeight;//纵向缩放值
    }

    void Update()
    {
        Drag();//拖动
        Scale();//滚轮缩放
    }

    private void Scale()//滚轮缩放
    {
        tempAxis = Input.GetAxis("Mouse ScrollWheel");//获取滚轮输入，-1/0/1
        if (tempAxis == 0) return;

        temp -= tempAxis * ScrollScale * temp;
        if (temp < 0)　　//控制不让视野为负值，导致内容被中心对称
        {
            temp += tempAxis * ScrollScale * temp;
            return;
        }
        camera.orthographicSize = temp;
    }

    private void Drag()//拖动
    {
        if (Input.GetMouseButtonDown(1))//右键按钮
        {
            isDrag = true;
            startMousePosition = Input.mousePosition;//开始拖动前记录鼠标位置
            startCameraPosition = transform.localPosition;//开始拖动前记录相机位置
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
