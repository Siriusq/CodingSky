using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelComplete : MonoBehaviour
{
    Animator chestBehaviour;
    bool open;

    void Start()
    {
        chestBehaviour = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider chestCollider)
    {
        if (chestCollider.tag == "Player")
        {
            //Debug.Log("Enter!");
            Tweener tweener = transform.DOMove(this.transform.position - transform.forward * 0.9f + transform.up * 0.2f, 1f);//箱子像后上移动，防止穿模
        }
    }

    void OnTriggerStay(Collider chestCollider)
    {
        open = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canOPen;
        if(chestCollider.tag == "Player" && open)
        {
            chestBehaviour.SetBool("reachTreasure", true);
            //Todo: 弹出通关弹窗
        }
    }
}
