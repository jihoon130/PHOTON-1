using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
  public  SkinnedMeshRenderer skin;
    MeshCollider mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh1 = new Mesh();
        skin.BakeMesh(mesh1);
        mesh.sharedMesh = mesh1;
    }
}
