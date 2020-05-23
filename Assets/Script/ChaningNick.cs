using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ChaningNick : MonoBehaviour
{
    public GameObject NickC1;
    public InputField Nick2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NickC1.transform.SetAsLastSibling();
    }


    public void ExitCh()
    {
        NickC1.SetActive(false);
    }

   public void ChangeNick()
    {
        if (Nick2.text.Length > 6)
            return;

        PlayerPrefs.SetString("NickName", Nick2.text);
        PhotonNetwork.NickName = Nick2.text;
        NickC1.SetActive(false);
    }
}
