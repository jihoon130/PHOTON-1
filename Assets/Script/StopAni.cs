using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAni : MonoBehaviour
{
    LobbyPlayer lb;
    // Start is called before the first frame update
    private void Awake()
    {
        lb = GetComponentInParent<LobbyPlayer>();
    }

    // Update is called once per frame
    public void Stop()
    {
        lb.StopAni();
    }
}
