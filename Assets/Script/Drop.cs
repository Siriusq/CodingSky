using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//��ȡ��drag�Ķ���
        if (drag != null)
        {
            drag.transformParentCache = this.transform;//���ݴ�ĸ������Ϊdrop���ڵ����
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //û����ק����������
        if(eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//��ȡ��drag�Ķ���
        if (drag != null)
        {
            drag.emptyBlockParentCache = this.transform;//���ݴ�ĸ������Ϊdrop���ڵ����
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*throw new System.NotImplementedException();*/
        //û����ק����������
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag drag = eventData.pointerDrag.GetComponent<Drag>();//��ȡ��drag�Ķ���
        if (drag != null && drag.emptyBlockParentCache == this.transform)
        {
            drag.emptyBlockParentCache = drag.transformParentCache;//���ݴ�ĸ������Ϊdrop���ڵ����
        }
    }
}
