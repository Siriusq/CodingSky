using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    public ArrayList received = new ArrayList();
    bool readin = false;
    bool finished = false;

    Vector3 leftRotation = new Vector3(0, -90, 0);
    Vector3 rightRotation = new Vector3(0, 90, 0);

    Vector3 nextPos;
    Vector3 destination;
    Vector3 direction;

    float speed = 500f;
    float rayLength = 1f;
    Vector3 currentVelocity = Vector3.zero;
    float smoothTime = 3F;

    bool canMove;
    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    void Start()
    {
        received = null;
        dogBehaviour = GetComponent<Animator>();
    }

    void Update()
    {
        if (received != null && !readin)
        {
            StartCoroutine(ExecuteBlocks());
            readin = true;
        }

        if (finished)
        {
            StopCoroutine(ExecuteBlocks());
        }

    }

    IEnumerator ExecuteBlocks()
    {
        Debug.Log("Start");
        foreach (string s in received)
        {          
            Move(s);
            Debug.Log(s);            
            yield return new WaitForSeconds(1.5f);
            dogBehaviour.SetBool("isMove", false);
            canCollect = false;
            canOPen = false;
            canAttack = false;
            dogBehaviour.SetBool("isCollecting", false);
            dogBehaviour.SetBool("isOpen", false);
            dogBehaviour.SetBool("isAttack", false);
        }
        finished = true;
        yield return new WaitForSeconds(1);        
    }

    public void Move(string x)
    {
        //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                
        if (x.Equals("MoveForward"))
        {
            dogBehaviour.SetBool("isMove", true);
            destination = this.transform.position + transform.forward;
            Tweener tweener = transform.DOMove(destination, 1);
            //this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + transform.forward, speed * Time.deltaTime);
            //Debug.Log(Time.deltaTime);
            //this.transform.position = Vector3.SmoothDamp(this.transform.position, this.transform.position + transform.forward, ref currentVelocity, 0.1f);
            //this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + transform.forward, speed * Time.deltaTime);
        }
        else if (x.Equals("TurnLeft"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y -= 90;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
            //this.transform.Rotate(leftRotation);
        }
        else if (x.Equals("TurnRight"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y += 90;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
            //this.transform.Rotate(rightRotation);
        }
        else if (x.Equals("Collect"))
        {
            dogBehaviour.SetBool("isCollecting", true);
            canCollect = true;
        }
        else if (x.Equals("Attack"))
        {
            dogBehaviour.SetBool("isAttack", true);
            canAttack = true;
        }
        else if (x.Equals("Treasure"))
        {
            dogBehaviour.SetBool("isOpen", true);
            canOPen = true;
        }
    }

    public void GetCode(ArrayList codes)
    {
        received = new ArrayList();
        received.AddRange(codes);
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(myRay, out hit, rayLength))
        {
            if (hit.collider.tag == "Gem")
            {
                return false;
            }
        }
        return true;
    }
}
