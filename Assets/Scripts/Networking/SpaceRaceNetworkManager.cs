using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceRaceNetworkManager : NetworkManager
{
    public static SpaceRaceNetworkManager Instance
    {
        get { return (SpaceRaceNetworkManager)NetworkManager.singleton; }
    }
}
