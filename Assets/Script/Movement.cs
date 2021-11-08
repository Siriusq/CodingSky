using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //Todo: 下面是condition嫁过来的，测试用。。。
    private GameObject conditionIf; //if的面板
    private GameObject conditionElse; //else的面板
    private Dropdown conditionDropdown; //条件选择下拉菜单

    public ArrayList codes = new ArrayList();//if执行得到的数组
    public ArrayList conditionBlockList = new ArrayList();//其实和上面一样的东西，但是不这么搞有报错

    private ArrayList ifCodeBlockTags = new ArrayList();
    private ArrayList elseCodeBlockTags = new ArrayList();

    string ifBlock;
    string elseBlock;

    bool isGem;
    bool isSlime;

    public Execute execute;
    //Todo: 淦..............................................................

    void Start()
    {
        received = null;
        dogBehaviour = GetComponent<Animator>();

/*        GameObject ifElseCode = new GameObject();
        ifElseCode.AddComponent<Condition>();
        condition = (Condition)ifElseCode.GetComponent(typeof(Condition));*/
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
                conditionBlockList = new ArrayList();
                conditionBlockList.AddRange(ConditionBlocks());
                Debug.Log("Before");                
                yield return new WaitForSeconds(0.1f);
                
                foreach (string x in conditionBlockList)
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
        if (x.Equals("MoveForward"))
        {
            dogBehaviour.SetBool("isMove", true);
            destination = this.transform.position + transform.forward;
            Tweener tweener = transform.DOMove(destination, 1);
        }
        else if (x.Equals("TurnLeft"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y -= 90;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
        }
        else if (x.Equals("TurnRight"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y += 90;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
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

    public ArrayList ConditionBlocks()
    {
        codes = new ArrayList();
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        conditionDropdown = GameObject.FindGameObjectWithTag("ConditionDropdown").GetComponent<Dropdown>();

        isGem = GameObject.FindGameObjectWithTag("Gem").GetComponent<CollectGem>().isGem;
        isSlime = GameObject.FindGameObjectWithTag("Slime").GetComponent<Attack>().isSlime;

        int option = conditionDropdown.value;//下拉菜单的选项，0是史莱姆，1是宝石

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //判断框里是不是循环 都直接用数组存算了，好回传
        ifCodeBlockTags.AddRange(isLoop(ifBlock));
        elseCodeBlockTags.AddRange(isLoop(elseBlock));

        if (option == 0)
        {
            if (isSlime && !isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else if (!isSlime && isGem)
            {
                codes.AddRange(elseCodeBlockTags);
            }
        }
        else
        {
            if (!isSlime && isGem)
            {
                codes.AddRange(elseCodeBlockTags);
            }
            else if (isSlime && !isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
        }

        isSlime = false;
        isGem = false;

        return codes;
    }

    public ArrayList isLoop(string s)// 判断if里面有没有循环或者子循环
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))//有循环就把循环加到数组
        {
            temp.AddRange(execute.LoopArray());
        }
        else if (s.Equals("SubLoop"))//有子循环就把子循环加到数组
        {
            temp.AddRange(execute.SubLoopArray());
        }
        else
        {
            temp.Add(s);//没有就只加原来的命令
        }

        return temp;
    }
}
