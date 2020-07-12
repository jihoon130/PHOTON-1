using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIPopUp : MonoBehaviour
{
    public GameObject[] PopUpUi;
    // 890 1305
    private float vEndPosX = 1305f;
    private float vEndPosX2 = 1305f;

    void Start()
    {
    }

    void Update()
    {
        PopUpUi[0].transform.DOLocalMoveX(vEndPosX, 1).SetEase(Ease.Unset);
        PopUpUi[1].transform.DOLocalMoveX(vEndPosX2, 1).SetEase(Ease.Unset);
    }

    public void MoveXValue(float X) => vEndPosX = X;
    public void MoveXValue2(float X) => vEndPosX2 = X;

}
