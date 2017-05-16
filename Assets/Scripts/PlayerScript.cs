using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    private ShipComponent _shipComponent;

    public ShipComponent ShipComonent
    {
        get { return _shipComponent; }
        set
        {
            _shipComponent = value;
            TargetAssignJob(connectionToClient, _shipComponent.Job);
        }
    }


    void Start()
    {
        if (isLocalPlayer)
        {

        }
        if (isServer)
        {
            GameManger.Instance.RegisterPlayer(this);
        }
    }

    [Command]
    public void CmdButtonDown()
    {
        _shipComponent.ButtonDown();
    }

    [Command]
    public void CmdButtonUp()
    {
        _shipComponent.ButtonUp();
    }
    
    [Command]
    public void CmdSendFloat(float value)
    {
        _shipComponent.RecieveFloat(value);
    }
    [TargetRpc]
    public void TargetAssignJob(NetworkConnection target, JobsEnum job)
    {
        Transform playerUIs = GameObject.Find("PlayerUI").GetComponent<Transform>();
        Transform uiTransform = null;
        foreach (Transform ui in playerUIs)
        {
            if (ui.name == job.ToString())
            {
                ui.gameObject.SetActive(true);
                uiTransform = ui;
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }
        switch (job)
        {
            case JobsEnum.Thruster:
                uiTransform.FindChild("ThrusterButton").GetComponent<ThrusterButton>().Player = this;
                break;
            default:
                break;
        }
    }
}
