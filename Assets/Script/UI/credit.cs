using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class credit : MonoBehaviour
{
    public GameObject Panel_credit;
    
    // Start is called before the first frame update
    void Start()
    {
        Panel_credit.SetActive(false);
    }

    public void OpencreditPanel()
    {
        Panel_credit.SetActive(true);
    }

    public void closecreditPanrl()
    {
        Panel_credit.SetActive(false);
    }
    
}
