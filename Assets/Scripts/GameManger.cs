using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManger : NetworkBehaviour
{
    private static GameManger _instance;
    public static GameManger Instance
    {
        get { return _instance; }
    }

    public GameObject ServerUI;
    public GameObject PlayerUI;
    public Text PlayerCountText;
    public Cinemachine.CinemachineVirtualCamera VirtualCamera;
    public Ship ShipPrefab;

    private List<PlayerScript> players;
    private List<PlayerScript> readyPlayers;
    private List<PlayerScript> blueTeam;
    private List<PlayerScript> redTeam;

    [ServerCallback]
    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        ServerUI.SetActive(true);
        PlayerUI.SetActive(false);
        players = new List<PlayerScript>();
        redTeam = new List<PlayerScript>();
        blueTeam = new List<PlayerScript>();
        readyPlayers = new List<PlayerScript>();
    }

    public void RegisterPlayer(PlayerScript player)
    {
        players.Add(player);
        PlayerCountText.text = "Players: " + players.Count;
        UpdatePlayerUIs();
    }

    public void Disassociate(PlayerScript player)
    {
        redTeam.Remove(player);
        blueTeam.Remove(player);
    }

    public void Associate(PlayerScript player, bool redTeam)
    {
        List<PlayerScript> team;
        List<PlayerScript> otherTeam;
        if (redTeam)
        {
            team = this.redTeam;
            otherTeam = this.blueTeam;
        }
        else
        {
            team = this.blueTeam;
            otherTeam = this.redTeam;
        }

        if (team.Count < ShipPrefab.ShipComponents.Length)
        {
            team.Add(player);
            otherTeam.Remove(player);
        }
    }

    public void ChangeComponent(PlayerScript player, int diff)
    {
        player.ComponentIndex += diff;
    }

    private void UpdatePlayerUI(PlayerScript player)
    {
        string data;
        if (player.Team == PlayerScript.TeamEnum.None)
        {
            data = "Disassociated,0";
        }
        else
        {
            string component = ShipPrefab.ShipComponents[player.ComponentIndex % ShipPrefab.ShipComponents.Length].ComponentName;

            List<PlayerScript> team = player.Team == PlayerScript.TeamEnum.Red ? redTeam : blueTeam;
            bool available = true;
            foreach (PlayerScript other in team)
            {
                if (other.Ready && other.ComponentIndex == player.ComponentIndex && other != player)
                {
                    available = false;
                }
            }
            data = string.Format("{0},{1}", component, available ? 1 : 0);
        }
        player.UpdatePlayerUI(data);
    }

    private void UpdatePlayerUIs()
    {
        foreach (PlayerScript player in players)
        {
            UpdatePlayerUI(player);
        }
    }

    public void SetPlayerReady(PlayerScript player, bool isReady)
    {
        if (isReady)
        {
            if (!readyPlayers.Contains(player))
            {
                readyPlayers.Add(player);
                bool good = true;
                foreach (PlayerScript p in players)
                {
                    if (!p.Ready)
                    {
                        good = false;
                    }
                }
                if (good)
                {
                    StartGame();
                }
            }
        }
        UpdatePlayerUIs();
    }

    public void StartGame()
    {
        ServerUI.SetActive(false);
        Ship blueShip = Instantiate<Ship>(ShipPrefab);
        Transform blueShipTransform = blueShip.GetComponent<Transform>();
        VirtualCamera.Follow = blueShipTransform;
        blueShipTransform.position = new Vector3(1, 0, 0);
        foreach (PlayerScript player in blueTeam)
        {
            player.ShipComponent = blueShip.ShipComponents[player.ComponentIndex % blueShip.ShipComponents.Length];
        }

        Ship redShip = Instantiate<Ship>(ShipPrefab);
        Transform redShipTransform = redShip.GetComponent<Transform>();
        //VirtualCamera.Follow = redShipTransform;
        redShipTransform.position = new Vector3(-1, 0, 0);
        foreach (PlayerScript player in redTeam)
        {
            player.ShipComponent = redShip.ShipComponents[player.ComponentIndex % redShip.ShipComponents.Length];
        }
    }
}
