using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform transformParentCache = null;//暂时存储图标原在的面板父类

    public Transform originalParent = null;//最开始点击的物体的父类缓存
    private GameObject block;//预制件生成的游戏物体
    private GameObject prefab = null;//加载的预制件

    GameObject emptyBlock = null;//占位符
    public Transform emptyBlockParentCache = null;//暂存占位符的父类

    public void OnBeginDrag(PointerEventData eventData)
    {
        //创建代码块空占位符
        emptyBlock = new GameObject();
        emptyBlock.transform.SetParent(this.transform.parent);

        //如果鼠标点击的是basic面板下的子类，那么根据所点击的物体名称加载对应的预制件
        if (this.transform.parent.CompareTag("basic_command_panel"))
        {          
            string objectTag = this.gameObject.tag;
            prefab = Resources.Load<GameObject>(objectTag);
            originalParent = this.transform.parent;
            //Debug.Log(originalParent.name);
        }

        //设置面板尺寸
        LayoutElement layoutElement = emptyBlock.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
        
        emptyBlock.transform.SetSiblingIndex(this.transform.GetSiblingIndex());//设置占位符在格子中的位置
        
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
                    //如果两者不在一行，因为gird中的限制是一行7个               
                    if (emptyBlockParentCache.childCount > 7)
                    {
                        float y = emptyBlockParentCache.GetChild(7).position.y - emptyBlockParentCache.GetChild(0).position.y;//正常应该是8，但是unity会在child序号变动的时候自己-1，所以是7，如果是8的话会报错溢出
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
        this.transform.SetParent(transformParentCache);//drop中已经将暂存值设置为drop所在的面板，这里更改被drop的物品的父类，实现一个面板移动到另一个面板的效果
        this.transform.SetSiblingIndex(emptyBlock.transform.GetSiblingIndex());

        // 生成原有物体
        if (prefab != null && !this.transform.parent.CompareTag("basic_command_panel"))
        {
            block = Instantiate(prefab);
            block.transform.SetParent(originalParent);//改变父类
            block.transform.localScale = new Vector3(1, 1, 1);//如果不设置的话默认会缩放成0.7倍，很奇怪
            prefab = null;
        }

        //防止执行面板图标溢出
        if (this.transform.parent.CompareTag("execute_panel") && this.transform.parent.childCount > 29){
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //防止循环面板图标溢出
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.parent.childCount > 15)
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //防止子循环面板图标溢出
        if (this.transform.parent.CompareTag("SubLoopPanel") && this.transform.parent.childCount > 8)
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }



        // if 面板托盘中的判断
        if (this.transform.parent.CompareTag("SubCondition"))
        {
            //Debug.Log(this.transform.parent.childCount);
            // 防止用户搞事情，把 if 代码块拖到自己的condition中去
            if (this.transform.tag.Equals("If"))
            {
                Destroy(this.gameObject);
                // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
            }
            // 如果面板中已经存在其他代码块，那么在拖动新的代码块过来之后删除原来的，这个2还是因为unity愚蠢的List计数问题
            else if(this.transform.parent.childCount > 2)
            {
                DestroyImmediate(this.transform.parent.GetChild(2).gameObject);
                // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
            }
        }

        //防止用户把循环代码块拖到自己的循环里去
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.tag.Equals("Loop"))
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //防止用户把if拖动到循环里去
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.tag.Equals("If"))
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //防止用户把if拖动到子循环里去
        if (this.transform.parent.CompareTag("SubLoopPanel") && this.transform.tag.Equals("If"))
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //防止用户把循环代码块拖到自己的循环里去 + 防止用户把主循环拖到子循环里去
        if (this.transform.parent.CompareTag("SubLoopPanel") && (this.transform.tag.Equals("Loop") || this.transform.tag.Equals("SubLoop")))
        {
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        //删除拖动的物体
        if (this.transform.parent.CompareTag("Delete")){
            Destroy(this.gameObject);
            // Todo: 可以加一个提示，检测到的时候提示不能这么干，也可以加一个音效
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;//拖拽结束时恢复UI拦截
        Destroy(emptyBlock);//结束时删除占位符
    }
}
