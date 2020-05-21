using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{
    public GameObject Item1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemUIChange(bool checkd) => Item1.SetActive(checkd);
}
