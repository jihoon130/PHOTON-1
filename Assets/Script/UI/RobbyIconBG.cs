using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobbyIconBG : MonoBehaviour
{
    public GameObject[] PointBg;
    void Start()
    {
        for (int i = 0; i < PointBg.Length; i++)
            PointBg[i].SetActive(false);
    }

    public void PointDonw(int type) => PointBg[type].SetActive(true);
    public void PointUp(int type) => PointBg[type].SetActive(false);
}
