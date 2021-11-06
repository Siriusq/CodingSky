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
            yield return new WaitForSeconds(1);
            dogBehaviour.SetBool("isMove", false);
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
            direction = this.transform.position + leftRotation;
            //Tweener tweener = transform.DOLocalRotate(direction, 1);
            this.transform.Rotate(leftRotation);
        }
        else if (x.Equals("TurnRight"))
        {
            direction = this.transform.position + rightRotation;
            //Tweener tweener = transform.DOLocalRotate(direction, 1);
            this.transform.Rotate(rightRotation);
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
