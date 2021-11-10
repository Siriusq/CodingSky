using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager manager;
    public Animator transAnimation;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void BackToMenu()//�ص��ؿ�ѡ��ҳ��
    {
        SceneManager.LoadScene("Main");
    }

    public void SettingPanel()//������
    {

    }

    public void VolumePanel()//����������
    {

    }

    public void LevelComplete()//ͨ��ҳ��
    {

    }

    public void Play()//Main Menu ����� Play ��ť�����º��л����ؿ�ѡ��
    {
        SceneManager.LoadScene("Select");
    }


    public void Continue()//������һ��
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(tempIndex);
        StartCoroutine(NextLevel(SceneManager.GetActiveScene().buildIndex + 1));//buildingIndex��build setting���泡�������
        //StopCoroutine(NextLevel(tempIndex));
    }

    IEnumerator NextLevel(int index)//Я�̼�����һ������������������һ�ذ�ť��Ҳ�����������˵��л����ؿ�ѡ����
    {
        transAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(index);
    }
}
