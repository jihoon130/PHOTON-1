using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
  public  PlayerDB pd;
    private void OnEnable()
    {
        pd.Ranking();
    }
}
