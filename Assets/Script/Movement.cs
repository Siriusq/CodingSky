using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    ArrayList received = new ArrayList();//按下开始按钮后接收到的指令组
    bool readin = false;//指示读取是否完成
    bool finished = false;//指示执行是否完成

    Vector3 destination;//移动到目的地
    Vector3 direction;//移动的方向

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    //下面是给if else的
    GameObject conditionIf; //if的面板
    GameObject conditionElse; //else的面板
    Dropdown conditionDropdown; //条件选择下拉菜单

    ArrayList codes = new ArrayList();//if执行得到的数组
    ArrayList conditionBlockList = new ArrayList();//其实和上面一样的东西，但是不这么搞有报错

    ArrayList ifCodeBlockTags = new ArrayList();//if中的代码
    ArrayList elseCodeBlockTags = new ArrayList();//else中的代码
    string ifBlock;
    string elseBlock;
    bool isGem;
    bool isSlime;

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
            if (s.Equals("if"))
            {
                conditionBlockList = new ArrayList();
                conditionBlockList.AddRange(ConditionBlocks());             
                yield return new WaitForSeconds(0.1f);                
                foreach (string x in conditionBlockList)
                {
                    Move(x);
                    Debug.Log("if " + x);
                    yield return new WaitForSeconds(1.5f);
                    boolControl();
                }
            }
            else
            {
                Move(s);
                Debug.Log(s);
                yield return new WaitForSeconds(1.5f);
                boolControl();
            }          
            
        }
        finished = true;
        yield return new WaitForSeconds(1);        
    }

    private void boolControl()
    {
        dogBehaviour.SetBool("isMove", false);
        canCollect = false;
        canOPen = false;
        canAttack = false;
        dogBehaviour.SetBool("isCollecting", false);
        dogBehaviour.SetBool("isOpen", false);
        dogBehaviour.SetBool("isAttack", false);
    }

    private void Move(string x)//狗狗动作代码
    {                
        if (x.Equals("MoveForward"))
        {
            dogBehaviour.SetBool("isMove", true);
            destination = this.transform.position + transform.forward;
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOMove(destination, 1);
        }
        else if (x.Equals("TurnLeft"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y -= 90;
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
        }
        else if (x.Equals("TurnRight"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y += 90;
            AudioManager.actionListener = 4;
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
            AudioManager.actionListener = 2;
        }
        else if (x.Equals("Treasure"))
        {
            dogBehaviour.SetBool("isOpen", true);
            canOPen = true;
            AudioManager.actionListener = 6;
        }               
    }

    public void GetCode(ArrayList codes)
    {
        received = new ArrayList();
        received.AddRange(codes);
    }

    private ArrayList ConditionBlocks()//判断执行if还是else的代码
    {
        codes = new ArrayList();
        ifCodeBlockTags = new ArrayList();
        elseCodeBlockTags = new ArrayList();
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        conditionDropdown = GameObject.FindGameObjectWithTag("ConditionDropdown").GetComponent<Dropdown>();

        isGem = CollectGem.isGem;
        isSlime = Attack.isSlime;

        int option = conditionDropdown.value;//下拉菜单的选项，0是史莱姆，1是宝石

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //判断框里是不是循环 都直接用数组存了，好回传
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
                codes.AddRange(ifCodeBlockTags);
            }
            else if (isSlime && !isGem)
            {
                codes.AddRange(elseCodeBlockTags);
            }
        }

        isSlime = false;
        isGem = false;

        return codes;
    }

    private ArrayList isLoop(string s)// 判断if里面有没有循环或者子循环
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))//有循环就把循环加到数组
        {
            temp.AddRange(Execute.LoopBlockTags);
        }
        else if (s.Equals("SubLoop"))//有子循环就把子循环加到数组
        {
            temp.AddRange(Execute.SubLoopBlockTags);
        }
        else
        {
            temp.Add(s);//没有就只加原来的命令
        }

        return temp;
    }
}
