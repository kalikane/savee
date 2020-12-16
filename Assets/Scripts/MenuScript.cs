using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public GameObject addPanel;
    public GameObject listExpressionPanel;
    public GameObject profilPanel;

    public Button add_btn;
    public Button list_btn;
    public Button profil_btn;
    public DataBaseManager baseManager;


    // Start is called before the first frame update
    void Start()
    {
        //baseManager.Awake();
        if (add_btn)
        {
            add_btn.onClick.AddListener(() =>
            {
                if (addPanel)
                    addPanel.SetActive(true);
                if (listExpressionPanel)
                    listExpressionPanel.SetActive(false);
                if (profilPanel)
                    profilPanel.SetActive(false);

                if (ExpressionBoardManager.instance)
                {
                    ExpressionBoardManager.instance.RemoveExpressionPrefab();
                    //ExpressionBoardManager.instance.expressionsList.Clear();
                }
            });
        }

        if (list_btn)
        {
            list_btn.onClick.AddListener(() =>
            {
                if (ExpressionBoardManager.instance)
                {
                    ExpressionBoardManager.instance.RemoveExpressionPrefab();
                    //ExpressionBoardManager.instance.expressionsList.Clear();
                }

                if (addPanel)
                    addPanel.SetActive(false);
                if (profilPanel)
                    profilPanel.SetActive(false);


                Debug.Log("Ajout = " + Router.childAdded);
                //await DataBaseManager.sharedInstance.GetExpressions();
                ExpressionBoardManager.instance.OpenListExpressionPanel();
            });
        }

        if (profil_btn)
        {
            profil_btn.onClick.AddListener(() =>
            {
                if (ExpressionBoardManager.instance)
                {
                    ExpressionBoardManager.instance.RemoveExpressionPrefab();
                    //ExpressionBoardManager.instance.expressionsList.Clear();

                }
                if (profilPanel)
                    profilPanel.SetActive(true);
                if (listExpressionPanel)
                    listExpressionPanel.SetActive(false);
                if (addPanel)
                    addPanel.SetActive(false);

                Debug.Log("Load List Pannel");
                //await DataBaseManager.sharedInstance.GetExpressions();
                ExpressionBoardManager.instance.OpenListExpressionPanel();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
