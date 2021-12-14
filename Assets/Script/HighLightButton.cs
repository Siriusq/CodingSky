using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//«Î–¿…Õ÷’º´Ã◊Õﬁ°£°£°£
public class HighLightButton : MonoBehaviour
{
    static ArrayList blocks = new ArrayList();
    public ArrayList Blocks { get { return blocks; } }
    bool readin = false;//Indicates if the read is complete
    bool finished = false;//Indicates if execution is complete

    GameObject executePanel;// Executive Panel
    int executeChildCount;// Number of commands in the execution panel
    public Slider loopCountSlider;// Slider for adjusting the loop count
    GameObject loopPanel; // Loop Panel
    int loopChildCount;//Number of commands in the loop panel
    int loopTimes;//Loop time
    public Slider subLoopCountSlider;// Slider for adjusting the subloop count
    GameObject subLoopPanel; // Subloop Panel
    int subLoopChildCount;//Number of commands in the subloop panel
    int subLoopTimes;//Sub loop time
    public Dropdown conditionDropdown; //Conditional selection drop-down menu
    int option;//Drop down menu options
    GameObject ifPanel;//if
    GameObject elsePanel;//else

    Button tempButton;// Executive Panel Button
    Button tempLoopButton;// Loop Panel Button
    Button tempSubLoopButton;// Subloop Panel Button
    Button tempIfButton;// If Panel Button
    Button tempElseButton;// Else Panel Button

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
            StartCoroutine(StartHighLight());//Code block highlighting in sync with dog movement
        }

        if (finished)
        {
            StopCoroutine(StartHighLight());
        }
    }

    IEnumerator StartHighLight()
    {
        executePanel = GameObject.FindGameObjectWithTag("execute_panel"); // Find the executive panel in the game
        executeChildCount = executePanel.transform.childCount; //Number of code blocks in the panel

        for (int i = 0; i < executeChildCount; i++)
        {            
            tempButton = executePanel.transform.GetChild(i).GetComponent<Button>();
            if (Movement.canFly)
            {
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

    IEnumerator SubLoop()//Sub Loop
    {
        subLoopPanel = GameObject.FindGameObjectWithTag("SubLoopPanel");
        subLoopChildCount = subLoopPanel.transform.childCount;
        subLoopTimes = (int)subLoopCountSlider.value;

        for (int n = 0; n < subLoopTimes; n++)
        {
            if (Movement.canFly)
            {
                break;
            }
            for (int k = 0; k < subLoopChildCount; k++)
            {
                if (Movement.canFly)
                {
                    break;            
                }
                tempSubLoopButton = subLoopPanel.transform.GetChild(k).GetComponent<Button>();
                tempSubLoopButton.Select();
                yield return new WaitForSeconds(1.5f);
            }
        }        
    }

    IEnumerator Loop()//Loop
    {
        loopPanel = GameObject.FindGameObjectWithTag("LoopPanel");
        loopChildCount = loopPanel.transform.childCount;
        loopTimes = (int)loopCountSlider.value;

        for (int m = 0; m < loopTimes; m++)
        {
            for (int j = 0; j < loopChildCount; j++)
            {
                if (Movement.canFly)
                {
                    break;
                }
                tempLoopButton = loopPanel.transform.GetChild(j).GetComponent<Button>();
                if (tempLoopButton.transform.tag.Equals("SubLoop"))
                {
                    yield return StartCoroutine(SubLoop());
                    StopCoroutine(SubLoop());
                }
                else
                {
                    tempLoopButton.Select();
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }        
    }

    IEnumerator Condition()//if else
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

    IEnumerator IfButton(string s)//Execute the commands in the if panel
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

    IEnumerator ElseButton(string s)//Execute the commands in the else panel
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

    public static void GetCode(ArrayList orders)//Determine if the Execute button has been pressed
    {
        blocks = new ArrayList();
        blocks.AddRange(orders);
        testHighlight = true;
    }
}
