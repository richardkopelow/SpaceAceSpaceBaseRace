using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : ShipComponent
{
    public Rigidbody2D rigid;
    public float Force = 5;

    Transform trans;

    private bool down;

    public Thruster() : base()
    {
        Job = JobsEnum.Thruster;
    }

    public void Start()
    {
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (down)
        {
            rigid.AddForceAtPosition(trans.up * Force, trans.position);
        }
    }

    public override void ButtonDown()
    {
        down = true;
    }

    public override void ButtonUp()
    {
        down = false;
    }
}
