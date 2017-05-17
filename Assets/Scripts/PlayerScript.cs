using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour
{
    public enum TeamEnum
    {
        Red,
        Blue,
        None
    }

    private ShipComponent _shipComponent;
    public ShipComponent ShipComponent
    {
        get { return _shipComponent; }
        set
        {
            _shipComponent = value;
            TargetAssignJob(connectionToClient, _shipComponent.Job);
        }
    }

    private bool _ready;
    public bool Ready
    {
        get { return _ready; }
        set
        {
            _ready = value;
            readyButtonText.text = _ready ? "Unready" : "Ready";
            CmdSetReady(_ready);
        }
    }

    public TeamEnum Team { get; set; }

    public int ComponentIndex = 0;

    #region GUI
    Button readyButton;
    Text readyButtonText;
    Text componentTitle;
    #endregion

    void Start()
    {
        Team = TeamEnum.None;
        if (isLocalPlayer)
        {
            Transform playerUIs = GameObject.Find("PlayerUI").GetComponent<Transform>();
            Transform componentPicker = playerUIs.GetChild(0);
            Button redTeamButton = componentPicker.FindChild("RedTeamButton").GetComponent<Button>();
            redTeamButton.onClick.AddListener(() => {
                CmdAssociate(true);
                Ready = false;
            });
            Button disassociateButton = componentPicker.FindChild("DisassociateButton").GetComponent<Button>();
            disassociateButton.onClick.AddListener(() => {
                CmdDisassociate();
                Ready = false;
            });
            Button blueTeamButton = componentPicker.FindChild("BlueTeamButton").GetComponent<Button>();
            blueTeamButton.onClick.AddListener(() => {
                CmdAssociate(false);
                Ready = false;
            });
            readyButton = componentPicker.FindChild("ReadyButton").GetComponent<Button>();
            readyButtonText = readyButton.GetComponent<Transform>().GetChild(0).GetComponent<Text>();
            componentTitle = componentPicker.FindChild("ComponentTitle").GetComponent<Text>();
            readyButton.onClick.AddListener(() =>
            {
                Ready = !_ready;
            });
            Button previousComponentButton = componentPicker.FindChild("PreviousComponentButton").GetComponent<Button>();
            previousComponentButton.onClick.AddListener(() => {
                CmdChangeComponent(false);
                Ready = false;
            });
            Button nextComponentButton = componentPicker.FindChild("NextComponentButton").GetComponent<Button>();
            nextComponentButton.onClick.AddListener(() => {
                CmdChangeComponent(true);
                Ready = false;
            });
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
    private void CmdSetReady(bool isReady)
    {
        _ready = isReady;
        GameManger.Instance.SetPlayerReady(this, isReady);
    }

    [Command]
    private void CmdAssociate(bool redTeam)
    {
        Team = redTeam ? TeamEnum.Red : TeamEnum.Blue;
        GameManger.Instance.Associate(this, redTeam);
    }

    [Command]
    private void CmdDisassociate()
    {
        Team = TeamEnum.None;
        GameManger.Instance.Disassociate(this);
    }

    [Command]
    private void CmdChangeComponent(bool up)
    {
        GameManger.Instance.ChangeComponent(this, up?1:-1);
    }

    public void UpdatePlayerUI(string data)
    {
        TargetUpdatePlayerUI(connectionToClient, data);
    }

    [TargetRpc]
    private void TargetUpdatePlayerUI(NetworkConnection target, string data)
    {
        string[] parts = data.Split(',');
        
        componentTitle.text = parts[0];
        readyButton.interactable = parts[1] == "1" ? true : false;
    }
}