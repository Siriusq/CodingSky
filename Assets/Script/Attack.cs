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

    bool isSlime = false;// Ifģ���������ж�ǰ����ɶ�Ĳ���
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
            // Todo: �пտ��Լӹ�������
            //Tweener tweener = transform.DOMove(this.transform.position + transform.up, 1.5f);//���ɹ���֮��ʷ��ķðͷ

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

        //Tweener tweener = transform.DOMove(this.transform.position + transform.up, 1.5f);//���ɹ���֮��ʷ��ķðͷ

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);//�ȴ�1s
        
        dog = GameObject.FindGameObjectWithTag("Player");
        dog.transform.GetComponent<Rigidbody>().freezeRotation = false;//�رչ�������ת����

        Tweener tweener = transform.DOMove(this.transform.position + transform.up, 0.1f);//������ɣ�
    }

    IEnumerator AttackWait()
    {
        // ��������ʱ�����ܹ�����ҵ���Ч��
        yield return new WaitForSeconds(0.5f);
        attacking.SetBool("GotHit", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
