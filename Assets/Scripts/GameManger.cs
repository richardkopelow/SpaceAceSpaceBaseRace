using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManger : NetworkBehaviour
{
    private static GameManger _instance;
    public static GameManger Instance
    {
        get { return _instance; }
    }

    public GameObject ServerUI;
    public GameObject PlayerUI;
    public GameObject Ship;

    private List<PlayerScript> players;

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
    }

    public void RegisterPlayer(PlayerScript player)
    {
        players.Add(player);
        if (players.Count>0)
        {
            ServerUI.SetActive(false);
            GameObject ship = Instantiate<GameObject>(Ship);
            player.ShipComonent = ship.GetComponent<Thruster>();
        }
    }
}
