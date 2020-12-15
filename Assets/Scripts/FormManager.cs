using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FormManager : MonoBehaviour
{

    public InputField newExpression;
    public InputField meaning;
    public Button save_btn;

    // Start is called before the first frame update
    void Start()
    {
        if (save_btn)
            save_btn.onClick.AddListener(() =>
            {
                if (newExpression & meaning)
                {
                    SaveNewExpression(newExpression.text, meaning.text);
                    newExpression.text = string.Empty;
                    meaning.text = string.Empty;
                }
            });
    }

    public void SaveNewExpression(string expression, string meaningOfExrpession)
    {

        if (!string.IsNullOrWhiteSpace(expression) && !string.IsNullOrWhiteSpace(meaningOfExrpession))
        {

            var ex = new Expression(newExpression.text, meaning.text, AuthManager.currentUser.UserId);
            DataBaseManager.sharedInstance.CreaNewExpression(ex);
        }
        else
        {
            Alert.Instance.OkAlert("Error", "Please you have to add and expression with its meaning.");
        }
    }
}
