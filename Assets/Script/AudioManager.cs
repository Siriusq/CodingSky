using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

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

    public Button mainVol;
    public Button musicVol;
    public Button sfxVol;
    public Button muteMainVol;
    public Button muteMusicVol;
    public Button muteSfxVol;
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    float main;
    float music;
    float sfx;

    public void MainSlider(float f)//����������
    {
        audioMixer.SetFloat("MainMixer", Mathf.Log10(f) * 20);
    }

    public void BgmSlider(float f)//BGM��������
    {
        audioMixer.SetFloat("BGMMixer", Mathf.Log10(f) * 20);
    }

    public void SfxSlider(float f)//SFX��������
    {
        audioMixer.SetFloat("SFXMixer", Mathf.Log10(f) * 20);
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
    public void ClickButton() { clickButton.Play(); }
    public void CloseButton() { closeButton.Play(); }
    public void ClickPanelButton() { clickPanelButton.Play(); }
    public void DisableButton() { disableButton.Play(); }
}