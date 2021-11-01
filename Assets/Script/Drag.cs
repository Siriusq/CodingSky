using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform transformParentCache = null;//暂时存储图标原在的面板父类
    
    GameObject emptyBlock = null;//占位符
    public Transform emptyBlockParentCache = null;//暂存占位符的父类

    public void OnBeginDrag(PointerEventData eventData)
    {
        //创建代码块空占位符
        emptyBlock = new GameObject();
        emptyBlock.transform.SetParent(this.transform.parent);

        //设置面板尺寸
        LayoutElement layoutElement = emptyBlock.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
        
        emptyBlock.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        
        transformParentCache = this.transform.parent;
        emptyBlockParentCache = transformParentCache;
        this.transform.SetParent(this.transform.parent.parent);//将拖拽物体的父类设置为所在面板的父类（就是她的爷爷）

        GetComponent<CanvasGroup>().blocksRaycasts = false;//拖拽物体时关闭UI拦截
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;//实现拖拽效果

        //当占位符的父类发生变化时改变父类缓存
        if (emptyBlock.transform.parent != emptyBlockParentCache)
        {
            emptyBlock.transform.SetParent(emptyBlockParentCache);
        }

        //实现在两个块中间插入正在拖拽的块
        int positionIndex = emptyBlockParentCache.childCount;//位置参数
        
        //遍历位置
        for (int i = 0; i < emptyBlockParentCache.childCount; i++)
        {
            this.transform.SetSiblingIndex(positionIndex);
            
            if (this.transform.position.x < emptyBlockParentCache.GetChild(i).position.x)
            {
                positionIndex = i;

                //如果拖拽的物体与目标位置不在同一行，则将参数+8，8是一行的格子数量，但是这样只能支持两行，得读取到中间差了几行，然后就加几个8
                if (this.transform.position.y < emptyBlockParentCache.GetChild(i).position.y)
                {
                    int row = 0;
                    //如果两者不在一行（7的原因是限制一行只能有8个块）
                    if (emptyBlockParentCache.childCount > 6)
                    {
                        float y = emptyBlockParentCache.GetChild(8).position.y - emptyBlockParentCache.GetChild(0).position.y;
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
        emptyBlock.transform.SetSiblingIndex(positionIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(transformParentCache);//drop中已经将暂存值设置为drop所在的面板，这里更改被drop的物品的父类，实现一个面板移动到另一个面板的效果
        this.transform.SetSiblingIndex(emptyBlock.transform.GetSiblingIndex());

        GetComponent<CanvasGroup>().blocksRaycasts = true;//拖拽结束时恢复UI拦截

        Destroy(emptyBlock);//结束时删除占位符
    }
}
