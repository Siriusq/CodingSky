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

    public void Play()//Main Menu 上面的 Play 按钮，按下后切换到关卡选择
    {
        SceneManager.LoadScene("Select");
    }


    public void Continue()//加载下一关
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(tempIndex);
        StartCoroutine(NextLevel(SceneManager.GetActiveScene().buildIndex + 1));//buildingIndex是build setting里面场景的序号
        //StopCoroutine(NextLevel(tempIndex));
    }

    IEnumerator NextLevel(int index)//携程加载下一个场景，可以用在下一关按钮，也可以用在主菜单切换到关卡选择上
    {
        transAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(index);
    }
}
