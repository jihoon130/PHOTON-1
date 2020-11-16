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
            StartCoroutine("tutorial");
            CancelInvoke("tutocheck");
        }
    }

    void tutocheck1()
    {
        if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine("tutorial1");
            CancelInvoke("tutocheck1");
        }
    }

    IEnumerator tutorial()
    {
        Player.GetComponent<Move>().tutomove = false;
        text.text = "안녕 내 이름은 김선우 탐정이야 나는 너에게 조작법을 설명하려고해.";
        yield return new WaitForSeconds(6f);
        text.text = "조작키 wasd를 누르면 움직일수 있어 한번 움직여 볼래?";
        Player.GetComponent<Move>().tutomove = true;
        InvokeRepeating("tutocheck1", 1f, 0.1f);
    }

    IEnumerator tutorial1()
    {
        yield return new WaitForSeconds(4f);
        text.text = "좋아! 잘했어 그다음으로 넘어가볼까?";
        yield return new WaitForSeconds(3f);
        text.text = "마우스 왼쪽키를 누르면 공격을 할 수 있어 맵에 소환된 표적을 찾아 공격을 해보자.";
        GameObject.Find("target").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void tutocheck2()
    {
        StartCoroutine("tutorial2");
    }

    IEnumerator tutorial2()
    {
        yield return new WaitForSeconds(4f);
        text.text = "좋아! 잘했어 그다음으로 넘어가볼까?";
        GameObject.Find("target").transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        text.text = "이번엔 아이템을 장착해보려고 해 맵 중앙에 아이템을 스폰해두었어! 가까이 가면 먹을수있는데 이 후 마우스 휠을 내려볼까?";
        Instantiate(moogi, new Vector3(-2.02f, 8.785f, -10.72f), Quaternion.identity);
        InvokeRepeating("tutocheck3", 0.1f, 0.1f);
    }

    void tutocheck3()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Create>()._BulletMake == BulletMake.Machinegun)
        {
            CancelInvoke("tutocheck3");
            StartCoroutine("tutorial3");
        }
    }

    IEnumerator tutorial3()
    {
        yield return new WaitForSeconds(4f);
        text.text = "좋아! 잘했어 그다음으로 넘어가볼까?";
        yield return new WaitForSeconds(3f);
        text.text = "이번엔 구르기를 해보려고해 구르는 상태에서는 상대방의 공격을 회피할수 있어 왼쪽 shift버튼을 누르면 구르기를 할수 있단다. 한번 해볼까?";
        InvokeRepeating("tutocheck4", 0.1f, 0.1f);
    }

    void tutocheck4()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            CancelInvoke("tutocheck4");
            StartCoroutine("tutorial4");
        }
    }

    IEnumerator tutorial4()
    {
        yield return new WaitForSeconds(4f);
        text.text = "좋아! 잘했어 이제 게임을 즐길 수 있을거같아 수고했어";
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Photon.Pun.PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
    }
}
