using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control gem floating rotation
/***************************************************************************************
*    Title: Making Objects Float Up & Down in Unity
*    Author: Donovank Keith
*    Date: 2016
*    Code version: 1.0
*    Availability: http://www.donovankeith.com/2016/05/making-objects-float-up-down-in-unity/
*
***************************************************************************************/

public class GemFloating : MonoBehaviour
{
    float rotateSpeed = 45.0f;//Rotation angle per second
    float floatingAmplitude = 0.2f;//vertical amplitude
    float frequency = 1f;//Vibration frequency

    Vector3 gemPosition = new Vector3();
    Vector3 tempPosition = new Vector3();

    void Start()
    {
        gemPosition = transform.position;
    }

    void Update()
    {
        //Floating effect
        tempPosition = gemPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * floatingAmplitude;
        transform.position = tempPosition;

        //Rotation effect
        Vector3 eulers = new Vector3(0f, Time.deltaTime * rotateSpeed, 0f);
        transform.Rotate(eulers, Space.World);       
    }
}
