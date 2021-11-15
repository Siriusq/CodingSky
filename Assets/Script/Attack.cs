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
    public static bool isSlime = false;// Ifģ���������ж�ǰ����ɶ�Ĳ���
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
        yield return new WaitForSeconds(0.5f);//�ȴ�1s
        Movement.canFly = true;//����������ִֹͣ��
        yield return new WaitForSeconds(1f);//�ȴ�1s
        
        
        dog = GameObject.FindGameObjectWithTag("Player");
        dog.transform.GetComponent<Rigidbody>().freezeRotation = false;//�رչ�������ת����

        Tweener tweener = transform.DOMove(this.transform.position + transform.up, 0.1f);//������ɣ�
        AudioManager.actionListener = 5;

        yield return new WaitForSeconds(2f);//�ȴ�1s
        gameManager.Failed();
        AudioManager.actionListener = 1;
    }

    IEnumerator AttackWait()
    {
        // ��������ʱ�����ܹ�����ҵ���Ч��
        yield return new WaitForSeconds(0.5f);
        attacking.SetBool("GotHit", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        attackCount++;
        Debug.Log("Attack: " + attackCount);
    }
}
