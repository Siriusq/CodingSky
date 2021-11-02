using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenerateBlock : MonoBehaviour, IPointerDownHandler
{
    private GameObject block;
    //private bool isDraging = false;

    // Update is called once per frame
/*    void Update()
    {
        if (isDraging)
        {
            
            block.SetActive(true);
            block.transform.position = this.transform.position;
            block.transform.localScale = new Vector3(1, 1, 1);

            if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                block = null;
            }
        }               
    }*/



    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log(eventData);
        GameObject prefab = null;
        //Todo: 这里用if或者switch判断不同块的名字，不行的话就用笨方法，每一个基础块都设置一个对应的脚本加载.  正常应该可以用for循环一下每个块的位置，如果对上了就是
        string objectTag = this.gameObject.tag;
        prefab = Resources.Load<GameObject>(objectTag);
        /*switch (objectTag)
        {
            case "MoveForward":
                prefab = Resources.Load<GameObject>("MoveForward");
            case "TurnLeft"
        }*/
        /*if (this.gameObject.tag.Equals("MoveForward"))
        {
            prefab = Resources.Load<GameObject>("MoveForward");//这里改成调用每个按钮的名字
        }*/
        

        //Todo: 这里到用到if和loop的关卡的时候设置一个选择器，选择点击后块出现在什么地方，选择器的内容对应下面Find里的内容
        if (prefab != null)
        {
            block = Instantiate(prefab);
            
            //block.transform.SetParent(this.transform.parent.parent.GetChild(1));
            block.transform.SetParent(this.transform.parent.parent.Find("Execute Panel"));//改变父类
            block.transform.localScale = new Vector3(1, 1, 1);//如果不设置的话默认会缩放成0.7倍，很奇怪
            //isDraging = true;
        }
    }
}
