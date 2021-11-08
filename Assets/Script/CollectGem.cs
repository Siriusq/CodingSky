using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGem : MonoBehaviour
{
    public int gemsCount = 0;//��ʯ������
    bool collect;
    bool isGem = false;//Ifģ���������ж�ǰ���ǲ��Ǳ�ʯ�Ĳ���
    public bool IsGem => isGem;

    public void OnTriggerStay(Collider gemCollider)//������ڱ�ʯ��ײ����ͣ��ʱ
    {
        collect = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canCollect;//��ȡ�ƶ��ű���Ĳ���ֵ���ж��ܷ�ʰȡ����
        isGem = true;
        if (gemCollider.tag == "Player" && collect)
        {
            Debug.Log("Collected!");
            gemsCount++;
            Debug.Log(gemsCount);
            Destroy(gameObject);//ɾ����ʯ���൱�ڳԵ�
            collect = false;
        }
    }

    public void OnTriggerEnter(Collider gemCollider)//��ҽ��뱦ʯ����ײ��ʱ������ʯ��ֹ��ģ
    {
        if (gemCollider.tag == "Player")
        {
            Debug.Log("Enter!");
            Tweener tweener = transform.DOMove(this.transform.position + transform.forward, 1.2f);
        }
    }

    public void OnTriggerExit(Collider gemCollider)
    {
        if (gemCollider.tag == "Player")
        {
            isGem = false;
        }
    }
}
