using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelComplete : MonoBehaviour
{
    public GameManager gameManager;
    Animator chestBehaviour;
    bool open;
    public bool enter = false;

    void Start()
    {
        chestBehaviour = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider chestCollider)
    {
        if (chestCollider.tag == "Player")
        {
            //Debug.Log("Enter!");
            enter = true;
            Tweener tweener = transform.DOMove(this.transform.position - transform.forward * 0.9f + transform.up * 0.2f, 1f);//箱子像后上移动，防止穿模
        }
    }

    void OnTriggerStay(Collider chestCollider)
    {
        open = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canOPen;
        if(chestCollider.tag == "Player" && open)
        {
            chestBehaviour.SetBool("reachTreasure", true);
            // 弹出通关弹窗
            StartCoroutine(PopUpTrans());
            StopCoroutine(PopUpTrans());
            
        }
    }

    IEnumerator PopUpTrans()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.LevelComplete();
    }
}
