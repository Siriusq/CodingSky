using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Execute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject executePanel;// 执行面板
    static ArrayList codeBlockTags = new ArrayList(); // 存储执行面版内命令的数组
    public static ArrayList CodeBlockTags => codeBlockTags;//getter

    public Slider loopCountSlider;// 调整循环次数的滑块
    GameObject loopPanel; // 循环面板
    int loopTime; // 循环次数
    static ArrayList loopBlockTags = new ArrayList(); // 存储主循环面版内命令的数组
    public static ArrayList LoopBlockTags => loopBlockTags;//getter

    public Slider subLoopCountSlider;// 调整子循环次数的滑块
    GameObject subLoopPanel; // 子循环面板
    int subLoopTime; // 子循环次数
    static ArrayList subLoopBlockTags = new ArrayList(); // 存储子循环面版内命令的数组
    public static ArrayList SubLoopBlockTags => subLoopBlockTags;//getter

    public Dropdown conditionDropdown; //条件选择下拉菜单

    public Movement player;
    public HighLightButton highLight;

    public static ArrayList highlightButtons = new ArrayList();// 高亮顺序数组

    public void OnPointerDown(PointerEventData eventData)
    {
        codeBlockTags = new ArrayList(); //清空数组缓存，因为游戏中可能会多次点击运行按钮
        loopBlockTags = new ArrayList();
        subLoopBlockTags = new ArrayList();
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // 找到游戏中的执行面版
        int childCount = executePanel.transform.childCount; //面板中代码块的个数

        subLoopBlockTags.AddRange(SubLoopArray());
        loopBlockTags.AddRange(LoopArray());

        if (childCount != 0)
        {
            // 先把对应的块加到数组里
            foreach (Transform block in executePanel.transform)//遍历代码块
            {
                highlightButtons.Add(block.tag);

                if (block.tag.Equals("Loop"))// 如果出现循环代码块，就跳到循环面板里，将循环面板里的代码块按循环次数添加到数组里
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
                else//其他情况直接添加
                {
                    codeBlockTags.Add(block.tag);
                }                
            }            
        }        
    }

    public void OnPointerUp(PointerEventData eventData) //鼠标抬起的时候把代码命令组传到Movement里
    {
        player.GetCode(codeBlockTags);
        HighLightButton.GetCode(highlightButtons);
    }

    public ArrayList LoopArray() //获取循环面板里的代码
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

    public ArrayList SubLoopArray() //获取子循环面板里的代码
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
