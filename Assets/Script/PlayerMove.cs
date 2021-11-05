using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    Vector3 up = Vector3.zero;
    Vector3 right = new Vector3(0, 90, 0);
    Vector3 down = new Vector3(0, 180, 0);
    Vector3 left = new Vector3(0, 270, 0);
    Vector3 currentDirection = Vector3.zero;

    //Todo: 11
    Vector3 upRight = new Vector3(0, 90, 0);
    Vector3 upLeft = new Vector3(0, 0, 0);
    Vector3 downRight = new Vector3(0, 180, 0);
    Vector3 downLeft = new Vector3(0, 270, 0);
    Vector3 playerFace = Vector3.zero;
    //Todo: 00
    Vector3 nextPos;
    Vector3 destination;
    Vector3 direction;
    //Todo: 00
    float speed = 5f;
    float rayLength = 1f;
    //Todo: 00
    bool canMove;
    private Vector3 dogDirection = new Vector3(0, 90, 0);

    public ArrayList orders = new ArrayList();
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = right;
        nextPos = Vector3.right;
        destination = transform.position;

        //Todo: 11
        playerFace = upRight;
        nextPos = Vector3.right;
        destination = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Move();        
/*        if(orders != null )
        {
            //string x = orders.GetRange(count,1);
            Run(orders.GetRange(count, 1));
            count++;
            //Walk(orders);
        }*/
    }

/*    public void getTags(ArrayList orders)
    {
        this.orders = orders;        
    }*/

/*    public void Run(ArrayList a)
    {
        string s = "";
        foreach(string x in a)
        {
            s = x;
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //foreach (string s in codeBlocks)
        //{
            //开始的方向是90，左转就是-90，右转就是+90，如果现在的方向是90，移动Vector3.后面就是right；0就是up，180就是down，270就是left
            if (s.Equals("TurnLeft"))
            {
                dogDirection.y -= 90;

            }
            if (s.Equals("TurnRight"))
            {
                dogDirection.y += 90;
            }

            if (s.Equals("MoveForward"))
            {
                if (dogDirection.y == 90)
                {
                    nextPos = Vector3.right;
                    playerFace = upRight;
                    canMove = true;
                }
                if (dogDirection.y == 0)
                {
                    nextPos = Vector3.up;
                    playerFace = upLeft;
                    canMove = true;
                }
                if (dogDirection.y == 180)
                {
                    nextPos = Vector3.down;
                    playerFace = downRight;
                    canMove = true;
                }
                if (dogDirection.y == 270)
                {
                    nextPos = Vector3.left;
                    playerFace = downLeft;
                    canMove = true;
                }
            }


            if (Vector3.Distance(destination, transform.position) <= 0.00001f)
            {
                transform.localEulerAngles = dogDirection;
                if (canMove)
                {
                    if (Valid())
                    {
                        destination = transform.position + nextPos;
                        direction = nextPos;
                        canMove = false;
                    }

                }

            }
        //}

    }*/

    public void Walk(ArrayList codeBlocks)
    {
        
        foreach (string s in codeBlocks)
        {
            //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            //开始的方向是90，左转就是-90，右转就是+90，如果现在的方向是90，移动Vector3.后面就是right；0就是up，180就是down，270就是left
            if (s.Equals("TurnLeft"))
            {
                dogDirection.y -= 90;

            }
            if (s.Equals("TurnRight"))
            {
                dogDirection.y += 90;
            }

            if (s.Equals("MoveForward"))
            {
                if(dogDirection.y == 90)
                {
                    nextPos = Vector3.right;
                    playerFace = upRight;
                    canMove = true;
                }
                if(dogDirection.y == 0)
                {
                    nextPos = Vector3.up;
                    playerFace = upLeft;
                    canMove = true;
                }
                if (dogDirection.y == 180)
                {
                    nextPos = Vector3.down;
                    playerFace = downRight;
                    canMove = true;
                }
                if (dogDirection.y == 270)
                {
                    nextPos = Vector3.left;
                    playerFace = downLeft;
                    canMove = true;
                }
            }


/*            if (Vector3.Distance(destination, transform.position) <= 0.00001f)
            {
                transform.localEulerAngles = dogDirection;
                if (canMove)
                {
                    if (Valid())
                    {
                        destination = transform.position + nextPos;
                        direction = nextPos;
                        canMove = false;
                    }

                }

            }*/
        }

    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination,speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
            canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDirection = down;
            canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
        }

        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            //transform.localEulerAngles = currentDirection;
            transform.localEulerAngles = dogDirection;
            if (canMove)
            {
                if (Valid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }

            }

        }
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit, rayLength))
        {
            if(hit.collider.tag == "Gem")
            {
                return false;
            }
        }
        return true;
    }
}
