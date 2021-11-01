using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//获取被drag的对象
        if (drag != null)
        {
            drag.transformParentCache = this.transform;//将暂存的父类更改为drop所在的面板
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //没有拖拽物体则跳过
        if(eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//获取被drag的对象
        if (drag != null)
        {
            drag.emptyBlockParentCache = this.transform;//将暂存的父类更改为drop所在的面板
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //没有拖拽物体则跳过
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//获取被drag的对象
        if (drag != null && drag.emptyBlockParentCache == this.transform)
        {
            drag.emptyBlockParentCache = drag.transformParentCache;//将暂存的父类更改为drop所在的面板
        }
    }
}
