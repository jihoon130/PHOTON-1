using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIPopUp : MonoBehaviour
{
    public GameObject[] PopUpUi;
    // 890 1305
    private float vEndPosX = 1305f;

    void Start()
    {
    }

    void Update()
    {
        PopUpUi[0].transform.DOLocalMoveX(vEndPosX, 1).SetEase(Ease.Unset);
    }

    public void MoveXValue(float X) => vEndPosX = X;

}
