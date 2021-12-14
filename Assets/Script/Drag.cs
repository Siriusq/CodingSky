using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject messagePrefab;

    public Transform transformParentCache = null;//Temporarily store the parent class of the panel where the block was originally located

    public Transform originalParent = null;//Cache of the parent class of the first clicked object
    private GameObject block;//Game objects generated from prefabricated parts
    private GameObject prefab = null;//Loaded prefabs

    GameObject emptyBlock = null;//placeholder
    public Transform emptyBlockParentCache = null;//Parent of the temporary placeholder

    /***************************************************************************************
    *    Title: Unity UI: Drag & Drop
    *    Author: Quill18
    *    Date: 2015
    *    Code version: 1.0
    *    Availability: https://www.youtube.com/watch?v=P66SSOzCqFU&list=PLC2dBFqUwJkJFPnk67wTDvEr2_I0KPZNS&index=5
    *    Used in OnBeginDrag/OnDrag/OnEndDrag
    ***************************************************************************************/

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Creating empty placeholders for code blocks
        emptyBlock = new GameObject();
        emptyBlock.transform.SetParent(this.transform.parent);

        //If the mouse click is on a subclass under the BASIC panel, the corresponding prefab is loaded according to the name of the clicked object
        if (this.transform.parent.CompareTag("basic_command_panel"))
        {          
            string objectTag = this.gameObject.tag;
            prefab = Resources.Load<GameObject>(objectTag);
            originalParent = this.transform.parent;
            //Debug.Log(originalParent.name);
        }

        //Set panel size
        LayoutElement layoutElement = emptyBlock.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
        
        emptyBlock.transform.SetSiblingIndex(this.transform.GetSiblingIndex());//Set the position of the placeholder in the grid

        transformParentCache = this.transform.parent;
        emptyBlockParentCache = transformParentCache;
        this.transform.SetParent(this.transform.parent.parent);//Set the parent class of the dragged object to the parent class of the panel it's in (which is its grandfather)

        GetComponent<CanvasGroup>().blocksRaycasts = false;//Turn off UI blocking when dragging and dropping objects
        AudioManager.actionListener = 7;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;//Implementing the drag-and-drop effect

        //Change parent class cache when parent class of placeholder changes
        if (emptyBlock.transform.parent != emptyBlockParentCache)
        {
            emptyBlock.transform.SetParent(emptyBlockParentCache);
        }

        //Implement inserting the block being dragged in between two blocks
        int positionIndex = emptyBlockParentCache.childCount;//Location Parameters

        //Traversing positions
        for (int i = 0; i < emptyBlockParentCache.childCount; i++)
        {
            this.transform.SetSiblingIndex(positionIndex);            
            if (this.transform.position.x < emptyBlockParentCache.GetChild(i).position.x)
            {
                positionIndex = i;
                //If the dragged object is not in the same row as the target position, the parameter will be +8, 8 is the number of grids in a row, but this can only support two rows, have to read to the middle of the difference between a few rows, and then add a few 8
                if (this.transform.position.y < emptyBlockParentCache.GetChild(i).position.y)
                {
                    int row = 0;
                    //If the two are not on one line, as the limit in gird is 7 on one line               
                    if (emptyBlockParentCache.childCount > 7)
                    {
                        float y = emptyBlockParentCache.GetChild(7).position.y - emptyBlockParentCache.GetChild(0).position.y;//Normal should be 8, but unity will be -1 when the child number changes, so it is 7, if it is 8 will report an error overflow
                        row = (int)((emptyBlockParentCache.GetChild(i).position.y - this.transform.position.y) / y) * (-1);
                    }                    
                    positionIndex += 7 * row;
                }                
                if (emptyBlock.transform.GetSiblingIndex() < positionIndex)
                {
                    positionIndex--;
                }                
                break;
            }
        }
        //Debug.Log(positionIndex);
        emptyBlock.transform.SetSiblingIndex(positionIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        this.transform.SetParent(transformParentCache);//drop has set the staging value to the panel where the drop is located, here change the parent class of the dropped item to achieve the effect of moving one panel to another
        this.transform.SetSiblingIndex(emptyBlock.transform.GetSiblingIndex());

        //Generate original object
        if (prefab != null && !this.transform.parent.CompareTag("basic_command_panel"))
        {
            block = Instantiate(prefab);
            block.transform.SetParent(originalParent);
            block.transform.localScale = new Vector3(1, 1, 1);
            prefab = null;
        }

        //Determine whether the operation is legal or not, and pop up an alert if it is not
        Constraint();

        //Delete dragged objects
        if (this.transform.parent.CompareTag("Delete")){
            Destroy(this.gameObject);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;//Resume UI blocking at the end of drag and drop
        Destroy(emptyBlock);//Delete placeholder at the end
    }

    private void Constraint()
    {
        //Prevent users from dragging loop blocks into their own loops + Prevent users from dragging main loops into subloops
        if (this.transform.parent.CompareTag("SubLoopPanel") && (this.transform.tag.Equals("Loop") || this.transform.tag.Equals("SubLoop")))
        {
            Destroy(this.gameObject);
            SendMessage(6);
        }

        //Prevent execution panel icon overflow
        if (this.transform.parent.CompareTag("execute_panel") && this.transform.parent.childCount > 29)
        {
            Destroy(this.gameObject);
            SendMessage(0);
        }

        //Prevent loop panel icon overflow
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.parent.childCount > 15)
        {
            Destroy(this.gameObject);
            SendMessage(1);
        }

        //Prevent subloop panel icons from overflowing
        if (this.transform.parent.CompareTag("SubLoopPanel") && this.transform.parent.childCount > 8)
        {
            Destroy(this.gameObject);
            SendMessage(2);
        }

        // if Judgment in the panel tray
        if (this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse"))
        {
            //Debug.Log(this.transform.parent.childCount);
            // Prevent users from messing up and dragging if code blocks into their own condition
            if (this.transform.tag.Equals("If"))
            {
                Destroy(this.gameObject);
                SendMessage(3);
            }
            // If other code blocks already exist in the panel, then delete the original after dragging the new block over, this 2 or because of unity's stupid List counting problem
            else if (this.transform.parent.childCount > 2)
            {
                DestroyImmediate(this.transform.parent.GetChild(2).gameObject);
                //gameManager.WarningPopup("Conditional code block cannot be used in their own panels!");
            }
        }

        //Prevent users from dragging loop code blocks into their own loops
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.tag.Equals("Loop"))
        {
            Destroy(this.gameObject);
            SendMessage(4);
        }

        if (this.transform.tag.Equals("If"))
        {
            GameObject If = GameObject.FindGameObjectWithTag("SubCondition");
            GameObject Else = GameObject.FindGameObjectWithTag("SubConditionElse");
            if (If.transform.childCount == 0 || Else.transform.childCount == 0)
            {
                Destroy(this.gameObject);
                if (!this.transform.parent.CompareTag("Delete"))
                {
                    SendMessage(7);
                }
            }
            else
            {
                string ifContent = If.transform.GetChild(0).tag;
                string elseContent = Else.transform.GetChild(0).tag;
                //The user first drags the loop into the if panel, and then drags the if into the loop panel
                if (this.transform.parent.CompareTag("LoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop"))
                    {
                        Destroy(this.gameObject);
                        SendMessage(5);
                    }
                }
                //Sub-loop Same as above
                if (this.transform.parent.CompareTag("SubLoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop") || ifContent.Equals("SubLoop") || elseContent.Equals("SubLoop"))
                    {
                        Destroy(this.gameObject);
                        SendMessage(5);
                    }
                }
            }
        }

        //Prevent if the loop panel exists if and then drag the loop to the if panel
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("Loop"))
        {
            //Find the element in the loop panel
            GameObject loopP = GameObject.FindGameObjectWithTag("LoopPanel");
            //With if words
            foreach (Transform loopBlock in loopP.transform)
            {
                if (loopBlock.tag.Equals("If"))
                {
                    Destroy(this.gameObject);
                    SendMessage(5);
                }
            }
        }

        //Prevent the existence of if in the subloop panel, and then drag the subloop to the if panel
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("SubLoop"))
        {
            //Find the element in the loop panel
            GameObject subLoopP = GameObject.FindGameObjectWithTag("SubLoopPanel");
            bool findIf = false;
            //With if words
            foreach (Transform subLoopBlock in subLoopP.transform)
            {
                if (subLoopBlock.tag.Equals("If"))
                {
                    findIf = true;
                    Destroy(this.gameObject);
                }
            }
            //Destroy directly when the loop is dragged to the if panel
            if (findIf)
            {
                Destroy(this.gameObject);
            }
        }
    }


    public void SendMessage(int i)//pop up
    {
        string s = "";
        switch (i)
        {
            case 0:
                s = "The execution panel can only hold a maximum of 28 code blocks, please optimize your code logic!";
                break;
            case 1:
                s = "The loop panel can only hold a maximum of 14 code blocks, please optimize your code logic!";
                break;
            case 2:
                s = "The subloop panel can only hold a maximum of 7 code blocks, please optimize your code logic!";
                break;
            case 3:
                s = "Conditional code block cannot be used in their own panels!";
                break;
            case 4:
                s = "Cannot use a loop code block in the same loop panel!";
                break;
            case 5:
                s = "Infinite loop detected, operation denied";
                break;
            case 6:
                s = "Cannot use a loop code block in the same loop/subloop panel!";
                break;
            case 7:
                s = "Cannot use empty an conditional statement!";
                break;

        }

        AudioManager.actionListener = 1;//Notify AudioManager to play a warning sound

        StartCoroutine(WaitMessage(s));
        StopCoroutine(WaitMessage(s));
        GameObject[] usedMessage = GameObject.FindGameObjectsWithTag("Warning");//Destroy pop-ups
        foreach (GameObject message in usedMessage)
        {
            Destroy(message,4.5f);
        }
    }

    IEnumerator WaitMessage(string s)//Instantiate the pop-up window and then play the animation
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        var message = Instantiate(messagePrefab);
        message.transform.SetParent(canvas.transform);
        Text t = message.GetComponentInChildren<Text>();
        t.text = s;
        yield return new WaitForSeconds(2f);
    }
}
