using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ʊ�ʯƯ����ת

public class GemFloating : MonoBehaviour
{
    float rotateSpeed = 45.0f;//ÿ����ת�Ƕ�
    float floatingAmplitude = 0.2f;//�������
    float frequency = 1f;//��Ƶ��

    Vector3 gemPosition = new Vector3();
    Vector3 tempPosition = new Vector3();

    void Start()
    {
        gemPosition = transform.position;
    }

    void Update()
    {
        //����Ч��
        tempPosition = gemPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * floatingAmplitude;
        transform.position = tempPosition;

        //��תЧ��
        Vector3 eulers = new Vector3(0f, Time.deltaTime * rotateSpeed, 0f);
        transform.Rotate(eulers, Space.World);       
    }
}
