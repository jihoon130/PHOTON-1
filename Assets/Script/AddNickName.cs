using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AddNickName : MonoBehaviour
{
    public InputField Nick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNickName()
    {
        if (Nick.text == "")
        {
            PlayerPrefs.SetString("NickName", "무한지훈교도");
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
