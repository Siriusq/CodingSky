using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    //cite: https://www.cnblogs.com/Yukisora/p/8747167.html
    //�ĳɹ������ţ��Ҽ��϶�
    private new Camera camera;
    private bool isDrag = false;//�Ƿ����϶�״̬
    private Vector3 startMousePosition;//��ʼ�϶���ʱ���������Ļ�ϵ�λ��
    private Vector3 startCameraPosition;//��ʼ�϶���ʱ�����������ռ��ϵ�λ��
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

        dragScaleX = 1.0f / camera.scaledPixelHeight;//��������ֵ
        dragScaleY = 1.0f / camera.scaledPixelHeight;//��������ֵ
    }

    void Update()
    {
        Drag();//�϶�
        Scale();//��������
    }

    private void Scale()//��������
    {
        tempAxis = Input.GetAxis("Mouse ScrollWheel");//��ȡ�������룬-1/0/1
        if (tempAxis == 0) return;

        temp -= tempAxis * ScrollScale * temp;
        if (temp < 0)����//���Ʋ�����ҰΪ��ֵ���������ݱ����ĶԳ�
        {
            temp += tempAxis * ScrollScale * temp;
            return;
        }
        camera.orthographicSize = temp;
    }

    private void Drag()//�϶�
    {
        if (Input.GetMouseButtonDown(1))//�Ҽ���ť
        {
            isDrag = true;
            startMousePosition = Input.mousePosition;//��ʼ�϶�ǰ��¼���λ��
            startCameraPosition = transform.localPosition;//��ʼ�϶�ǰ��¼���λ��
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
