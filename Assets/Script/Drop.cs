using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/***************************************************************************************
*    Title: Unity UI: Drag & Drop
*    Author: Quill18
*    Date: 2015
*    Code version: 1.0
*    Availability: https://www.youtube.com/watch?v=P66SSOzCqFU&list=PLC2dBFqUwJkJFPnk67wTDvEr2_I0KPZNS&index=5
*
***************************************************************************************/

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//Get the dragged object
        if (drag != null)
        {
            drag.transformParentCache = this.transform;//Change the parent class of the staging to the panel where drop is located
            AudioManager.actionListener = 8;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //Skip if no object is dragged
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//Get the dragged object
        if (drag != null)
        {
            drag.emptyBlockParentCache = this.transform;//Change the parent class of the staging to the panel where drop is located
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //Skip if no object is dragged
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//Get the dragged object
        if (drag != null && drag.emptyBlockParentCache == this.transform)
        {
            drag.emptyBlockParentCache = drag.transformParentCache;//Change the parent class of the staging to the panel where drop is located
        }
    }
}
