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
    public GameObject volumePanel;
    public Animator volumeAnimation;
    public GameObject settingPanel;
    public Animator settingAnimation;
    public GameObject tipsPanel;
    public Animator tipsAnimation;
    public GameObject failPanel;
    public Animator fialAnimation;


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
        int tempIndex = SceneManager.GetActiveScene().buildIndex;//buildingIndex是build setting里面场景的序号
        StartCoroutine(TransAnimation(tempIndex));
        StopCoroutine(TransAnimation(tempIndex));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void ChooseLevel(int index)//关卡选择菜单，每个关卡块传入自己的关卡号
    {
        int level = index + 1;//build里面scene从0开始计数，0是main，1是选关，所以+1
        StartCoroutine(TransAnimation(level));
        StopCoroutine(TransAnimation(level));
    }

    public void BackToMenu()//回到关卡选择页面
    {
        StartCoroutine(TransAnimation(1));
        StopCoroutine(TransAnimation(1));
    }

    public void SettingPanel()//打开设置
    {
        if (settingPanel != null)
        {
            if(settingPanel.activeSelf == true)
            {                
                StartCoroutine(SettingWaitSeconds());
                StopCoroutine(SettingWaitSeconds());
            }
            else
            {
                settingPanel.SetActive(true);
            }
        }
    }

    public void VolumePanel()//打开音量控制
    {
        if (volumePanel != null)
        {
            if(volumePanel.activeSelf == true)
            {
                StartCoroutine(VolumeWaitSeconds());
                StopCoroutine(VolumeWaitSeconds());                
            }
            else
            {
                volumePanel.SetActive(true);
            }            
        }
    }

    public void ShowTips()//提示面板
    {
        if(tipsPanel != null)
        {
            if(tipsPanel.activeSelf == true)
            {
                StartCoroutine(TipsWaitSeconds());
                StopCoroutine(TipsWaitSeconds());
            }
            else
            {
                tipsPanel.SetActive(true);
            }
        }
    }

    public void LevelComplete()//通关页面
    {
        if(winningPanel != null)
        {
            winningPanel.SetActive(true);
            int gemCount = 3 - GameObject.FindGameObjectsWithTag("Gem").Length;//三个宝石，减掉剩下的就是收集了的
            if(gemCount > 2)
            {
                gems[2].SetActive(true);//三星
            }
            if (gemCount > 1)
            {
                gems[1].SetActive(true);//两星
            }
            if (gemCount > 0)
            {
                gems[0].SetActive(true);//一星
            }
        }
    }

    public void Failed()//狗子被弹飞后提示重开
    {
        if(failPanel != null)
        {
            failPanel.SetActive(true);
        }
    }


    public void Continue()//加载下一关
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(TransAnimation(tempIndex));//buildingIndex是build setting里面场景的序号
        StopCoroutine(TransAnimation(tempIndex));
    }

    IEnumerator TransAnimation(int index)//携程加载下一个场景，可以用在下一关按钮，也可以用在主菜单切换到关卡选择上
    {
        transAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(index);
    }

    IEnumerator SettingWaitSeconds()
    {
        settingAnimation.SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        settingPanel.SetActive(false);
    }

    IEnumerator VolumeWaitSeconds()
    {
        volumeAnimation.SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        volumePanel.SetActive(false);
    }

    IEnumerator TipsWaitSeconds()
    {
        tipsAnimation.SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        tipsPanel.SetActive(false);
    }
}
