using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGem : MonoBehaviour
{
    public static int gemsCount = 0;//宝石计数器
    bool collect;
    public static bool isGem = false;//If模组中用来判断前面是不是宝石的布尔

    void OnTriggerStay(Collider gemCollider)//当玩家在宝石碰撞体里停留时
    {
        collect = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canCollect;//读取移动脚本里的布尔值，判断能否拾取星星
        isGem = true;
        if (gemCollider.tag == "Player" && collect)
        {
            //Debug.Log("Collected!");
            gemsCount++;
            Debug.Log(gemsCount);
            Destroy(gameObject);//删除宝石（相当于吃掉
            collect = false;
        }
    }

    void OnTriggerEnter(Collider gemCollider)//玩家进入宝石的碰撞体时，升起宝石防止穿模
    {
        if (gemCollider.tag == "Player")
        {
            //Debug.Log("Enter!");
            Tweener tweener = transform.DOMove(this.transform.position + transform.forward, 1.2f);
        }
    }

    void OnTriggerExit(Collider gemCollider)
    {
        if (gemCollider.tag == "Player")
        {
            isGem = false;
        }
    }
}
