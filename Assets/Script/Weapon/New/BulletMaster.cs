using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMaster
{
    private float Power;
    public float power { get { return Power; } }

    public BulletMaster(float p) { Power = p;}

    public float MathSpeed(Vector3 position,Vector3 point)
    {
        return Vector3.Distance(position, point) >= 50 ? 0.3f : 0.5f;
    }
}
