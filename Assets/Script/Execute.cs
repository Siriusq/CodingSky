using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Execute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject executePanel;// Executive Panel
    static ArrayList codeBlockTags = new ArrayList(); // An array that stores the commands to be executed in the panel
    public static ArrayList CodeBlockTags => codeBlockTags;//getter

    public Slider loopCountSlider;// Slider for adjusting the number of loop
    GameObject loopPanel; // Loop Panel
    int loopTime; // Number of loops
    static ArrayList loopBlockTags = new ArrayList(); // Array for storing commands in the main loop panel
    public static ArrayList LoopBlockTags => loopBlockTags;//getter

    public Slider subLoopCountSlider;// Slider for adjusting the number of subloop
    GameObject subLoopPanel; // SubLoop Panel
    int subLoopTime; // Number of subloops
    static ArrayList subLoopBlockTags = new ArrayList(); // Array for storing commands in the subloop panel
    public static ArrayList SubLoopBlockTags => subLoopBlockTags;//getter

    public Dropdown conditionDropdown; //Conditional selection drop-down menu

    public Movement player;
    public HighLightButton highLight;

    public static ArrayList highlightButtons = new ArrayList();// Highlighted sequential arrays
    public GameObject clickBlock;

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

    public void OnPointerUp(PointerEventData eventData) //Pass the code command group to Movement when the button is lifted
    {
        player.GetCode(codeBlockTags);
        HighLightButton.GetCode(highlightButtons);
        clickBlock.SetActive(true);
        //this.GetComponent<Button>().enabled = false;
        StartCoroutine(BlockWait());
        StopCoroutine(BlockWait());
    }

    public ArrayList LoopArray() //Get the code in the loop panel
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
                    else if (loopBlock.tag.Equals("If"))//Add if support for subloops
                    {
                        loopArrayList.Add("if");
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

    public ArrayList SubLoopArray() //Get the code in the subloop panel
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
                    if (subLoopBlock.tag.Equals("If"))//Add if support for subloops
                    {
                        subLoopArrayList.Add("if");
                    }
                    else
                    {
                        subLoopArrayList.Add(subLoopBlock.tag);
                    }
                    
                }
            }
        }
        return subLoopArrayList;
    }

    IEnumerator BlockWait()
    {
        yield return new WaitForSeconds(0.01f);
        this.GetComponent<Button>().enabled = false;
    }
}
