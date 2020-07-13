using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Step3 : MonoBehaviour
{
    public bool a=false;
    public Vector3 pos;
   public PhotonView pv;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (a)
            return;

        string PlayerGetName = GameObject.Find("SelectPlayer").GetComponent<SelectPlayer>().CharacterName;

        Debug.Log(PlayerGetName);

        if (PlayerGetName == "Orange")
            PlayerGetName = "Green";

        
        if (pos != null)
        {
            //PhotonNetwork.Instantiate("Player_" + PlayerGetName, pos, Quaternion.identity);
            a = true;
        }

    }

}
