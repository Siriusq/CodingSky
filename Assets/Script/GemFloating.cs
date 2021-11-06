using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 控制宝石漂浮旋转

public class GemFloating : MonoBehaviour
{
    float rotateSpeed = 45.0f;//每秒旋转角度
    float floatingAmplitude = 0.2f;//上下振幅
    float frequency = 1f;//振动频率

    Vector3 gemPosition = new Vector3();
    Vector3 tempPosition = new Vector3();

    void Start()
    {
        gemPosition = transform.position;
    }

    void Update()
    {
        //浮动效果
        tempPosition = gemPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * floatingAmplitude;
        transform.position = tempPosition;

        //旋转效果
        Vector3 eulers = new Vector3(0f, Time.deltaTime * rotateSpeed, 0f);
        transform.Rotate(eulers, Space.World);       
    }
}
