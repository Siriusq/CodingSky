using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    Animator attacking;
    public static int attackCount = 0;
    bool attack;
    GameObject dog;
    public static bool isSlime = false;// If模组中用来判断前面是啥的布尔
    public GameManager gameManager;

    void Start()
    {
        attacking = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider attackCollider)
    {
        if (attackCollider.tag == "Player")
        {
            isSlime = true;
        }        
    }

    void OnTriggerStay(Collider stay)
    {
        attack = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canAttack;
        if(stay.tag.Equals("Player") && attack)
        {
            StartCoroutine(AttackWait());
            attack = false;
            StopCoroutine(AttackWait());
        }
    }

    void OnTriggerExit(Collider leave)
    {
        if (leave.tag == "Player")
        {
            Tweener tweener = transform.DOMove(this.transform.position - transform.up, 1.5f);
            isSlime = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Wait());        
        StopCoroutine(Wait());
    }

    void OnCollisionExit(Collision collision)
    {

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);//等待1s
        Movement.canFly = true;//狗狗被顶飞停止执行
        yield return new WaitForSeconds(1f);//等待1s
        
        
        dog = GameObject.FindGameObjectWithTag("Player");
        dog.transform.GetComponent<Rigidbody>().freezeRotation = false;//关闭狗狗的旋转锁定

        Tweener tweener = transform.DOMove(this.transform.position + transform.up, 0.1f);//狗狗起飞！
        AudioManager.actionListener = 5;

        yield return new WaitForSeconds(2f);//等待1s
        gameManager.Failed();
        AudioManager.actionListener = 1;
    }

    IEnumerator AttackWait()
    {
        // 调整动画时间达成受攻击后挂掉的效果
        yield return new WaitForSeconds(0.5f);
        attacking.SetBool("GotHit", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        attackCount++;
        Debug.Log("Attack: " + attackCount);
    }
}
