using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    Animator dogBehaviour;

    ArrayList received = new ArrayList();//Command list received after pressing the Execute button
    public ArrayList Received { get { return received; } }
    bool readin = false;//Indicates if the read is complete
    bool finished = false;//Indicates if execution is complete

    Vector3 destination;//Destinations to move to
    Vector3 direction;//Direction of movement

    public Vector3 Destination { get { return destination; } }
    public Vector3 Direction { get { return direction; } }

    public bool canCollect = false;
    public bool canOPen = false;
    public bool canAttack = false;

    public static bool canFly = false;

    //if else panel
    GameObject conditionIf; //if panel
    GameObject conditionElse; //else panel
    Dropdown conditionDropdown; //condition selet

    ArrayList codes = new ArrayList();//if arraylist
    ArrayList conditionBlockList = new ArrayList();//same to codes

    ArrayList ifCodeBlockTags = new ArrayList();//code block in if panel
    ArrayList elseCodeBlockTags = new ArrayList();//code block in else panel
    string ifBlock;
    string elseBlock;
    bool isGem;
    bool isSlime;
    bool turnSign = false;

    public bool testMove = false;

    void Start()
    {
        received = null;
        dogBehaviour = GetComponent<Animator>();
        canFly = false;
    }

    void Update()
    {
        if (received != null && !readin)
        {
            StartCoroutine(ExecuteBlocks());
            readin = true;
        }

        if (finished)
        {
            StopCoroutine(ExecuteBlocks());
        }
    }

    IEnumerator ExecuteBlocks()
    {
        Debug.Log("Start");
        foreach (string s in received)
        {
            if (canFly)
            {
                break;//After the dog is toppled, the execution of the code block is stopped
            }
            if (s.Equals("if"))
            {
                conditionBlockList = new ArrayList();
                conditionBlockList.AddRange(ConditionBlocks());
                //yield return new WaitForSeconds(0.1f);
                foreach (string x in conditionBlockList)
                {
                    Move(x);
                    Debug.Log(x);
                    yield return new WaitForSeconds(1.5f);
                    boolControl();
                }
            }
            else
            {
                Move(s);
                Debug.Log(s);
                yield return new WaitForSeconds(1.5f);
                boolControl();
            }          
            
        }
        finished = true;
        yield return new WaitForSeconds(1);        
    }

    private void boolControl()
    {
        dogBehaviour.SetBool("isMove", false);
        canCollect = false;
        canOPen = false;
        canAttack = false;
        dogBehaviour.SetBool("isCollecting", false);
        dogBehaviour.SetBool("isOpen", false);
        dogBehaviour.SetBool("isAttack", false);
    }

    private void Move(string x)//Dog movement code
    {                
        if (x.Equals("MoveForward"))
        {
            testMove = true;
            dogBehaviour.SetBool("isMove", true);
            destination = this.transform.position + transform.forward;
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOMove(destination, 1);
        }
        else if (x.Equals("TurnLeft"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y -= 90;
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);            
        }
        else if (x.Equals("TurnRight"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            direction.y += 90;
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);            
        }
        else if (x.Equals("Turn"))
        {
            dogBehaviour.SetBool("isMove", true);
            direction = transform.forward;
            if (!turnSign)
            {
                direction.y -= 180;
                turnSign = true;
            }
            else
            {
                direction.y += 180;
                turnSign = false;
            }
           
            AudioManager.actionListener = 4;
            Tweener tweener = transform.DOLocalRotate(direction, 1, RotateMode.LocalAxisAdd);
        }
        else if (x.Equals("Collect"))
        {
            dogBehaviour.SetBool("isCollecting", true);
            canCollect = true;
        }
        else if (x.Equals("Attack"))
        {
            dogBehaviour.SetBool("isAttack", true);
            canAttack = true;
            AudioManager.actionListener = 2;
        }
        else if (x.Equals("Treasure"))
        {
            dogBehaviour.SetBool("isOpen", true);
            canOPen = true;
            AudioManager.actionListener = 6;
        }
        //Fix the bug that Z-axis can't be locked
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    public void GetCode(ArrayList codes)
    {
        received = new ArrayList();
        received.AddRange(codes);
    }

    private ArrayList ConditionBlocks()//Code to determine whether to execute an if or an else
    {
        codes = new ArrayList();
        ifCodeBlockTags = new ArrayList();
        elseCodeBlockTags = new ArrayList();
        conditionIf = GameObject.FindGameObjectWithTag("SubCondition");
        conditionElse = GameObject.FindGameObjectWithTag("SubConditionElse");
        conditionDropdown = GameObject.FindGameObjectWithTag("ConditionDropdown").GetComponent<Dropdown>();

        isGem = CollectGem.isGem;
        isSlime = Attack.isSlime;

        int option = conditionDropdown.value;//Drop down menu options, 0 is Slime, 1 is Crystal

        ifBlock = conditionIf.transform.GetChild(0).tag;
        elseBlock = conditionElse.transform.GetChild(0).tag;

        //Determine if there is a loop code block in the panel
        ifCodeBlockTags.AddRange(isLoop(ifBlock));
        elseCodeBlockTags.AddRange(isLoop(elseBlock));

        Debug.Log("Slime "+isSlime);
        Debug.Log("Gem "+isGem);

        if (option == 0)
        {
            if (isSlime)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else
            {
                codes.AddRange(elseCodeBlockTags);
            }
        }
        else if (option == 1)
        {
            if (isGem)
            {
                codes.AddRange(ifCodeBlockTags);
            }
            else
            {
                codes.AddRange(elseCodeBlockTags);
            }
        }

        isSlime = false;
        isGem = false;

        return codes;
    }

    private ArrayList isLoop(string s)// Determine if there is a loop or subloop inside the if block
    {
        ArrayList temp = new ArrayList();

        if (s.Equals("Loop"))//If there is a loop, add the loop to the array
        {
            temp.AddRange(Execute.LoopBlockTags);
        }
        else if (s.Equals("SubLoop"))//If there is a subloop, add the subloop to the array
        {
            temp.AddRange(Execute.SubLoopBlockTags);
        }
        else
        {
            temp.Add(s);//If not, just add the original command
        }

        return temp;
    }
}
