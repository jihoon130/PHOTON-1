using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartCredit : MonoBehaviour
{
    public GameObject[] fadeobj;
    int a = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("fade");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            fadeobj[0].GetComponent<CanvasGroup>().alpha = 0;
            fadeobj[1].GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().alpha = 0;
            this.gameObject.SetActive(false);
        }

        switch(a)
        {
            case 1:
                fadeobj[0].GetComponent<CanvasGroup>().alpha += Time.deltaTime;
                break;
            case 2:
                fadeobj[0].GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
                break;
            case 3:
                fadeobj[1].GetComponent<CanvasGroup>().alpha += Time.deltaTime;
                break;
            case 4:
                fadeobj[1].GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
                break;
            case 5:
                GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
                break;
        }
    }
   IEnumerator fade()
    {
        yield return new WaitForSeconds(1f);
        a = 1;
        yield return new WaitForSeconds(2f);
        a = 2;
        yield return new WaitForSeconds(2f);
        a = 3;
        yield return new WaitForSeconds(2f);
        a = 4;
        yield return new WaitForSeconds(2f);
        a = 5;
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }

}
