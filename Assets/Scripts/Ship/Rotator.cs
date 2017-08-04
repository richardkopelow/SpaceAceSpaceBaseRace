using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : ShipComponent
{

    private Transform trans;
    private float rate;

    public Rotator() : base()
    {
        Job = JobsEnum.Rotator;
    }

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 rot = trans.eulerAngles;
        rot.z += rate * Time.deltaTime;
    }

    public override void RecieveFloat(float value)
    {
        rate = value;
    }
}
