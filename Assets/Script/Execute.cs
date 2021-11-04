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
    public ArrayList loopBlockTags = new ArrayList(); // �洢��ѭ����������������

    public Slider subLoopCountSlider;// ������ѭ�������Ļ���
    private GameObject subLoopPanel; // ��ѭ�����
    private int subLoopTime; // ��ѭ������
    public ArrayList subLoopBlockTags = new ArrayList(); // �洢��ѭ����������������


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

        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");

        // ��Ӵ�ѭ���еĴ���鵽����
        if (subLoopPanel.transform.childCount != 0)
        {
            subLoopTime = (int)subLoopCountSlider.value;
            
            for (int i = 0; i < subLoopTime; i++)
            {
                foreach (Transform subLoopBlock in subLoopPanel.transform)
                {
                    subLoopBlockTags.Add(subLoopBlock.tag);
                }
            }
        }

        // �����ѭ���еĴ���鵽����,��ѭ�����԰�����ѭ��
        if (loopPanel.transform.childCount != 0)
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
        }

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
        //Test
        foreach (string s in codeBlockTags)
        {
            Debug.Log(s);
        }
    }
}
