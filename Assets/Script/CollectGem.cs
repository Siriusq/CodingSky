using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGem : MonoBehaviour
{
    public static int gemsCount = 0;//Gem Counter
    bool collect;
    public static bool isGem = false;
    public bool gemsTest = false;

    private void Start()
    {
        DOTween.Clear(true);
    }

    void OnTriggerStay(Collider gemCollider)//When the player stays in the gem collision body
    {
        collect = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canCollect;//Read the boolean value from the move script to determine if the star can be picked up
        isGem = true;
        if (gemCollider.tag == "Player" && collect)
        {
            //Debug.Log("Collected!");
            gemsCount++;
            AudioManager.actionListener = 3;
            Debug.Log(gemsCount);
            Destroy(gameObject);//Delete Gems 
            collect = false;
            isGem = false;
        }
    }

    void OnTriggerEnter(Collider gemCollider)//When the player enters the collision body of the jewel, the jewel is raised to prevent mold penetration
    {
        if (gemCollider.tag == "Player")
        {
            //Debug.Log("rrr!");
            gemsTest = true;
            Tweener tweener = transform.DOMove(this.transform.position + transform.forward, 1.2f);
        }
    }

    void OnTriggerExit(Collider gemCollider)
    {
        if (gemCollider.tag == "Player")
        {
            isGem = false;
            gemsTest = false;
        }
    }
}
