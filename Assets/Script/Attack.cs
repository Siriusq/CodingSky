using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    Animator attacking;
    public int attackCount = 0;
    bool attack;
    GameObject dog;

    bool isSlime = false;// If模组中用来判断前面是啥的布尔
    public bool IsSlime => isSlime;

    // Start is called before the first frame update
    void Start()
    {
        attacking = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider attackCollider)
    {
        if (attackCollider.tag == "Player")
        {
            isSlime = true;
        }
        
    }

    public void OnTriggerStay(Collider stay)
    {
        attack = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canAttack;
        if(stay.tag.Equals("Player") && attack)
        {
            attackCount++;
            
            StartCoroutine(AttackWait());
            attack = false;


            StopCoroutine(AttackWait());
        }
    }

    public void OnTriggerExit(Collider leave)
    {
        if (leave.tag == "Player")
        {
            Tweener tweener = transform.DOMove(this.transform.position - transform.up, 1.5f);
            // Todo: 有空可以加攻击动画
            //Tweener tweener = transform.DOMove(this.transform.position + transform.up, 1.5f);//顶飞狗狗之后史莱姆冒头

            isSlime = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Wait());
        
        StopCoroutine(Wait());
    }

    public void OnCollisionExit(Collision collision)
    {

        //Tweener tweener = transform.DOMove(this.transform.position + transform.up, 1.5f);//顶飞狗狗之后史莱姆冒头

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);//等待1s
        
        dog = GameObject.FindGameObjectWithTag("Player");
        dog.transform.GetComponent<Rigidbody>().freezeRotation = false;//关闭狗狗的旋转锁定

        Tweener tweener = transform.DOMove(this.transform.position + transform.up, 0.1f);//狗狗起飞！
    }

    IEnumerator AttackWait()
    {
        // 调整动画时间达成受攻击后挂掉的效果
        yield return new WaitForSeconds(0.5f);
        attacking.SetBool("GotHit", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
