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
    private int loopTime; //循环次数
    

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
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // 找到游戏中的执行面版
        int childCount = executePanel.transform.childCount; //面板中代码块的个数
        if(childCount != 0)
        {
            // Todo: 可以给运行到的代码块的高亮，不过应该加在人物移动那里
            foreach (Transform block in executePanel.transform)//遍历代码块
            {
                if (block.tag.Equals("Loop"))// 如果出现循环代码块，就跳到循环面板里，将循环面板里的代码块按循环次数添加到数组里
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
        //Test
        foreach (string s in codeBlockTags)
        {
            Debug.Log(s);
        }
    }
}
