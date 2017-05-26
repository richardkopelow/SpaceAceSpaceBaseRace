using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public ComponentPicker Picker;
    public ButtonThruster ButtonThruster;

    bool lockedUI;

    public void SetReady(bool ready)
    {
        Picker.ReadyButtonText.text = ready ? "Unready" : "Ready";
    }

    public void SetTeam(PlayerScript.TeamEnum team)
    {
        Color backgroundColor;
        switch (team)
        {
            case PlayerScript.TeamEnum.Red:
                backgroundColor = Color.red;
                break;
            case PlayerScript.TeamEnum.Blue:
                backgroundColor = Color.blue;
                break;
            default:
                backgroundColor = Color.gray;
                break;
        }
        Picker.Background.color = backgroundColor;
    }

    public void LockUI()
    {
        lockedUI = true;
    }
}
