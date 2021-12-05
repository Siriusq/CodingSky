using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGem : MonoBehaviour
{
    public static int gemsCount = 0;//��ʯ������
    bool collect;
    public static bool isGem = false;//Ifģ���������ж�ǰ���ǲ��Ǳ�ʯ�Ĳ���
    public bool gemsTest = false;

    private void Start()
    {
        DOTween.Clear(true);
    }

    void OnTriggerStay(Collider gemCollider)//������ڱ�ʯ��ײ����ͣ��ʱ
    {
        collect = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canCollect;//��ȡ�ƶ��ű���Ĳ���ֵ���ж��ܷ�ʰȡ����
        isGem = true;
        if (gemCollider.tag == "Player" && collect)
        {
            //Debug.Log("Collected!");
            gemsCount++;
            AudioManager.actionListener = 3;
            Debug.Log(gemsCount);
            Destroy(gameObject);//ɾ����ʯ���൱�ڳԵ�
            collect = false;
            isGem = false;
        }
    }

    void OnTriggerEnter(Collider gemCollider)//��ҽ��뱦ʯ����ײ��ʱ������ʯ��ֹ��ģ
    {
        if (gemCollider.tag == "Player")
        {
            //Debug.Log("rrr!");
            gemsTest = true;
            Tweener tweener = transform.DOMove(this.transform.position + transform.forward, 1.2f);
        }
    }

    void OnTriggerExit(Collider gemCollider)
    {
        if (gemCollider.tag == "Player")
        {
            isGem = false;
            gemsTest = false;
        }
    }
}
