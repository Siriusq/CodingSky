using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform transformParentCache = null;//��ʱ�洢ͼ��ԭ�ڵ���常��
    
    GameObject emptyBlock = null;//ռλ��
    public Transform emptyBlockParentCache = null;//�ݴ�ռλ���ĸ���

    public void OnBeginDrag(PointerEventData eventData)
    {
        //����������ռλ��
        emptyBlock = new GameObject();
        emptyBlock.transform.SetParent(this.transform.parent);

        //�������ߴ�
        LayoutElement layoutElement = emptyBlock.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;

        emptyBlock.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        
        transformParentCache = this.transform.parent;
        emptyBlockParentCache = transformParentCache;
        this.transform.SetParent(this.transform.parent.parent);//����ק����ĸ�������Ϊ�������ĸ��ࣨ��������үү��

        GetComponent<CanvasGroup>().blocksRaycasts = false;//��ק����ʱ�ر�UI����
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;//ʵ����קЧ��

        //��ռλ���ĸ��෢���仯ʱ�ı丸�໺��
        if(emptyBlock.transform.parent != emptyBlockParentCache)
        {
            emptyBlock.transform.SetParent(emptyBlockParentCache);
        }

        //ʵ�����������м����������ק�Ŀ�
        int positionIndex = emptyBlockParentCache.childCount;//λ�ò���
        //����λ��
        // Todo: Ŀǰֻ�ܵ��в��룬������Ҫʵ�ֶ��в��룬���y���ж�
        for(int i = 0; i < emptyBlockParentCache.childCount; i++)
        {
            if(this.transform.position.x < emptyBlockParentCache.GetChild(i).position.x)
            {
                positionIndex = i;
                if (emptyBlock.transform.GetSiblingIndex() < positionIndex)
                {
                    positionIndex--;
                }
                //emptyBlockSpace.transform.SetSiblingIndex(i);
                break;
            }
        }
        emptyBlock.transform.SetSiblingIndex(positionIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(transformParentCache);//drop���Ѿ����ݴ�ֵ����Ϊdrop���ڵ���壬������ı�drop����Ʒ�ĸ��࣬ʵ��һ������ƶ�����һ������Ч��
        this.transform.SetSiblingIndex(emptyBlock.transform.GetSiblingIndex());
        //emptyBlockSpace.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;//��ק����ʱ�ָ�UI����

        Destroy(emptyBlock);//����ʱɾ��ռλ��

    }



/*    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
