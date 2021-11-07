using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Use to restart the game
// 在编辑模式下重新开始会导致光源消失，unity的锅，build之后运行没有问题
public class Restart : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene("level1");
    }

}
