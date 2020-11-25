using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour
{
    public GameObject image;
    public TextMeshProUGUI text;
    public GameObject moogi;
    public GameObject Player;
    int a, b, c,d = 0;
    private void Awake()
    {
        InvokeRepeating("tutocheck", 1f, 0.1f);
    }

    void tutocheck()
    {
        if (GameObject.Find("tutoImage"))
        {
            image = GameObject.Find("tutoImage");
            image.transform.GetChild(0).gameObject.SetActive(true);
            image.transform.GetChild(1).gameObject.SetActive(true);
            text = GameObject.Find("tutoText").GetComponent<TextMeshProUGUI>();
            Player = GameObject.FindGameObjectWithTag("Player");
            InvokeRepeating("tutorial", 0.1f, 0.04f);
            CancelInvoke("tutocheck");
        }
    }

    void tutocheck1()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            image.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            image.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            image.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            image.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }


        if (image.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.activeInHierarchy &&
            image.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.activeInHierarchy &&
            image.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.activeInHierarchy &&
            image.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            image.transform.GetChild(0).gameObject.SetActive(true);
            image.transform.GetChild(1).gameObject.SetActive(true);
            image.transform.GetChild(2).gameObject.SetActive(false);
            InvokeRepeating("tutorial1",0.1f,0.04f);
            CancelInvoke("tutocheck1");
        }
    }

    void tutorial()
    {
        Player.GetComponent<Move>().tutomove = false;
        if (Input.GetKeyDown(KeyCode.Space)) a++;
        if (a == 0)
        {
            text.text = "안녕! 원티드에 어서와. 너도 보물을 찾으러 온거지?";
        }
        else if (a == 1)
        {
            text.text = "보물을 탐내는 녀석들을 모두 물에 빠트려야 보물을 쟁취할 수 있어.";
        }
        else if (a == 2)
        {
            text.text = "조작키 wasd를 누르면 움직일수 있어 한번 움직여 볼래?";
           
        }
        else if (a >= 3)
        {
            image.transform.GetChild(0).gameObject.SetActive(false);
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(2).gameObject.SetActive(true);
            Player.GetComponent<Move>().tutomove = true;
            InvokeRepeating("tutocheck1", 1f, 0.01f);
            CancelInvoke("tutorial");
        }
    }

    void tutorial1()
    {
        if (Input.GetKeyDown(KeyCode.Space)) b++;
        if (b == 0)
        {
            text.text = "잘했어! 돌아다니다보면 적을 만날거야.";
        }
        else if (b == 1)
        {
            text.text = "적의 공격을 피하려면 구르는게 최고지. Shift로 굴러봐!";
        }
        else if (b >= 2)
        {
            image.transform.GetChild(0).gameObject.SetActive(false);
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(3).gameObject.SetActive(true);
            CancelInvoke("tutorial1");
            InvokeRepeating("tutocheck2", 0.1f, 0.01f);
        }
    }

    public void tutocheck2()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            image.transform.GetChild(0).gameObject.SetActive(true);
            image.transform.GetChild(1).gameObject.SetActive(true);
            image.transform.GetChild(3).gameObject.SetActive(false);
            CancelInvoke("tutocheck2");
            InvokeRepeating("tutorial2", 0.1f, 0.04f);
        }
    }

    void tutorial2()
    {
        if (Input.GetKeyDown(KeyCode.Space)) c++;
        if (c == 0)
        {
            text.text = "잘하는데? 보물을 얻는건 식은 죽 먹기겠어.";
        }
        else if (c == 1)
        {
            text.text = "이제 적을 날려봐야지. M1 (마우스 왼클릭) 버튼을 눌러서 펀치를 날려봐!";
        }
        else if (c >= 2)
        {
            image.transform.GetChild(0).gameObject.SetActive(false);
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(4).gameObject.SetActive(true);
            CancelInvoke("tutorial2");
            InvokeRepeating("tutocheck3", 0.1f, 0.01f);
        }
    }

    void tutocheck3()
    {
        if (Input.GetMouseButtonDown(0))
            image.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(true);

        if(Input.GetMouseButton(1)&&Input.GetMouseButtonDown(0))
            image.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);

        if (image.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.activeInHierarchy&&
            image.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.activeInHierarchy )
        {
            image.transform.GetChild(0).gameObject.SetActive(true);
            image.transform.GetChild(1).gameObject.SetActive(true);
            image.transform.GetChild(4).gameObject.SetActive(false);
            InvokeRepeating("tutorial3", 0.1f, 0.04f);
            CancelInvoke("tutocheck3");
        }
    }

    void tutorial3()
    {
        if (Input.GetKeyDown(KeyCode.Space)) d++;
        if (d == 0)
        {
            text.text = "자, 이제 마지막이야. 배 위에서 시간을 보내다 보면, 강력한 아이템이 등장해.";
        }
        else if (d == 1)
        {
            text.text = "맵 중앙에 떨어진 아이템을 먹어봐!";
        }
        else if (d >= 2)
        {
            image.transform.GetChild(0).gameObject.SetActive(false);
            image.transform.GetChild(1).gameObject.SetActive(false);
            image.transform.GetChild(5).gameObject.SetActive(true);
            Instantiate(moogi, new Vector3(-2.02f, 8.785f, -10.72f), Quaternion.identity);
            CancelInvoke("tutorial3");
            InvokeRepeating("tutocheck4", 0.1f, 0.01f);
        }
    }

    void tutocheck4()
    {
        if (GameObject.Find("Item1"))
            image.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(true);

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Create>()._BulletMake == BulletMake.Machinegun)
            image.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Create>()._BulletMake == BulletMake.Machinegun && Input.GetMouseButtonDown(0))
            image.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);

        if (image.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.activeInHierarchy&&
            image.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.activeInHierarchy&&
            image.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            image.transform.GetChild(0).gameObject.SetActive(true);
            image.transform.GetChild(1).gameObject.SetActive(true);
            image.transform.GetChild(5).gameObject.SetActive(false);
            CancelInvoke("tutocheck4");
            StartCoroutine("tutorial4");
        }
    }

    IEnumerator tutorial4()
    {
        text.text = "좋았어! 이제 보물을 얻으러 갈 시간이야. 잘 해봐!";
        yield return new WaitForSeconds(5f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Photon.Pun.PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
        Destroy(this.gameObject);
    }
}
