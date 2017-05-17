using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobsEnum
{
    Thruster
}

public class ShipComponent : MonoBehaviour
{
    public JobsEnum Job;

    public virtual void ButtonDown()
    {

    }
    public virtual void ButtonUp()
    {

    }
    public virtual void RecieveFloat(float value)
    {

    }
}
