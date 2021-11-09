using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour
{
    static ArrayList blocks = new ArrayList();
    bool readin = false;//指示读取是否完成
    bool finished = false;//指示执行是否完成

    GameObject executePanel;// 执行面板
    int executeChildCount;
    public Slider loopCountSlider;// 调整循环次数的滑块
    GameObject loopPanel; // 循环面板
    int loopChildCount;
    int loopTimes;
    public Slider subLoopCountSlider;// 调整子循环次数的滑块
    GameObject subLoopPanel; // 子循环面板
    int subLoopChildCount;
    int subLoopTimes;
    public Dropdown conditionDropdown; //条件选择下拉菜单
    int option;


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

            StartCoroutine(StartHighLight());
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

        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        loopChildCount = loopPanel.transform.childCount; //面板中代码块的个数
        loopTimes = (int)loopCountSlider.value;

        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        subLoopChildCount = subLoopPanel.transform.childCount; //面板中代码块的个数
        subLoopTimes = (int)subLoopCountSlider.value;

        option = conditionDropdown.value;

        for (int i = 0; i < executeChildCount; i++)
        {
            Debug.Log("!!");
            //GameObject temp = executePanel.transform.GetChild(i).gameObject;
            Button tempButton = executePanel.transform.GetChild(i).GetComponent<Button>();
            tempButton.Select();
            
            yield return new WaitForSeconds(1.5f);
        }

        foreach (string s in blocks)
        {
            //temp.Select();
            if (s.Equals("If"))
            {

            }
            else if (s.Equals("Loop"))
            {

            }
            else if (s.Equals("SubLoop"))
            {

            }
        }

        finished = true;
        yield return new WaitForSeconds(1);
    }



    public static void GetCode(ArrayList orders)
    {
        blocks = new ArrayList();
        blocks.AddRange(orders);
    }
}
