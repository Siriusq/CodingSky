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
        //Todo: ������if����switch�жϲ�ͬ������֣����еĻ����ñ�������ÿһ�������鶼����һ����Ӧ�Ľű�����.  ����Ӧ�ÿ�����forѭ��һ��ÿ�����λ�ã���������˾���
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
            prefab = Resources.Load<GameObject>("MoveForward");//����ĳɵ���ÿ����ť������
        }*/
        

        //Todo: ���ﵽ�õ�if��loop�Ĺؿ���ʱ������һ��ѡ������ѡ������������ʲô�ط���ѡ���������ݶ�Ӧ����Find�������
        if (prefab != null)
        {
            block = Instantiate(prefab);
            
            //block.transform.SetParent(this.transform.parent.parent.GetChild(1));
            block.transform.SetParent(this.transform.parent.parent.Find("Execute Panel"));//�ı丸��
            block.transform.localScale = new Vector3(1, 1, 1);//��������õĻ�Ĭ�ϻ����ų�0.7���������
            //isDraging = true;
        }
    }
}
