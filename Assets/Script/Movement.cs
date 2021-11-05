using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public ArrayList received = new ArrayList();
    bool readin = false;

    Vector3 up = Vector3.zero;
    Vector3 right = new Vector3(0, 90, 0);
    Vector3 down = new Vector3(0, 180, 0);
    Vector3 left = new Vector3(0, 270, 0);
    Vector3 currentDirection = Vector3.zero;

    Vector3 nextPos;
    Vector3 destination;
    Vector3 direction;

    float speed = 500f;
    float rayLength = 1f;

    bool canMove;

    void Start()
    {
        received = null;
    }

    void Update()
    {
        if (received != null && !readin)
        {
            StartCoroutine(Test());
            readin = true;
        }
    }

    IEnumerator Test()
    {
        Debug.Log("aaa");
        foreach (string s in received)
        {
            Move(s);
            Debug.Log(s);
            yield return new WaitForSeconds(1);            
        }
        yield return new WaitForSeconds(1);
    }

    public void Move(string x)
    {
        //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);


        
        if (x.Equals("MoveForward"))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + transform.forward, speed * Time.deltaTime);
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
