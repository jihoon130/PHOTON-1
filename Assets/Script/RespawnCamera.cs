using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCamera : MonoBehaviour
{
    Move CharaterMove;
    Camera ca;
    bool OKCursor=false;
    public Texture2D[] cursor;
    // Start is called before the first frame update
    void Start()
    {
        ca = GetComponent<Camera>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject game in gameObjects)
        {
            if(game.GetComponent<Move>().PV.IsMine)
            {
                CharaterMove = game.GetComponent<Move>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = ca.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                if (!OKCursor)
                {
                    Cursor.SetCursor(cursor[0], new Vector2(cursor[0].width /4, cursor[0].height / 4), CursorMode.Auto);
                    OKCursor = true;
                }
                if (CharaterMove && Input.GetMouseButtonDown(0))
                {
                    CharaterMove.repo = hit.point;
                    CharaterMove.ResetPos();
                }
            }
            else
            {
                if (OKCursor)
                {
                    Cursor.SetCursor(cursor[1], new Vector2(cursor[1].width / 4, cursor[1].height / 4), CursorMode.Auto);
                    OKCursor = false;
                }
            }

        }

    }
}
