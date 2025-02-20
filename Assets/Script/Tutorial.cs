using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject Canvas_TowerIcon;
    public GameObject Canvas_Money;
    public GameObject Canvas_Startgame;
    public GameObject Canva_Status;
    
    public GameObject Canvas_Tutorial;
    public GameObject Panel_1;
    public GameObject Panel_2;
    public GameObject Panel_3;
    
    // Start is called before the first frame update
    void Start()
    {
        Canvas_Tutorial.SetActive(true);
        Canvas_TowerIcon.SetActive(false);
        Canvas_Startgame.SetActive(false);
        Canvas_Money.SetActive(false);
        Canva_Status.SetActive(false);
    }

    public void OpenPanel2()
    {
        Panel_1.SetActive(false);
        Panel_2.SetActive(true);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound();
        }
    }

    public void OpenPanel3()
    {
        Panel_2.SetActive(false);
        Panel_3.SetActive(true);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound();
        }
    }

    public void closePanel()
    {
        Panel_3.SetActive(false);
        Canvas_Tutorial.SetActive(false);
        Canvas_TowerIcon.SetActive(true);
        Canvas_Startgame.SetActive(true);
        Canvas_Money.SetActive(true);
        Canva_Status.SetActive(true);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound();
        }
    }

}
