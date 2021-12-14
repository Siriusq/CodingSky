using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void AlertParam(int param);

    [DllImport("__Internal")]
    private static extern void PostLevel(int level);


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
    public GameObject PausePanel;


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

    public void UnityCallJSFuncWithParam(int param)
    {
        AlertParam(param);
    }

    public void SendLevel(int level)
    {
        PostLevel(level);
    }

    public void Restart()//reload scene
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex;//buildingIndex is the index in build setting
        StartCoroutine(TransAnimation(tempIndex));
        StopCoroutine(TransAnimation(tempIndex));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()//pause game
    {
        if (Time.timeScale == 1)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ChooseLevel(int index)//Level selection menu, each level block passes in its own level number
    {
        int level = index + 1;//build inside the scene count from 0, 0 is the main, 1 is the selection of the level, so +1
        StartCoroutine(TransAnimation(level));
        StopCoroutine(TransAnimation(level));
    }

    public void BackToMenu()//Back to the level selection page
    {
        StartCoroutine(TransAnimation(1));
        StopCoroutine(TransAnimation(1));
    }

    public void SettingPanel()//Open Settings panel
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

    public void VolumePanel()//Turn on volume control panel
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

    public void ShowTips()//Tip Panel
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

    public void LevelComplete()//Level complete
    {
        if(winningPanel != null)
        {
            winningPanel.SetActive(true);
            int gemCount = 3 - GameObject.FindGameObjectsWithTag("Gem").Length;//crystal count
            if(gemCount > 2)
            {
                gems[2].SetActive(true);//3
            }
            if (gemCount > 1)
            {
                gems[1].SetActive(true);//2
            }
            if (gemCount > 0)
            {
                gems[0].SetActive(true);//1
            }
        }
    }

    public void Failed()//level failed
    {
        if(failPanel != null)
        {
            failPanel.SetActive(true);
        }
    }


    public void Continue()//Loading the next level
    {
        int tempIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(TransAnimation(tempIndex));
        StopCoroutine(TransAnimation(tempIndex));
    }

    IEnumerator TransAnimation(int index)//Load next scene, either with the next level button, or with the main menu switch to level selection
    {
        transAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(index);
        SendLevel(index-1);
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
