using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Step2 : MonoBehaviour
{
   public float time;
    private void OnEnable()
    {
        time = 4.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time>=0.0f)
        {
            time -= Time.deltaTime;
        }

        GetComponent<Text>().text = ((int)time).ToString();
    }
}
