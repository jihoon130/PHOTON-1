using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{

    public string CharacterName;


    private void Awake()
    {
        CharacterName = "Blue";
        DontDestroyOnLoad(gameObject);
    }
}
