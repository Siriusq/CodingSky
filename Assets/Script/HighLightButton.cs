using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�������ռ����ޡ�����
public class HighLightButton : MonoBehaviour
{
    static ArrayList blocks = new ArrayList();
    public ArrayList Blocks { get { return blocks; } }
    bool readin = false;//ָʾ��ȡ�Ƿ����
    bool finished = false;//ָʾִ���Ƿ����

    GameObject executePanel;// ִ�����
    int executeChildCount;// ִ�������������
    public Slider loopCountSlider;// ����ѭ�������Ļ���
    GameObject loopPanel; // ѭ�����
    int loopChildCount;//ѭ�������������
    int loopTimes;//ѭ������
    public Slider subLoopCountSlider;// ������ѭ�������Ļ���
    GameObject subLoopPanel; // ��ѭ�����
    int subLoopChildCount;//��ѭ�������������
    int subLoopTimes;//��ѭ������
    public Dropdown conditionDropdown; //����ѡ�������˵�
    int option;//�����˵�ѡ��
    GameObject ifPanel;//����if
    GameObject elsePanel;//����else

    Button tempButton;// ִ���������
    Button tempLoopButton;// ѭ���������
    Button tempSubLoopButton;// ��ѭ���������
    Button tempIfButton;// if�������
    Button tempElseButton;// else�������

    public static bool testHighlight = false;


    // Start is called before the first frame update
    void Start()
    {
        blocks = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (blocks != null && !readin)
        {            
            readin = true;           
            StartCoroutine(StartHighLight());//�빷���ƶ�ͬ�����д�������
        }

        if (finished)
        {
            StopCoroutine(StartHighLight());
        }
    }

    IEnumerator StartHighLight()
    {
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // �ҵ���Ϸ�е�ִ�����
        executeChildCount = executePanel.transform.childCount; //����д����ĸ���

        for (int i = 0; i < executeChildCount; i++)
        {            
            tempButton = executePanel.transform.GetChild(i).GetComponent<Button>();
            if (Movement.canFly)
            { //����������ִֹͣ��
                break; 
            } 
            if (tempButton.transform.tag.Equals("Loop"))
            {
                tempButton.Select();
                yield return StartCoroutine(Loop());
                StopCoroutine(Loop());
            }
            else if (tempButton.transform.tag.Equals("SubLoop"))
            {
                tempButton.Select();
                yield return StartCoroutine(SubLoop());
                StopCoroutine(SubLoop());
            }
            else if (tempButton.transform.tag.Equals("If"))
            {
                tempButton.Select();
                yield return StartCoroutine(Condition());
                StopCoroutine(Condition());
            }
            else
            {
                tempButton.Select();
                yield return new WaitForSeconds(1.5f);
            }          
            //Debug.Log(tempButton.transform.tag);
        }

        finished = true;
        yield return new WaitForSeconds(1);
    }

    IEnumerator SubLoop()//��ѭ����
    {
        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        subLoopChildCount = subLoopPanel.transform.childCount; //����д����ĸ���
        subLoopTimes = (int)subLoopCountSlider.value;

        for (int n = 0; n < subLoopTimes; n++)
        {
            if (Movement.canFly)
            { //����������ִֹͣ��
                break;
            }
            for (int k = 0; k < subLoopChildCount; k++)
            {
                if (Movement.canFly)
                { //����������ִֹͣ��
                    break;            
                }
                tempSubLoopButton = subLoopPanel.transform.GetChild(k).GetComponent<Button>();
                tempSubLoopButton.Select();
                yield return new WaitForSeconds(1.5f);
            }
        }        
    }

    IEnumerator Loop()//��ѭ����
    {
        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        loopChildCount = loopPanel.transform.childCount; //����д����ĸ���
        loopTimes = (int)loopCountSlider.value;

        for (int m = 0; m < loopTimes; m++)
        {
            for (int j = 0; j < loopChildCount; j++)
            {
                if (Movement.canFly)
                { //����������ִֹͣ��
                    break;
                }
                tempLoopButton = loopPanel.transform.GetChild(j).GetComponent<Button>();
                if (tempLoopButton.transform.tag.Equals("SubLoop"))
                {
                    yield return StartCoroutine(SubLoop());
                    StopCoroutine(SubLoop());
                }//����ѭ�����if�ˣ�������ɵ�ˣ���drop��Ҳ�����ˣ�Ҫ��
                else
                {
                    tempLoopButton.Select();
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }        
    }

    IEnumerator Condition()//��������
    {
        option = conditionDropdown.value;
        ifPanel = GameObject.FindGameObjectWithTag("SubCondition");
        elsePanel = GameObject.FindGameObjectWithTag("SubConditionElse");
        tempIfButton = ifPanel.transform.GetChild(0).GetComponent<Button>();
        tempElseButton = elsePanel.transform.GetChild(0).GetComponent<Button>();
        bool isGem = CollectGem.isGem;
        bool isSlime = Attack.isSlime;

        if (option == 0)
        {
            if (isSlime)
            {
                tempIfButton.Select();
                yield return StartCoroutine(IfButton(tempIfButton.transform.tag));//�ȴ�Э�����
                StopCoroutine(IfButton(tempIfButton.transform.tag));
            }
            else
            {
                tempElseButton.Select();
                yield return StartCoroutine(ElseButton(tempElseButton.transform.tag));
                StopCoroutine(ElseButton(tempIfButton.transform.tag));
            }
        }
        else
        {
            if (isGem)
            {
                tempIfButton.Select();
                yield return StartCoroutine(IfButton(tempIfButton.transform.tag));
                StopCoroutine(IfButton(tempIfButton.transform.tag));
            }
            else
            {
                tempElseButton.Select();
                yield return StartCoroutine(ElseButton(tempElseButton.transform.tag));
                StopCoroutine(ElseButton(tempIfButton.transform.tag));
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator IfButton(string s)//ִ��if����ڵ�����
    {
        if (s.Equals("SubLoop"))
        {
            yield return StartCoroutine(SubLoop());
            StopCoroutine(SubLoop());
        }
        else if (s.Equals("Loop"))
        {
            yield return StartCoroutine(Loop());
            StopCoroutine(Loop());
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator ElseButton(string s)//ִ��else����ڵ�����
    {
        if (s.Equals("SubLoop"))
        {
            yield return StartCoroutine(SubLoop());
            StopCoroutine(SubLoop());
        }
        else if (s.Equals("Loop"))
        {
            yield return StartCoroutine(Loop());
            StopCoroutine(Loop());
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }
    }

    public static void GetCode(ArrayList orders)//�ж�play��ť�Ƿ��Ѿ�����
    {
        blocks = new ArrayList();
        blocks.AddRange(orders);
        testHighlight = true;
    }
}
