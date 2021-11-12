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

        if(mainSlider.value == 0.0001f) { mainVol.gameObject.SetActive(false); }//���־�����ť��ؿ�һ����
        if (musicSlider.value == 0.0001f) { musicVol.gameObject.SetActive(false); }
        if (sfxSlider.value == 00.0001f) { sfxVol.gameObject.SetActive(false); }

        
    }

    private void Update()
    {
        if (warningListener) { Warning(); }
        switch (actionListener)
        {
            case 1://����
                Warning();
                actionListener = 0;
                break;
            case 2://����
                Attack1();
                actionListener = 0;
                break;
            case 3://�ռ�
                CollectGems();
                actionListener = 0;
                break;
            case 4://�ƶ�
                PlayerMove();
                actionListener = 0;
                break;
            case 5://ʷ��ķ
                Slime();
                actionListener = 0;
                break;
            case 6://ʤ��
                Win();
                actionListener = 0;
                break;
            case 7://��ק
                Drag();
                actionListener = 0;
                break;
            case 8://����
                Drop();
                actionListener = 0;
                break;
            case 9://����
                Vectory();
                actionListener = 0;
                break;
        }
    }

    public void MainSlider(float f)//����������
    {
        audioMixer.SetFloat("MainMixer", Mathf.Log10(f) * 20);
        mainCache = f;
        if (f == 0.0001f) { mainVol.gameObject.SetActive(false); }//�������ʱ�Զ�����������ť
        else { mainVol.gameObject.SetActive(true); }//����������һ���ʱ�������
    }

    public void BgmSlider(float f)//BGM��������
    {
        audioMixer.SetFloat("BGMMixer", Mathf.Log10(f) * 20);
        musicCache = f;
        if (f == 0.0001f) { musicVol.gameObject.SetActive(false); }
        else { musicVol.gameObject.SetActive(true); }
    }

    public void SfxSlider(float f)//SFX��������
    {
        audioMixer.SetFloat("SFXMixer", Mathf.Log10(f) * 20);
        sfxCache = f;
        if (f == 0.0001f) { sfxVol.gameObject.SetActive(false); }
        else { sfxVol.gameObject.SetActive(true); }
    }

    public void MuteButton()//һ������
    {
        if (mainVol != null)
        {
            main = mainSlider.value;
            mainSlider.value = 0.0001f;
            mainVol.gameObject.SetActive(false);
        }
    }

    public void UnMuteButton()//ȡ������
    {
        if(muteMainVol != null)
        {
            mainVol.gameObject.SetActive(true);
            mainSlider.value = main;
        }
    }

    public void BgmMuteButton()//BGM����
    {
        if (musicVol != null)
        {
            music = musicSlider.value;
            musicSlider.value = 0.0001f;
            musicVol.gameObject.SetActive(false);
        }
    }

    public void UnBgmMuteButton()//ȡ��BGM����
    {
        if (muteMusicVol != null)
        {
            musicVol.gameObject.SetActive(true);
            musicSlider.value = music;
        }
    }

    public void SfxMuteButton()//SFX����
    {
        if (sfxVol != null)
        {
            sfx = sfxSlider.value;
            sfxSlider.value = 0.0001f;
            sfxVol.gameObject.SetActive(false);
        }
    }

    public void UnSfxMuteButton()//ȡ��SFX����
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
