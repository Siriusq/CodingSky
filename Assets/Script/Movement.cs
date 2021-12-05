using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    ArrayList received = new ArrayList();//���¿�ʼ��ť����յ���ָ����
    public ArrayList Received { get { return received; } }
    bool readin = false;//ָʾ��ȡ�Ƿ����
    bool finished = false;//ָʾִ���Ƿ����

    Vector3 destination;//�ƶ���Ŀ�ĵ�
    Vector3 direction;//�ƶ��ķ���

    public Vector3 Destination { get { return destination; } }
    public Vector3 Direction { get { return direction; } }

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    public static bool canFly = false;

    //�����Ǹ�if else��
    GameObject conditionIf; //if�����
    GameObject conditionElse; //else�����
    Dropdown conditionDropdown; //����ѡ�������˵�

    ArrayList codes = new ArrayList();//ifִ�еõ�������
    ArrayList conditionBlockList = new ArrayList();//��ʵ������һ���Ķ��������ǲ���ô���б���

    ArrayList ifCodeBlockTags = new ArrayList();//if�еĴ���
    ArrayList elseCodeBlockTags = new ArrayList();//else�еĴ���
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
                break;//����������ִֹͣ��
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

    private void Move(string x)//������������
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
        //�޸�Z���������˵�bug
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    public void GetCode(ArrayList codes)
    {
        received = new ArrayList();
        received.AddRange(codes);
    }

    private ArrayList ConditionBlocks()//�ж�ִ��if����else�Ĵ���
    {
        codes = new ArrayList();
        ifCodeBlockTags = new ArrayList();
        elseCodeBlockTags = new ArrayList();
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        conditionDropdown = GameObject.FindGameObjectWithTag("ConditionDropdown").GetComponent<Dropdown>();

        isGem = CollectGem.isGem;
        isSlime = Attack.isSlime;

        int option = conditionDropdown.value;//�����˵���ѡ�0��ʷ��ķ��1�Ǳ�ʯ

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //�жϿ����ǲ���ѭ�� ��ֱ����������ˣ��ûش�
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

    private ArrayList isLoop(string s)// �ж�if������û��ѭ��������ѭ��
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))//��ѭ���Ͱ�ѭ���ӵ�����
        {
            temp.AddRange(Execute.LoopBlockTags);
        }
        else if (s.Equals("SubLoop"))//����ѭ���Ͱ���ѭ���ӵ�����
        {
            temp.AddRange(Execute.SubLoopBlockTags);
        }
        else
        {
            temp.Add(s);//û�о�ֻ��ԭ��������
        }

        return temp;
    }
}
