using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    private GameObject conditionIf; //if的面板
    private GameObject conditionElse; //else的面板
    private Dropdown conditionDropdown; //条件选择下拉菜单

    public ArrayList codes = new ArrayList();//if执行得到的数组
    public ArrayList test = new ArrayList();

    private ArrayList ifCodeBlockTags = new ArrayList();
    private ArrayList elseCodeBlockTags = new ArrayList();

    string ifBlock;
    string elseBlock;

    bool isGem;
    bool isSlime;

    public Execute execute;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ArrayList generateCodeBlocks()
    {
        codes = null;
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        conditionDropdown = GameObject.FindGameObjectWithTag("ConditionDropdown").GetComponent<Dropdown>();

        isGem = GameObject.FindGameObjectWithTag("Gem").GetComponent<CollectGem>().isGem;
        isSlime = GameObject.FindGameObjectWithTag("Slime").GetComponent<Attack>().isSlime;

        int option = conditionDropdown.value;//下拉菜单的选项，0是史莱姆，1是宝石

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //判断框里是不是循环 都直接用数组存算了，好回传
        ifCodeBlockTags.AddRange(isLoop(ifBlock));
        elseCodeBlockTags.AddRange(isLoop(elseBlock));

        if (option == 0)
        {
            if (isSlime && !isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else if(!isSlime && isGem)
            {
                codes.AddRange(elseCodeBlockTags);
            }           
        }
        else
        {
            if (!isSlime && isGem)
            {
                codes.AddRange(elseCodeBlockTags);
            }
            else if (isSlime && !isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
        }

        isSlime = false;
        isGem = false;

        return codes;
    }

    public ArrayList isLoop(string s)
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))
        {
            temp.AddRange(execute.LoopArray());
        }
        else if (s.Equals("SubLoop"))
        {
            temp.AddRange(execute.SubLoopArray());
        }
        else
        {
            temp.Add(s);
        }

        return temp;
    }

    public ArrayList getCodeBlocks()
    {
        generateCodeBlocks();
        return codes;
    }
}
