using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class BackMove : MonoBehaviourPunCallbacks
{
    private Move _Move;
    private PlayerAni _PlayerAni;

    public PhotonView PV;
    public Rigidbody rb;
    public AudioSource Audio;
    public AudioClip[] audioS;
    public Renderer[] rd;
    float b = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        Audio = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        _Move = GetComponentInParent<Move>();
        _PlayerAni = GetComponentInParent<PlayerAni>();
    }

    private void Update()
    {
        if (b >= 0.1f)
        {
            b -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pok"))
        {
            ObjMoveback5(other, 5000f);
        }
    }
    [PunRPC]
    public void ObjMoveback4RPC(float a, float b, float c, float d)
    {
        _PlayerAni._State = State.Dmg;
        Vector3 pushdi = new Vector3(a, b, c) - transform.position;
        pushdi = -pushdi.normalized;
        pushdi.y = 0f;
        rb.AddForce(pushdi * d, ForceMode.Impulse);
    }


    public void ObjMoveback5(Collider collision, float speed = 1000.0f)
    {
        int a;
        a = Random.Range(0, 2);
        Audio.clip = audioS[a];
        Audio.Play();
        rb.velocity = Vector3.zero;
        _PlayerAni._State = State.Dmg;

        //if(PV.Owner.ToString() != collision.GetComponent<GrenadeEffect>().PV.Owner.ToString())
        //_Move.Piguck2 = collision.GetComponent<GrenadeEffect>().PV.Owner.ToString();

        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    public void ObjMoveback6(GameObject collision, float speed = 1000.0f)
    {
        int a;
        a = Random.Range(0, 2);
        Audio.clip = audioS[a];
        Audio.Play();
        rb.velocity = Vector3.zero;
        _PlayerAni._State = State.Dmg;

        //if(PV.Owner.ToString() != collision.GetComponent<GrenadeEffect>().PV.Owner.ToString())
        //_Move.Piguck2 = collision.GetComponent<GrenadeEffect>().PV.Owner.ToString();

        rb.AddForce(collision.transform.forward * speed, ForceMode.Impulse);
    }

    [PunRPC]
    public void BackRPC(float a, float b, float c,float d, string e)
    {
        if (_Move.isPhoenix || GetComponentInParent<Machinegun>().isMachineRay)
        {
            return;
        }
        if (_Move._PlayerAni._State == State.Dash)
            return;


        int f;
        f = Random.Range(0, 2);
        Audio.clip = audioS[f];
        Audio.Play();

        rb.velocity = Vector3.zero;
        _PlayerAni._State = State.Dmg;

        if(PV.Owner.ToString() != e)
        _Move.Piguck2 = e;

        Vector3 pushdi = new Vector3(a, b, c) - transform.position;
        pushdi = -pushdi.normalized;
        pushdi.y = 0f;
        rb.AddForce(pushdi * d, ForceMode.Impulse);
    }

    public void hitOn()
    {
        for(int i = 0; i < rd.Length; i++)
            rd[i].material.SetFloat("_Damaged", 1.0f);
    }
    public void hitOff()
    {
        for (int i = 0; i < rd.Length; i++)
            rd[i].material.SetFloat("_Damaged", 0.0f);
    }

    public void RespawnUpdate(float f)
    {
        for (int i = 0; i < rd.Length; i++)
            rd[i].material.SetFloat("_Respawn", f);
    }
}