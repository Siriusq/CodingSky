using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Execute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject executePanel;// ִ�����
    public ArrayList codeBlockTags = new ArrayList(); // �洢ִ����������������

    public Slider loopCountSlider;// ����ѭ�������Ļ���
    private GameObject loopPanel; // ѭ�����
    private int loopTime; // ѭ������
    private ArrayList loopBlockTags = new ArrayList(); // �洢��ѭ����������������

    public Slider subLoopCountSlider;// ������ѭ�������Ļ���
    private GameObject subLoopPanel; // ��ѭ�����
    private int subLoopTime; // ��ѭ������
    private ArrayList subLoopBlockTags = new ArrayList(); // �洢��ѭ����������������

    private GameObject conditionIf; //if�����
    private GameObject conditionElse; //else�����
    public Dropdown conditionDropdown; //����ѡ�������˵�

    public Movement player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        codeBlockTags = new ArrayList(); //������黺�棬��Ϊ��Ϸ�п��ܻ��ε�����а�ť
        loopBlockTags = new ArrayList();
        subLoopBlockTags = new ArrayList();
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // �ҵ���Ϸ�е�ִ�����
        int childCount = executePanel.transform.childCount; //����д����ĸ���

        //loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        //subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");

        //conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        //conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");

        // ��Ӵ�ѭ���еĴ���鵽����
/*        if (subLoopPanel.transform.childCount != 0)
        {
            subLoopTime = (int)subLoopCountSlider.value;
            
            for (int i = 0; i < subLoopTime; i++)
            {
                foreach (Transform subLoopBlock in subLoopPanel.transform)
                {
                    subLoopBlockTags.Add(subLoopBlock.tag);
                }
            }
        }*/

        subLoopBlockTags.AddRange(SubLoopArray());

        // �����ѭ���еĴ���鵽����,��ѭ�����԰�����ѭ��
/*        if (loopPanel.transform.childCount != 0)
        {
            loopTime = (int)loopCountSlider.value;
            
            for (int i = 0; i < loopTime; i++)
            {
                foreach (Transform loopBlock in loopPanel.transform)
                {
                    if (loopBlock.tag.Equals("SubLoop"))
                    {
                        loopBlockTags.AddRange(subLoopBlockTags);
                    }
                    else
                    {
                        loopBlockTags.Add(loopBlock.tag);
                    }                    
                }
            }
        }*/

        loopBlockTags.AddRange(LoopArray());

        // �ж��������
/*        if(conditionIf.transform.childCount != 0 && conditionElse.transform.childCount != 0)
        {
            int selectValue = conditionDropdown.value;// �������ѡ��Ĵ��ţ�0����1
            //Todo: �ж�����ǰ����0����1��������صĺ�setValue��ͬ�����if���tag����ͬ�����else���tag
        }*/

        if (childCount != 0)
        {
            // Todo: ���Ը����е��Ĵ����ĸ���������Ӧ�ü��������ƶ�����

            foreach (Transform block in executePanel.transform)//���������
            {
                if (block.tag.Equals("Loop"))// �������ѭ������飬������ѭ��������ѭ�������Ĵ���鰴ѭ��������ӵ�������
                {
                    codeBlockTags.AddRange(loopBlockTags);
                }
                else if (block.tag.Equals("SubLoop"))
                {
                    codeBlockTags.AddRange(subLoopBlockTags);
                }
                else if (block.tag.Equals("If"))// ���������������飬���������
                {
                    codeBlockTags.Add("if");
                }
                else//�������ֱ�����
                {
                    codeBlockTags.Add(block.tag);
                }                            
            }            
        }        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.GetCode(codeBlockTags);              
    }

    public ArrayList LoopArray()
    {
        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        ArrayList loopArrayList = new ArrayList();

        if (loopPanel.transform.childCount != 0)
        {
            loopTime = (int)loopCountSlider.value;

            for (int i = 0; i < loopTime; i++)
            {
                foreach (Transform loopBlock in loopPanel.transform)
                {
                    if (loopBlock.tag.Equals("SubLoop"))
                    {
                        loopArrayList.AddRange(subLoopBlockTags);
                    }
                    else
                    {
                        loopArrayList.Add(loopBlock.tag);
                    }
                }
            }
        }

        return loopArrayList;
    }

    public ArrayList SubLoopArray()
    {
        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        ArrayList subLoopArrayList = new ArrayList();        

        if (subLoopPanel.transform.childCount != 0)
        {
            subLoopTime = (int)subLoopCountSlider.value;

            for (int i = 0; i < subLoopTime; i++)
            {
                foreach (Transform subLoopBlock in subLoopPanel.transform)
                {
                    subLoopArrayList.Add(subLoopBlock.tag);
                }
            }
        }

        return subLoopArrayList;
    }

}
