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
    public static bool isSlime = false;// Used in the if panel for determining objects in front of the character
    public GameManager gameManager;
    public bool attackTest = false;// Determines whether a character can attack

    void Start()
    {
        attacking = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider attackCollider)
    {
        if (attackCollider.tag == "Player")
        {
            isSlime = true;
            attackTest = true;
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
            isSlime = false;
        }
    }

    void OnTriggerExit(Collider leave)
    {
        if (leave.tag == "Player")//Slime attack characters
        {
            attackTest = false;
            Tweener tweener = transform.DOMove(this.transform.position - transform.up, 1.5f);
            isSlime = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Wait());        
        StopCoroutine(Wait());
        isSlime = false;
    }

    void OnCollisionExit(Collision collision)
    {
        isSlime = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        Movement.canFly = true;//Characters can be hit by Slimes
        yield return new WaitForSeconds(1f);
        
        
        dog = GameObject.FindGameObjectWithTag("Player");
        dog.transform.GetComponent<Rigidbody>().freezeRotation = false;//Turn off character rotation lock

        Tweener tweener = transform.DOMove(this.transform.position + transform.up, 0.1f);//Dog Flying!
        AudioManager.actionListener = 5;

        yield return new WaitForSeconds(2f);
        gameManager.Failed();
        AudioManager.actionListener = 1;
    }

    IEnumerator AttackWait()
    {
        // Adjust the animation time to achieve the effect of Slime disappearing after being attacked
        yield return new WaitForSeconds(0.5f);
        attacking.SetBool("GotHit", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        attackCount++;
        Debug.Log("Attack: " + attackCount);
    }
}
