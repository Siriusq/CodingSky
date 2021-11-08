using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Execute : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject executePanel;// 执行面板
    public ArrayList codeBlockTags = new ArrayList(); // 存储执行面版内命令的数组

    public Slider loopCountSlider;// 调整循环次数的滑块
    private GameObject loopPanel; // 循环面板
    private int loopTime; // 循环次数
    private ArrayList loopBlockTags = new ArrayList(); // 存储主循环面版内命令的数组

    public Slider subLoopCountSlider;// 调整子循环次数的滑块
    private GameObject subLoopPanel; // 子循环面板
    private int subLoopTime; // 子循环次数
    private ArrayList subLoopBlockTags = new ArrayList(); // 存储子循环面版内命令的数组

    private GameObject conditionIf; //if的面板
    private GameObject conditionElse; //else的面板
    public Dropdown conditionDropdown; //条件选择下拉菜单

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
        codeBlockTags = new ArrayList(); //清空数组缓存，因为游戏中可能会多次点击运行按钮
        loopBlockTags = new ArrayList();
        subLoopBlockTags = new ArrayList();
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // 找到游戏中的执行面版
        int childCount = executePanel.transform.childCount; //面板中代码块的个数

        //loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        //subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");

        //conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        //conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");

        // 添加次循环中的代码块到数组
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

        // 添加主循环中的代码块到数组,主循环可以包含子循环
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

        // 判断条件语句
/*        if(conditionIf.transform.childCount != 0 && conditionElse.transform.childCount != 0)
        {
            int selectValue = conditionDropdown.value;// 下拉面板选择的代号，0或者1
            //Todo: 判断人物前面是0还是1，如果传回的和setValue相同，添加if里的tag，不同就添加else里的tag
        }*/

        if (childCount != 0)
        {
            // Todo: 可以给运行到的代码块的高亮，不过应该加在人物移动那里

            foreach (Transform block in executePanel.transform)//遍历代码块
            {
                if (block.tag.Equals("Loop"))// 如果出现循环代码块，就跳到循环面板里，将循环面板里的代码块按循环次数添加到数组里
                {
                    codeBlockTags.AddRange(loopBlockTags);
                }
                else if (block.tag.Equals("SubLoop"))
                {
                    codeBlockTags.AddRange(subLoopBlockTags);
                }
                else if (block.tag.Equals("If"))// 如果出现条件代码块，这个得想想
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
