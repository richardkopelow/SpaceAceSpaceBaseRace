using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
            Transform playerUIs = GameObject.Find("PlayerUI").GetComponent<Transform>();
            Transform componentPicker = playerUIs.GetChild(0);
            Button redTeamButton = componentPicker.FindChild("RedTeamButton").GetComponent<Button>();
            redTeamButton.onClick.AddListener(() => CmdAssociate(true));
            Button disassociateButton = componentPicker.FindChild("DisassociateButton").GetComponent<Button>();
            disassociateButton.onClick.AddListener(() => CmdDisassociate());
            Button blueTeamButton = componentPicker.FindChild("BlueTeamButton").GetComponent<Button>();
            blueTeamButton.onClick.AddListener(() => CmdAssociate(false));
            Button readyButton = componentPicker.FindChild("ReadyButton").GetComponent<Button>();
            readyButton.onClick.AddListener(() => CmdSetReady(true));
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

    [Command]
    public void CmdSetReady(bool isReady)
    {
        GameManger.Instance.SetPlayerReady(this, isReady);
    }

    [Command]
    private void CmdAssociate(bool redTeam)
    {
        GameManger.Instance.Associate(this, redTeam);
    }

    [Command]
    private void CmdDisassociate()
    {
        GameManger.Instance.Disassociate(this);
    }
}
