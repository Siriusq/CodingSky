using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject messagePrefab;

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
        AudioManager.actionListener = 7;
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
                    //������߲���һ�У���Ϊgird�е�������һ��7��               
                    if (emptyBlockParentCache.childCount > 7)
                    {
                        float y = emptyBlockParentCache.GetChild(7).position.y - emptyBlockParentCache.GetChild(0).position.y;//����Ӧ����8������unity����child��ű䶯��ʱ���Լ�-1��������7�������8�Ļ��ᱨ�����
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

        //��ִֹ�����ͼ�����
        if (this.transform.parent.CompareTag("execute_panel") && this.transform.parent.childCount > 29){
            Destroy(this.gameObject);
            SendMessage(0);
        }

        //��ֹѭ�����ͼ�����
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.parent.childCount > 15)
        {            
            Destroy(this.gameObject);
            SendMessage(1);
        }

        //��ֹ��ѭ�����ͼ�����
        if (this.transform.parent.CompareTag("SubLoopPanel") && this.transform.parent.childCount > 8)
        {
            Destroy(this.gameObject);
            SendMessage(2);
        }

        // if ��������е��ж�
        if (this.transform.parent.CompareTag("SubCondition")|| this.transform.parent.CompareTag("SubConditionElse"))
        {
            //Debug.Log(this.transform.parent.childCount);
            // ��ֹ�û������飬�� if ������ϵ��Լ���condition��ȥ
            if (this.transform.tag.Equals("If"))
            {
                Destroy(this.gameObject);
                SendMessage(3);
            }
            // ���������Ѿ�������������飬��ô���϶��µĴ�������֮��ɾ��ԭ���ģ����2������Ϊunity�޴���List��������
            else if(this.transform.parent.childCount > 2)
            {
                DestroyImmediate(this.transform.parent.GetChild(2).gameObject);
                //gameManager.WarningPopup("Conditional code block cannot be used in their own panels!");
            }
        }

        //��ֹ�û���ѭ��������ϵ��Լ���ѭ����ȥ
        if (this.transform.parent.CompareTag("LoopPanel") && this.transform.tag.Equals("Loop"))
        {
            Destroy(this.gameObject);
            SendMessage(4);
        }

        //Todo: ���if����������ǿյģ�������
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
                //�û��Ȱ�ѭ���Ͻ�if��壬�ٰ�if�Ͻ�ѭ����壬���Ÿ�������
                if (this.transform.parent.CompareTag("LoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop"))
                    {
                        Destroy(this.gameObject);
                    }
                }
                //��ѭ�� ͬ��
                if (this.transform.parent.CompareTag("SubLoopPanel"))
                {
                    if (ifContent.Equals("Loop") || elseContent.Equals("Loop") || ifContent.Equals("SubLoop") || elseContent.Equals("SubLoop"))
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }        

        //��ֹ���ѭ��������if��Ȼ���ٰ�ѭ���ϵ�if�����
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("Loop"))
        {
            //��ѭ��������Ԫ��
            GameObject loopP = GameObject.FindGameObjectWithTag("LoopPanel");
            bool findIf = false;
            //��if�Ļ�
            foreach (Transform loopBlock in loopP.transform)
            {
                if (loopBlock.tag.Equals("If"))
                {
                    findIf = true;
                    Destroy(this.gameObject);
                }
            }
            //ѭ���ϵ�if�����ֱ������
            if (findIf)
            {
                Destroy(this.gameObject);
            }            
        }

        //��ֹ�����ѭ��������if��Ȼ���ٰ���ѭ���ϵ�if�����
        if ((this.transform.parent.CompareTag("SubCondition") || this.transform.parent.CompareTag("SubConditionElse")) && this.transform.tag.Equals("SubLoop"))
        {
            //��ѭ��������Ԫ��
            GameObject subLoopP = GameObject.FindGameObjectWithTag("SubLoopPanel");
            bool findIf = false;
            //��if�Ļ�
            foreach (Transform subLoopBlock in subLoopP.transform)
            {
                if (subLoopBlock.tag.Equals("If"))
                {
                    findIf = true;
                    Destroy(this.gameObject);
                }
            }
            //ѭ���ϵ�if�����ֱ������
            if (findIf)
            {
                Destroy(this.gameObject);
            }
        }

        //��ֹ�û���ѭ��������ϵ��Լ���ѭ����ȥ + ��ֹ�û�����ѭ���ϵ���ѭ����ȥ
        if (this.transform.parent.CompareTag("SubLoopPanel") && (this.transform.tag.Equals("Loop") || this.transform.tag.Equals("SubLoop")))
        {
            Destroy(this.gameObject);
            SendMessage(6);
        }

        //ɾ���϶�������
        if (this.transform.parent.CompareTag("Delete")){
            Destroy(this.gameObject);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;//��ק����ʱ�ָ�UI����
        Destroy(emptyBlock);//����ʱɾ��ռλ��
    }

    private void Constraint()
    {

    }


    public void SendMessage(int i)//����
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

        AudioManager.actionListener = 1;//֪ͨAudioManager���ž�����Ч

        StartCoroutine(WaitMessage(s));
        StopCoroutine(WaitMessage(s));
        GameObject[] usedMessage = GameObject.FindGameObjectsWithTag("Warning");//���ٵ���
        foreach(GameObject message in usedMessage)
        {
            Destroy(message,4.5f);
        }
    }

    IEnumerator WaitMessage(string s)//ʵ����������Ȼ�󲥷Ŷ���
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        var message = Instantiate(messagePrefab);
        message.transform.SetParent(canvas.transform);
        Text t = message.GetComponentInChildren<Text>();
        t.text = s;
        yield return new WaitForSeconds(2f);
    }
}
