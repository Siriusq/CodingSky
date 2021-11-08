using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    //Todo: ��ʵѭ��ҲӦ�õ����������
    private GameObject conditionIf; //if�����
    private GameObject conditionElse; //else�����
    public Dropdown conditionDropdown; //����ѡ�������˵�
    public int option;

    public ArrayList codes = new ArrayList();//ifִ�еõ�������
    public ArrayList test = new ArrayList();

    private ArrayList ifCodeBlockTags = new ArrayList();
    private ArrayList elseCodeBlockTags = new ArrayList();

    string ifBlock;
    string elseBlock;

    bool isGem;
    bool isSlime;

    public Execute execute;
    public Movement movement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateCodeBlocks()
    {
        codes = null;
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        //conditionDropdown = GameObject.Find("ConditionDropdown").GetComponent<Dropdown>();

        isGem = GameObject.FindGameObjectWithTag("Gem").GetComponent<CollectGem>().isGem;
        isSlime = GameObject.FindGameObjectWithTag("Slime").GetComponent<Attack>().isSlime;

        //option = conditionDropdown.value;//�����˵���ѡ�0��ʷ��ķ��1�Ǳ�ʯ
        option = 0;

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //�жϿ����ǲ���ѭ�� ��ֱ������������ˣ��ûش�
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

        foreach(string s in codes)
        {
            Debug.Log(s);
        }

        //Todo: 
        movement.GetCondition(codes);
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
