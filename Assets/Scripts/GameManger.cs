using System.Collections;
using System.Collections.Generic;
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
                    if (!readyPlayers.Contains(p))
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
        else
        {
            readyPlayers.Remove(player);
        }
    }

    public void StartGame()
    {
        ServerUI.SetActive(false);
        Ship ship = Instantiate<Ship>(ShipPrefab);
        Transform shipTransform = ship.GetComponent<Transform>();
        VirtualCamera.Follow = shipTransform;
        for (int i = 0; i < blueTeam.Count; i++)
        {
            blueTeam[i].ShipComonent = ship.ShipComponents[i];
        }
    }
}
