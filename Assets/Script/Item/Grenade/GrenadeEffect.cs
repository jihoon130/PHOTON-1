using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrenadeEffect : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public GameObject Parent;

    private CapsuleCollider CapColl;

    private AudioSource Audio;
    public AudioClip[] Sounds;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        CapColl = GetComponent<CapsuleCollider>();
        Audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        int array = Random.Range(0, 2);
        Audio.clip = Sounds[array];
        Audio.Play();

        StartCoroutine("Destroy");
    }


    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        CapColl.enabled = false;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }
    }

}
