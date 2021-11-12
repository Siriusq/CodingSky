using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Execute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject executePanel;// ִ�����
    static ArrayList codeBlockTags = new ArrayList(); // �洢ִ����������������
    public static ArrayList CodeBlockTags => codeBlockTags;//getter

    public Slider loopCountSlider;// ����ѭ�������Ļ���
    GameObject loopPanel; // ѭ�����
    int loopTime; // ѭ������
    static ArrayList loopBlockTags = new ArrayList(); // �洢��ѭ����������������
    public static ArrayList LoopBlockTags => loopBlockTags;//getter

    public Slider subLoopCountSlider;// ������ѭ�������Ļ���
    GameObject subLoopPanel; // ��ѭ�����
    int subLoopTime; // ��ѭ������
    static ArrayList subLoopBlockTags = new ArrayList(); // �洢��ѭ����������������
    public static ArrayList SubLoopBlockTags => subLoopBlockTags;//getter

    public Dropdown conditionDropdown; //����ѡ�������˵�

    public Movement player;
    public HighLightButton highLight;

    public static ArrayList highlightButtons = new ArrayList();// ����˳������

    public void OnPointerDown(PointerEventData eventData)
    {
        codeBlockTags = new ArrayList(); //������黺�棬��Ϊ��Ϸ�п��ܻ��ε�����а�ť
        loopBlockTags = new ArrayList();
        subLoopBlockTags = new ArrayList();
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // �ҵ���Ϸ�е�ִ�����
        int childCount = executePanel.transform.childCount; //����д����ĸ���

        subLoopBlockTags.AddRange(SubLoopArray());
        loopBlockTags.AddRange(LoopArray());

        if (childCount != 0)
        {
            // �ȰѶ�Ӧ�Ŀ�ӵ�������
            foreach (Transform block in executePanel.transform)//���������
            {
                highlightButtons.Add(block.tag);

                if (block.tag.Equals("Loop"))// �������ѭ������飬������ѭ��������ѭ�������Ĵ���鰴ѭ��������ӵ�������
                {
                    codeBlockTags.AddRange(loopBlockTags);                   
                }
                else if (block.tag.Equals("SubLoop"))
                {
                    codeBlockTags.AddRange(subLoopBlockTags);
                }
                else if (block.tag.Equals("If"))
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

    public void OnPointerUp(PointerEventData eventData) //���̧���ʱ��Ѵ��������鴫��Movement��
    {
        player.GetCode(codeBlockTags);
        HighLightButton.GetCode(highlightButtons);
    }

    public ArrayList LoopArray() //��ȡѭ�������Ĵ���
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

    public ArrayList SubLoopArray() //��ȡ��ѭ�������Ĵ���
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
