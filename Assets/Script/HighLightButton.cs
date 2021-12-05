using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//请欣赏终极套娃。。。
public class HighLightButton : MonoBehaviour
{
    static ArrayList blocks = new ArrayList();
    public ArrayList Blocks { get { return blocks; } }
    bool readin = false;//指示读取是否完成
    bool finished = false;//指示执行是否完成

    GameObject executePanel;// 执行面板
    int executeChildCount;// 执行面板命令数量
    public Slider loopCountSlider;// 调整循环次数的滑块
    GameObject loopPanel; // 循环面板
    int loopChildCount;//循环面板命令数量
    int loopTimes;//循环次数
    public Slider subLoopCountSlider;// 调整子循环次数的滑块
    GameObject subLoopPanel; // 子循环面板
    int subLoopChildCount;//子循环面板命令数量
    int subLoopTimes;//子循环次数
    public Dropdown conditionDropdown; //条件选择下拉菜单
    int option;//下拉菜单选项
    GameObject ifPanel;//条件if
    GameObject elsePanel;//条件else

    Button tempButton;// 执行面板命令
    Button tempLoopButton;// 循环面板命令
    Button tempSubLoopButton;// 子循环面板命令
    Button tempIfButton;// if面板命令
    Button tempElseButton;// else面板命令

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
            StartCoroutine(StartHighLight());//与狗狗移动同步进行代码块高亮
        }

        if (finished)
        {
            StopCoroutine(StartHighLight());
        }
    }

    IEnumerator StartHighLight()
    {
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // 找到游戏中的执行面版
        executeChildCount = executePanel.transform.childCount; //面板中代码块的个数

        for (int i = 0; i < executeChildCount; i++)
        {            
            tempButton = executePanel.transform.GetChild(i).GetComponent<Button>();
            if (Movement.canFly)
            { //狗狗被顶飞停止执行
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

    IEnumerator SubLoop()//子循环的
    {
        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        subLoopChildCount = subLoopPanel.transform.childCount; //面板中代码块的个数
        subLoopTimes = (int)subLoopCountSlider.value;

        for (int n = 0; n < subLoopTimes; n++)
        {
            if (Movement.canFly)
            { //狗狗被顶飞停止执行
                break;
            }
            for (int k = 0; k < subLoopChildCount; k++)
            {
                if (Movement.canFly)
                { //狗狗被顶飞停止执行
                    break;            
                }
                tempSubLoopButton = subLoopPanel.transform.GetChild(k).GetComponent<Button>();
                tempSubLoopButton.Select();
                yield return new WaitForSeconds(1.5f);
            }
        }        
    }

    IEnumerator Loop()//主循环的
    {
        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        loopChildCount = loopPanel.transform.childCount; //面板中代码块的个数
        loopTimes = (int)loopCountSlider.value;

        for (int m = 0; m < loopTimes; m++)
        {
            for (int j = 0; j < loopChildCount; j++)
            {
                if (Movement.canFly)
                { //狗狗被顶飞停止执行
                    break;
                }
                tempLoopButton = loopPanel.transform.GetChild(j).GetComponent<Button>();
                if (tempLoopButton.transform.tag.Equals("SubLoop"))
                {
                    yield return StartCoroutine(SubLoop());
                    StopCoroutine(SubLoop());
                }//不加循环里的if了，套娃套傻了，在drop里也砍掉了，要命
                else
                {
                    tempLoopButton.Select();
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }        
    }

    IEnumerator Condition()//条件语句的
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
                yield return StartCoroutine(IfButton(tempIfButton.transform.tag));//等待协程完成
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

    IEnumerator IfButton(string s)//执行if面板内的命令
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

    IEnumerator ElseButton(string s)//执行else面板内的命令
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

    public static void GetCode(ArrayList orders)//判断play按钮是否已经按下
    {
        blocks = new ArrayList();
        blocks.AddRange(orders);
        testHighlight = true;
    }
}
