using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : ShipComponent
{
    public Rigidbody2D rigid;

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
            rigid.AddForceAtPosition(trans.up * 10, trans.position);
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
