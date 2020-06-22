using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLog : MonoBehaviour
{
    bool a=false;
   public float deleteT = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(deleteT>0.0f)
        {
            deleteT -= Time.deltaTime; 
            a = false;
        }
        else
        {
            a = true;
        }

        if(!a && transform.position.x <= 180f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 300f);
        }
        else if(a && transform.position.x >= -190f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 300f);
        }
    }

    public void ResetT()
    {
        transform.position = new Vector3(-190f, transform.position.y, transform.position.z);
    }
}
