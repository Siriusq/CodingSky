using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Use to restart the game
// �ڱ༭ģʽ�����¿�ʼ�ᵼ�¹�Դ��ʧ��unity�Ĺ���build֮������û������
public class Restart : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene("level1");
    }

}
