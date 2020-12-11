using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Alert : MonoBehaviour
{
    public TextMeshProUGUI alertTitle;
    public TextMeshProUGUI message_txt;
    
    public Button ok_btn;
  

    private static Alert _instance;

    public static Alert Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Instance is null");

            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        
    }


    public void OkAlert(string title, string message = null, Action okAction = null)
    {
        gameObject.SetActive(true);

        if (message_txt)
            message_txt.text = message;
        if (alertTitle)
            alertTitle.text = title;

        if (ok_btn)
        {
            ok_btn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                okAction?.Invoke();
            });
        }
    }


}
