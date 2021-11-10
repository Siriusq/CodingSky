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

    public void Restart()//重新开始游戏
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()//暂停游戏
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

    public void BackToMenu()//回到关卡选择页面
    {
        SceneManager.LoadScene("Main");
    }

    public void SettingPanel()//打开设置
    {

    }

    public void VolumePanel()//打开音量控制
    {

    }

    public void LevelComplete()//通关页面
    {

    }
}
