using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    public ArrayList received = new ArrayList();//按下开始按钮后接收到的指令组
    bool readin = false;//指示读取是否完成
    bool finished = false;//指示执行是否完成

    Vector3 destination;//移动到目的地
    Vector3 direction;//移动的方向

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    public Condition condition;
    private ArrayList conditionTemp = new ArrayList();

    private void Awake()
    {
        
        //condition.generateCodeBlocks();
    }

    void Start()
    {
        received = null;
        dogBehaviour = GetComponent<Animator>();

        GameObject ifElseCode = new GameObject();
        ifElseCode.AddComponent<Condition>();
        condition = (Condition)ifElseCode.GetComponent(typeof(Condition));
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
            if (s.Equals("if"))
            {
                //condition.generateCodeBlocks();
                //GameObject.FindGameObjectWithTag("If").SendMessage("generateCodeBlocks");
                Debug.Log("Before");
                condition.getCodeBlocks();
                yield return new WaitForSeconds(0.1f);
                //conditionTemp.Add("Attack");
                conditionTemp.AddRange(condition.getCodeBlocks());
                
                foreach (string x in conditionTemp)
                {
                    Move(x);
                    Debug.Log(x);
                    yield return new WaitForSeconds(1.5f);
                    dogBehaviour.SetBool("isMove", false);
                    canCollect = false;
                    canOPen = false;
                    canAttack = false;
                    dogBehaviour.SetBool("isCollecting", false);
                    dogBehaviour.SetBool("isOpen", false);
                    dogBehaviour.SetBool("isAttack", false);
                }
            }
            else
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

    public void GetCondition(ArrayList codes)
    {
        conditionTemp = new ArrayList();
        conditionTemp.AddRange(codes);
    }
}
