using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AddNickName : MonoBehaviour
{
    public InputField Nick;
    public void SetNickName()
    {
        if (Nick.text == "")
        {
            PlayerPrefs.SetString("NickName", "카우카우짱짱");
        }
        else
        {
            if (Nick.text.Length > 6)
                return;

            PlayerPrefs.SetString("NickName", Nick.text);
        }
        SceneManager.LoadScene(1);
    }
}
