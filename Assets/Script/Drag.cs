using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject messagePrefab;

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
        AudioManager.actionListener = 7;
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
            SendMessage(0);
        }

        //防止循环面板图标溢出
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.parent.childCount > 15)
        {            
            Destroy(this.gameObject);
            SendMessage(1);
        }

        //防止子循环面板图标溢出
        if (this.transform.parent.CompareTag("SubLoopPanel") && this.transform.parent.childCount > 8)
        {
            Destroy(this.gameObject);
            SendMessage(2);
        }

        // if 面板托盘中的判断
        if (this.transform.parent.CompareTag("SubCondition")|| this.transform.parent.CompareTag("SubConditionElse"))
        {
            //Debug.Log(this.transform.parent.childCount);
            // 防止用户搞事情，把 if 代码块拖到自己的condition中去
            if (this.transform.tag.Equals("If"))
            {
                Destroy(this.gameObject);
                SendMessage(3);
            }
            // 如果面板中已经存在其他代码块，那么在拖动新的代码块过来之后删除原来的，这个2还是因为unity愚蠢的List计数问题
            else if(this.transform.parent.childCount > 2)
            {
                DestroyImmediate(this.transform.parent.GetChild(2).gameObject);
                //gameManager.WarningPopup("Conditional code block cannot be used in their own panels!");
            }
        }

        //防止用户把循环代码块拖到自己的循环里去
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.tag.Equals("Loop"))
        {
            Destroy(this.gameObject);
            SendMessage(4);
        }

        //Todo: 如果if的面板里面是空的，不能拖
        if (this.transform.tag.Equals("If"))
        {
            GameObject If = GameObject.FindGameObjectWithTag("SubCondition");
            GameObject Else = GameObject.FindGameObjectWithTag("SubConditionElse");
            if (If.transform.childCount == 0 || Else.transform.childCount == 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                string ifContent = If.transform.GetChild(0).tag;
                string elseContent = Else.transform.GetChild(0).tag;
                //用户先把循环拖进if面板，再把if拖进循环面板，搁着给我套娃
                if (this.transform.parent.CompareTag("LoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop"))
                    {
                        Destroy(this.gameObject);
                    }
                }
                //子循环 同上
                if (this.transform.parent.CompareTag("SubLoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop") || ifContent.Equals("SubLoop") || elseContent.Equals("SubLoop"))
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }        

        //防止如果循环面板存在if，然后再把循环拖到if的面板
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("Loop"))
        {
            //找循环面板里的元素
            GameObject loopP = GameObject.FindGameObjectWithTag("LoopPanel");
            bool findIf = false;
            //有if的话
            foreach (Transform loopBlock in loopP.transform)
            {
                if (loopBlock.tag.Equals("If"))
                {
                    findIf = true;
                    Destroy(this.gameObject);
                }
            }
            //循环拖到if面板是直接销毁
            if (findIf)
            {
                Destroy(this.gameObject);
            }            
        }

        //防止如果子循环面板存在if，然后再把子循环拖到if的面板
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("SubLoop"))
        {
            //找循环面板里的元素
            GameObject subLoopP = GameObject.FindGameObjectWithTag("SubLoopPanel");
            bool findIf = false;
            //有if的话
            foreach (Transform subLoopBlock in subLoopP.transform)
            {
                if (subLoopBlock.tag.Equals("If"))
                {
                    findIf = true;
                    Destroy(this.gameObject);
                }
            }
            //循环拖到if面板是直接销毁
            if (findIf)
            {
                Destroy(this.gameObject);
            }
        }

        //防止用户把循环代码块拖到自己的循环里去 + 防止用户把主循环拖到子循环里去
        if (this.transform.parent.CompareTag("SubLoopPanel") && (this.transform.tag.Equals("Loop") || this.transform.tag.Equals("SubLoop")))
        {
            Destroy(this.gameObject);
            SendMessage(6);
        }

        //删除拖动的物体
        if (this.transform.parent.CompareTag("Delete")){
            Destroy(this.gameObject);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;//拖拽结束时恢复UI拦截
        Destroy(emptyBlock);//结束时删除占位符
    }

    private void Constraint()
    {

    }


    public void SendMessage(int i)//弹窗
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
                s = "Conditional code block loops are not supported at this time!";
                break;
            case 6:
                s = "Cannot use a loop code block in the same loop/subloop panel!";
                break;         

        }

        AudioManager.actionListener = 1;//通知AudioManager播放警告音效

        StartCoroutine(WaitMessage(s));
        StopCoroutine(WaitMessage(s));
        GameObject[] usedMessage = GameObject.FindGameObjectsWithTag("Warning");//销毁弹窗
        foreach(GameObject message in usedMessage)
        {
            Destroy(message,4.5f);
        }
    }

    IEnumerator WaitMessage(string s)//实例化弹窗，然后播放动画
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        var message = Instantiate(messagePrefab);
        message.transform.SetParent(canvas.transform);
        Text t = message.GetComponentInChildren<Text>();
        t.text = s;
        yield return new WaitForSeconds(2f);
    }
}
