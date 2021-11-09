using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightButton : MonoBehaviour
{
    static ArrayList blocks = new ArrayList();
    bool readin = false;//ָʾ��ȡ�Ƿ����
    bool finished = false;//ָʾִ���Ƿ����

    GameObject executePanel;// ִ�����
    int executeChildCount;
    public Slider loopCountSlider;// ����ѭ�������Ļ���
    GameObject loopPanel; // ѭ�����
    int loopChildCount;
    int loopTimes;
    public Slider subLoopCountSlider;// ������ѭ�������Ļ���
    GameObject subLoopPanel; // ��ѭ�����
    int subLoopChildCount;
    int subLoopTimes;
    public Dropdown conditionDropdown; //����ѡ�������˵�
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
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // �ҵ���Ϸ�е�ִ�����
        executeChildCount = executePanel.transform.childCount; //����д����ĸ���

        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        loopChildCount = loopPanel.transform.childCount; //����д����ĸ���
        loopTimes = (int)loopCountSlider.value;

        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        subLoopChildCount = subLoopPanel.transform.childCount; //����д����ĸ���
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
