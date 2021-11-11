using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager manager;
    public Animator transAnimation;
    public GameObject winningPanel;
    public GameObject[] gems;
    public static bool isWin = false;
    public GameObject warningPanel;
    public Animator messageAnimation;

    void Awake()
    {
        if(manager != null)
        {
            Destroy(gameObject);
        }
        manager = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()//���¿�ʼ��Ϸ
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex;//buildingIndex��build setting���泡�������
        StartCoroutine(TransAnimation(tempIndex));
        StopCoroutine(TransAnimation(tempIndex));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()//��ͣ��Ϸ
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ChooseLevel(int index)//�ؿ�ѡ��˵���ÿ���ؿ��鴫���Լ��Ĺؿ���
    {
        int level = index + 1;//build����scene��0��ʼ������0��main��1��ѡ�أ�����+1
        StartCoroutine(TransAnimation(level));
        StopCoroutine(TransAnimation(level));
    }

    public void BackToMenu()//�ص��ؿ�ѡ��ҳ��
    {
        StartCoroutine(TransAnimation(1));
        StopCoroutine(TransAnimation(1));
    }

    public void SettingPanel()//������
    {

    }

    public void VolumePanel()//����������
    {

    }

    public void LevelComplete()//ͨ��ҳ��
    {
        if(winningPanel != null)
        {
            winningPanel.SetActive(true);
            int gemCount = 3 - GameObject.FindGameObjectsWithTag("Gem").Length;//������ʯ������ʣ�µľ����ռ��˵�
            if(gemCount > 2)
            {
                gems[2].SetActive(true);//����
            }
            if (gemCount > 1)
            {
                gems[1].SetActive(true);//����
            }
            if (gemCount > 0)
            {
                gems[0].SetActive(true);//һ��
            }
        }
    }


    public void Continue()//������һ��
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(TransAnimation(tempIndex));//buildingIndex��build setting���泡�������
        StopCoroutine(TransAnimation(tempIndex));
    }

    IEnumerator TransAnimation(int index)//Я�̼�����һ������������������һ�ذ�ť��Ҳ�����������˵��л����ؿ�ѡ����
    {
        transAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(index);
    }
}
