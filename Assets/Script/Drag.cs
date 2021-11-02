using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform transformParentCache = null;//��ʱ�洢ͼ��ԭ�ڵ���常��

    public Transform originalParent = null;//�ʼ���������ĸ��໺��
    private GameObject block;//Ԥ�Ƽ����ɵ���Ϸ����
    private GameObject prefab = null;//���ص�Ԥ�Ƽ�

    GameObject emptyBlock = null;//ռλ��
    public Transform emptyBlockParentCache = null;//�ݴ�ռλ���ĸ���

    public void OnBeginDrag(PointerEventData eventData)
    {
        //����������ռλ��
        emptyBlock = new GameObject();
        emptyBlock.transform.SetParent(this.transform.parent);

        //������������basic����µ����࣬��ô������������������Ƽ��ض�Ӧ��Ԥ�Ƽ�
        if (this.transform.parent.CompareTag("basic_command_panel"))
        {          
            string objectTag = this.gameObject.tag;
            prefab = Resources.Load<GameObject>(objectTag);
            originalParent = this.transform.parent;
            //Debug.Log(originalParent.name);
        }

        //�������ߴ�
        LayoutElement layoutElement = emptyBlock.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;
        
        emptyBlock.transform.SetSiblingIndex(this.transform.GetSiblingIndex());//����ռλ���ڸ����е�λ��
        
        transformParentCache = this.transform.parent;
        emptyBlockParentCache = transformParentCache;
        this.transform.SetParent(this.transform.parent.parent);//����ק����ĸ�������Ϊ�������ĸ��ࣨ��������үү��

        GetComponent<CanvasGroup>().blocksRaycasts = false;//��ק����ʱ�ر�UI����
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;//ʵ����קЧ��

        //��ռλ���ĸ��෢���仯ʱ�ı丸�໺��
        if (emptyBlock.transform.parent != emptyBlockParentCache)
        {
            emptyBlock.transform.SetParent(emptyBlockParentCache);
        }

        //ʵ�����������м����������ק�Ŀ�
        int positionIndex = emptyBlockParentCache.childCount;//λ�ò���
        
        //����λ��
        for (int i = 0; i < emptyBlockParentCache.childCount; i++)
        {
            this.transform.SetSiblingIndex(positionIndex);            
            if (this.transform.position.x < emptyBlockParentCache.GetChild(i).position.x)
            {
                positionIndex = i;
                //�����ק��������Ŀ��λ�ò���ͬһ�У��򽫲���+8��8��һ�еĸ�����������������ֻ��֧�����У��ö�ȡ���м���˼��У�Ȼ��ͼӼ���8
                if (this.transform.position.y < emptyBlockParentCache.GetChild(i).position.y)
                {
                    int row = 0;
                    //������߲���һ�У�6��ԭ��������һ��ֻ����7���飩                    
                    if (emptyBlockParentCache.childCount > 7)
                    {
                        float y = emptyBlockParentCache.GetChild(8).position.y - emptyBlockParentCache.GetChild(1).position.y;
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
        Debug.Log(positionIndex);
        emptyBlock.transform.SetSiblingIndex(positionIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        this.transform.SetParent(transformParentCache);//drop���Ѿ����ݴ�ֵ����Ϊdrop���ڵ���壬������ı�drop����Ʒ�ĸ��࣬ʵ��һ������ƶ�����һ������Ч��
        this.transform.SetSiblingIndex(emptyBlock.transform.GetSiblingIndex());

        // ����ԭ������
        if (prefab != null && !this.transform.parent.CompareTag("basic_command_panel"))
        {
            block = Instantiate(prefab);
            block.transform.SetParent(originalParent);//�ı丸��
            block.transform.localScale = new Vector3(1, 1, 1);//��������õĻ�Ĭ�ϻ����ų�0.7���������
            prefab = null;
        }

        if (this.transform.parent.CompareTag("SubCondition"))
        {
            Destroy(this.transform.parent.GetChild(1));
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;//��ק����ʱ�ָ�UI����
        Destroy(emptyBlock);//����ʱɾ��ռλ��
    }
}
