using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    ArrayList received = new ArrayList();//按下开始按钮后接收到的指令组
    public ArrayList Received { get { return received; } }
    bool readin = false;//指示读取是否完成
    bool finished = false;//指示执行是否完成

    Vector3 destination;//移动到目的地
    Vector3 direction;//移动的方向

    public Vector3 Destination { get { return destination; } }
    public Vector3 Direction { get { return direction; } }

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    public static bool canFly = false;

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
    bool turnSign = false;

    public bool testMove = false;

    void Start()
    {
        received = null;
        dogBehaviour = GetComponent<Animator>();
        canFly = false;
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
            if (canFly)
            {
                break;//狗狗被顶飞停止执行
            }
            if (s.Equals("if"))
            {
                conditionBlockList = new ArrayList();
                conditionBlockList.AddRange(ConditionBlocks());
                //yield return new WaitForSeconds(0.1f);
                foreach (string x in conditionBlockList)
                {
                    Move(x);
                    Debug.Log(x);
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
            testMove = true;
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
        else if (x.Equals("Turn"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            if (!turnSign)
            {
                direction.y -= 180;
                turnSign = true;
            }
            else
            {
                direction.y += 180;
                turnSign = false;
            }
           
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
        //修复Z轴锁定不了的bug
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
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

        Debug.Log("Slime "+isSlime);
        Debug.Log("Gem "+isGem);

        if (option == 0)
        {
            if (isSlime)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else
            {
                codes.AddRange(elseCodeBlockTags);
            }
        }
        else if (option == 1)
        {
            if (isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else
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
