using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // สำหรับการใช้ Slider

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // ใช้ Singleton เพื่อเรียกใช้ได้จากทุกที่

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip placeTowerSound;
    public AudioClip enemyDeathSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip towerShootSound;
    public AudioClip sellTowerSound; // เสียงขาย Tower
    public AudioClip spawnEnemySound; // เสียงปล่อย Enem

    [Header("Volume Sliders")]
    public Slider musicSlider;  // Slider สำหรับปรับเสียงเพลงพื้นหลัง
    public Slider sfxSlider;    // Slider สำหรับปรับเสียงเอฟเฟกต์

    private void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ให้ SoundManager อยู่ตลอดเวลา
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();

        // โหลดค่า Volume จาก PlayerPrefs (ถ้ามี)
        backgroundMusicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // ตั้งค่า Slider ให้ตรงกับค่า Volume ปัจจุบัน
        if (musicSlider != null)
        {
            musicSlider.value = backgroundMusicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // ฟังก์ชันเล่นเพลงพื้นหลัง
    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    // ฟังก์ชันเล่นเสียงเอฟเฟกต์
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // ฟังก์ชันปรับระดับเสียงเพลงพื้นหลัง
    public void SetMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // บันทึกค่า
    }

    // ฟังก์ชันปรับระดับเสียงเอฟเฟกต์
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); // บันทึกค่า
    }

    // ฟังก์ชันเรียกใช้เสียงแต่ละประเภท
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }

    public void PlayPlaceTowerSound()
    {
        PlaySFX(placeTowerSound);
    }

    public void PlayEnemyDeathSound()
    {
        PlaySFX(enemyDeathSound);
    }

    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }

    public void PlayLoseSound()
    {
        PlaySFX(loseSound);
    }

    public void PlayTowerShootSound()
    {
        PlaySFX(towerShootSound);
    }

    public void PlaySellTowerSound() // ฟังก์ชันขาย Tower
    {
        PlaySFX(sellTowerSound); 
    }

    public void PlaySpawnEnemySound()// ฟังก์ชันปล่อย Enemy
    {
        PlaySFX(spawnEnemySound); 
    }
}


