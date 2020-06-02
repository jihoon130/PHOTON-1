using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scollviewscript : MonoBehaviour
{
    ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public  void SetContentSize()
    {
        float width = 100.0f;
        float height = 100.0f;
        scrollRect.content.sizeDelta = new Vector2(width, height);
    }
}
