using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    public ArrayList received = new ArrayList();//���¿�ʼ��ť����յ���ָ����
    bool readin = false;//ָʾ��ȡ�Ƿ����
    bool finished = false;//ָʾִ���Ƿ����

    Vector3 destination;//�ƶ���Ŀ�ĵ�
    Vector3 direction;//�ƶ��ķ���

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    public Condition condition;
    private ArrayList conditionTemp = new ArrayList();

    //Todo: ������condition�޹����ģ������á�����
    private GameObject conditionIf; //if�����
    private GameObject conditionElse; //else�����
    public Dropdown conditionDropdown; //����ѡ�������˵�
    //public int option;

    public ArrayList codes = new ArrayList();//ifִ�еõ�������
    public ArrayList test = new ArrayList();

    private ArrayList ifCodeBlockTags = new ArrayList();
    private ArrayList elseCodeBlockTags = new ArrayList();

    string ifBlock;
    string elseBlock;

    bool isGem;
    bool isSlime;

    public Execute execute;
    //Todo: ��..............................................................

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
                test = new ArrayList();
                //condition.generateCodeBlocks();
                //GameObject.FindGameObjectWithTag("If").SendMessage("generateCodeBlocks");
                test.AddRange(ConditionBlocks());
                Debug.Log("Before");
                //condition.getCodeBlocks();
                yield return new WaitForSeconds(0.1f);
                //conditionTemp.Add("Attack");
                //conditionTemp.AddRange(condition.getCodeBlocks());
                
                foreach (string x in test)
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

    public ArrayList ConditionBlocks()
    {
        codes = new ArrayList();
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        //conditionDropdown = GameObject.Find("ConditionDropdown").GetComponent<Dropdown>();
        //Todo: ����������������ó�private��ok�ˣ����ո�

        isGem = GameObject.FindGameObjectWithTag("Gem").GetComponent<CollectGem>().isGem;
        isSlime = GameObject.FindGameObjectWithTag("Slime").GetComponent<Attack>().isSlime;

        int option = conditionDropdown.value;//�����˵���ѡ�0��ʷ��ķ��1�Ǳ�ʯ
        //option = 0;

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //�жϿ����ǲ���ѭ�� ��ֱ������������ˣ��ûش�
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

    public ArrayList isLoop(string s)
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))
        {
            temp.AddRange(execute.LoopArray());
        }
        else if (s.Equals("SubLoop"))
        {
            temp.AddRange(execute.SubLoopArray());
        }
        else
        {
            temp.Add(s);
        }

        return temp;
    }
}
