﻿using System.Collections;
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
    public Text WinnerText;
    public Cinemachine.CinemachineVirtualCamera BlueVirtualCamera;
    public Cinemachine.CinemachineVirtualCamera RedVirtualCamera;
    public Transform Level;
    public Ship ShipPrefab;
    public Transform ArrowPrefab;

    private LevelGenerator levelGenerator;

    private List<PlayerScript> players;
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

        levelGenerator = GetComponent<LevelGenerator>();
    }

    public void RegisterPlayer(PlayerScript player)
    {
        players.Add(player);
        PlayerCountText.text = "Players: " + players.Count;
        StartCoroutine(delayedUpdatePlayerUIs(0.2f));
    }

    public void DeregisterPlayer(PlayerScript player)
    {
        players.Remove(player);
        PlayerCountText.text = "Players: " + players.Count;
        Disassociate(player);
        StartCoroutine(delayedUpdatePlayerUIs(0.2f));
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

    private void updatePlayerUI(PlayerScript player)
    {
        string data;
        if (player.Team == TeamEnum.None)
        {
            data = "Disassociated,0";
        }
        else
        {
            if (player.ComponentIndex < 0)
            {
                player.ComponentIndex = ShipPrefab.ShipComponents.Length + player.ComponentIndex;
            }
            player.ComponentIndex = player.ComponentIndex % ShipPrefab.ShipComponents.Length;

            string component = ShipPrefab.ShipComponents[player.ComponentIndex].ComponentName;

            List<PlayerScript> team = player.Team == TeamEnum.Red ? redTeam : blueTeam;
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

    private void updatePlayerUIs()
    {
        foreach (PlayerScript player in players)
        {
            updatePlayerUI(player);
        }
    }

    private IEnumerator delayedUpdatePlayerUIs(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        updatePlayerUIs();
    }

    public void SetPlayerReady(PlayerScript player, bool isReady)
    {
        if (isReady)
        {
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
        updatePlayerUIs();
    }

    public void StartGame()
    {
        ServerUI.SetActive(false);

        levelGenerator.Generate();
        #region Player Init
        //Spawn Ships
        Ship blueShip = spawnShip(TeamEnum.Blue);
        foreach (PlayerScript player in blueTeam)
        {
            player.ShipComponent = blueShip.ShipComponents[player.ComponentIndex % blueShip.ShipComponents.Length];
        }

        Ship redShip = spawnShip(TeamEnum.Red);
        foreach (PlayerScript player in redTeam)
        {
            player.ShipComponent = redShip.ShipComponents[player.ComponentIndex % redShip.ShipComponents.Length];
        }
        #endregion
    }

    private Ship spawnShip(TeamEnum team)
    {
        Ship ship = Instantiate<Ship>(ShipPrefab, Level);
        ship.Team = team;
        Transform shipTransform = ship.GetComponent<Transform>();
        float xOffset = 1;
        if (team == TeamEnum.Blue)
        {
            BlueVirtualCamera.Follow = shipTransform;
        }
        else
        {
            xOffset = -1;
            RedVirtualCamera.Follow = shipTransform;
        }
        shipTransform.position = new Vector3(xOffset, 0, 0);
        Transform arrow = Instantiate<Transform>(ArrowPrefab, shipTransform);
        arrow.position = new Vector3(0, -0.5f, -5);
        int layer = LayerMask.NameToLayer(team == TeamEnum.Blue ? "BlueCamera" : "RedCamera");
        arrow.gameObject.layer = layer;

        return ship;
    }

    public Coroutine EndGame(TeamEnum winningTeam)
    {
        return StartCoroutine(endGameCoroutine(winningTeam));
    }

    private IEnumerator endGameCoroutine(TeamEnum winningTeam)
    {
        foreach (PlayerScript player in players)
        {
            player.LockUI();
        }
        WinnerText.text = winningTeam == TeamEnum.Blue ? "The winner is <color=Blue>BLUE</color>" : "The winner is <color=Red>RED</color>";
        yield return new WaitForSeconds(5);
        WinnerText.text = "";
        foreach (PlayerScript player in players)
        {
            player.ShipComponent = null;
        }
        ServerUI.SetActive(true);
        foreach (Transform child in Level)
        {
            Destroy(child.gameObject);
        }
    }
}
