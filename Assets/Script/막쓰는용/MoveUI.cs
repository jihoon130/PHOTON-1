using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveUI : MonoBehaviour
{
    public GameObject Credit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Credit.transform.Translate(Vector3.up * Time.deltaTime*150f);
    }
    public void Credit_ResetPos()
    {
        Credit.transform.localPosition = new Vector3(0, -1432f, 0);
    }
}
