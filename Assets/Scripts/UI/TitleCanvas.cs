using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour {

    GameObject creditText;
    GameObject startButton;
    GameObject creditButton;

    private void Start()
    {
        creditText = transform.Find("CreditText").gameObject;
        startButton = transform.Find("startBtn").gameObject;
        creditButton = transform.Find("creditBtn").gameObject;
        creditText.SetActive(false);
    }

    public void OnPressStart()
    {
        SceneManager.LoadScene(1);        
    }

    public void OnPressCredit()
    {
        creditText.SetActive(true);
        startButton.SetActive(false);
        creditButton.SetActive(false);
    }

    public void OnPressReturnToMain()
    {
        creditText.SetActive(false);
        startButton.SetActive(true);
        creditButton.SetActive(true);
    }
}
