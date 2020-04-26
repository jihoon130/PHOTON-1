using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EastEgg : MonoBehaviour
{
    CanvasGroup cg;
    // Start is called before the first frame update
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cg.alpha>=0.0f)
        {
            cg.alpha -= Time.deltaTime/8;
        }
    }

}
