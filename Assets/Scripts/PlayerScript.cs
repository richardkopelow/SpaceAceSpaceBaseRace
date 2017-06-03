using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour
{
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
            playerUIManager.SetReady(_ready);
            CmdSetReady(_ready);
        }
    }

    public TeamEnum Team { get; set; }

    public int ComponentIndex = 0;

    private PlayerUIManager playerUIManager;

    void Start()
    {
        Team = TeamEnum.None;
        if (isLocalPlayer)
        {
            playerUIManager = GameObject.Find("PlayerUI").GetComponent<PlayerUIManager>();
            playerUIManager.Player = this;

            playerUIManager.Picker.RedTeamButton.onClick.AddListener(redTeamClick);
            playerUIManager.Picker.DisassociateButton.onClick.AddListener(disassociateClick);
            playerUIManager.Picker.BlueTeamButton.onClick.AddListener(blueTeamClick);
            playerUIManager.Picker.ReadyButton.onClick.AddListener(readyClick);
            playerUIManager.Picker.PreviousComponentButton.onClick.AddListener(previousComponentClick);
            playerUIManager.Picker.NextComponentButton.onClick.AddListener(nextComponentClick);
        }
        if (isServer)
        {
            GameManger.Instance.RegisterPlayer(this);
        }
    }

    #region UI Handlers
    private void redTeamClick()
    {
        CmdAssociate(true);
        Ready = false;
        playerUIManager.SetTeam(TeamEnum.Red);
    }

    private void disassociateClick()
    {
        CmdDisassociate();
        Ready = false;
        playerUIManager.SetTeam(TeamEnum.None);
    }

    private void blueTeamClick()
    {
        CmdAssociate(false);
        Ready = false;
        playerUIManager.SetTeam(TeamEnum.Blue);
    }

    private void readyClick()
    {
        Ready = !_ready;
    }

    private void previousComponentClick()
    {
        CmdChangeComponent(false);
        Ready = false;
    }

    private void nextComponentClick()
    {
        CmdChangeComponent(true);
        Ready = false;
    }
    #endregion

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
        playerUIManager.SetUI(job);
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
        
        playerUIManager.Picker.ComponentTitle.text = parts[0];
        playerUIManager.Picker.ReadyButton.interactable = parts[1] == "1" ? true : false;
    }

    public void LockUI()
    {
        TargetLockUI(connectionToClient);
    }

    [TargetRpc]
    private void TargetLockUI(NetworkConnection target)
    {
        playerUIManager.LockUI();
    }
}