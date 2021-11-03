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
    private int loopTime; //ѭ������
    

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
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // �ҵ���Ϸ�е�ִ�����
        int childCount = executePanel.transform.childCount; //����д����ĸ���
        if(childCount != 0)
        {
            // Todo: ���Ը����е��Ĵ����ĸ���������Ӧ�ü��������ƶ�����
            foreach (Transform block in executePanel.transform)//���������
            {
                if (block.tag.Equals("Loop"))// �������ѭ������飬������ѭ��������ѭ�������Ĵ���鰴ѭ��������ӵ�������
                {
                    loopTime = (int)loopCountSlider.value;
                    loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
                    int loopChildCount = loopPanel.transform.childCount;
                    for(int i = 0; i< loopTime; i++)
                    {
                        foreach (Transform loopBlock in loopPanel.transform)
                        {
                            codeBlockTags.Add(loopBlock.tag);
                        }
                    }
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
