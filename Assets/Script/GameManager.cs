using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager manager;
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
}
