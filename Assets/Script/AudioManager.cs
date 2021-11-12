using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;

    public static AudioMixer amInstance;

    public AudioSource attack1;
    public AudioSource attack2;
    public AudioSource collectGems;
    public AudioSource playerMove;
    public AudioSource slime;
    public AudioSource win;
    public AudioSource drag;
    public AudioSource drop;
    public AudioSource warning;
    public AudioSource clickButton;
    public AudioSource closeButton;
    public AudioSource clickPanelButton;
    public AudioSource disableButton;
    public AudioSource vectory;

    public Button mainVol;
    public Button musicVol;
    public Button sfxVol;
    public Button muteMainVol;
    public Button muteMusicVol;
    public Button muteSfxVol;
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    static float main;
    static float music;
    static float sfx;
    static float mainCache = 1;
    static float musicCache = 1;
    static float sfxCache = 1;

    public static bool warningListener = false;
    public static bool attackListener = false;
    public static int actionListener = 0;

    private void Start()
    {
        mainSlider.value = mainCache;
        musicSlider.value = musicCache;
        sfxSlider.value = sfxCache;

        if(mainSlider.value == 0.0001f) { mainVol.gameObject.SetActive(false); }//保持静音按钮跨关卡一致性
        if (musicSlider.value == 0.0001f) { musicVol.gameObject.SetActive(false); }
        if (sfxSlider.value == 00.0001f) { sfxVol.gameObject.SetActive(false); }

        
    }

    private void Update()
    {
        if (warningListener) { Warning(); }
        switch (actionListener)
        {
            case 1://警告
                Warning();
                actionListener = 0;
                break;
            case 2://攻击
                Attack1();
                actionListener = 0;
                break;
            case 3://收集
                CollectGems();
                actionListener = 0;
                break;
            case 4://移动
                PlayerMove();
                actionListener = 0;
                break;
            case 5://史莱姆
                Slime();
                actionListener = 0;
                break;
            case 6://胜利
                Win();
                actionListener = 0;
                break;
            case 7://拖拽
                Drag();
                actionListener = 0;
                break;
            case 8://放下
                Drop();
                actionListener = 0;
                break;
            case 9://三星
                Vectory();
                actionListener = 0;
                break;
        }
    }

    public void MainSlider(float f)//主音量调节
    {
        audioMixer.SetFloat("MainMixer", Mathf.Log10(f) * 20);
        mainCache = f;
        if (f == 0.0001f) { mainVol.gameObject.SetActive(false); }//滑块归零时自动开启静音按钮
        else { mainVol.gameObject.SetActive(true); }//滑块大于向右滑动时解除静音
    }

    public void BgmSlider(float f)//BGM音量调节
    {
        audioMixer.SetFloat("BGMMixer", Mathf.Log10(f) * 20);
        musicCache = f;
        if (f == 0.0001f) { musicVol.gameObject.SetActive(false); }
        else { musicVol.gameObject.SetActive(true); }
    }

    public void SfxSlider(float f)//SFX音量调节
    {
        audioMixer.SetFloat("SFXMixer", Mathf.Log10(f) * 20);
        sfxCache = f;
        if (f == 0.0001f) { sfxVol.gameObject.SetActive(false); }
        else { sfxVol.gameObject.SetActive(true); }
    }

    public void MuteButton()//一键静音
    {
        if (mainVol != null)
        {
            main = mainSlider.value;
            mainSlider.value = 0.0001f;
            mainVol.gameObject.SetActive(false);
        }
    }

    public void UnMuteButton()//取消静音
    {
        if(muteMainVol != null)
        {
            mainVol.gameObject.SetActive(true);
            mainSlider.value = main;
        }
    }

    public void BgmMuteButton()//BGM静音
    {
        if (musicVol != null)
        {
            music = musicSlider.value;
            musicSlider.value = 0.0001f;
            musicVol.gameObject.SetActive(false);
        }
    }

    public void UnBgmMuteButton()//取消BGM静音
    {
        if (muteMusicVol != null)
        {
            musicVol.gameObject.SetActive(true);
            musicSlider.value = music;
        }
    }

    public void SfxMuteButton()//SFX静音
    {
        if (sfxVol != null)
        {
            sfx = sfxSlider.value;
            sfxSlider.value = 0.0001f;
            sfxVol.gameObject.SetActive(false);
        }
    }

    public void UnSfxMuteButton()//取消SFX静音
    {
        if (muteSfxVol != null)
        {
            sfxVol.gameObject.SetActive(true);
            sfxSlider.value = sfx;
        }
    }

    public void Attack1() { attack1.Play(); }
    public void Attack2() { attack2.Play(); }
    public void CollectGems() { collectGems.Play(); }
    public void PlayerMove() { playerMove.Play(); }
    public void Slime() { slime.Play(); }
    public void Win() { win.Play(); }
    public void Drag() { drag.Play(); }
    public void Drop() { drop.Play(); }
    public void Warning() { warning.Play(); }
    public void Vectory() { vectory.Play(); }
    public void ClickButton() { clickButton.Play(); }
    public void CloseButton() { closeButton.Play(); }
    public void ClickPanelButton() { clickPanelButton.Play(); }
    public void DisableButton() { disableButton.Play(); }
}
