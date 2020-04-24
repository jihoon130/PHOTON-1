using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddNickName : MonoBehaviour
{
    public InputField Nick;
    public GameObject Play;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public  void SetNickName()
    {
        PlayerPrefs.SetString("NickName", Nick.text);
        gameObject.SetActive(false);
        Play.SetActive(true);
    }
}
