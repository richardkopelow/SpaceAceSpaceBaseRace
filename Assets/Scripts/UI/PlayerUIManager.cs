using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerScript Player;
    public UIDoor Door;
    public ComponentPicker Picker;
    public ButtonThruster ButtonThruster;

    Transform trans;

    bool lockedUI;

    private void Start()
    {
        trans = GetComponent<Transform>();
        Door.Open();
    }

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

    public void SetUI(JobsEnum job)
    {
        StartCoroutine(setUICoroutine(job));
    }

    private IEnumerator setUICoroutine(JobsEnum job)
    {
        yield return Door.Close();
        showUI(job);
        yield return Door.Open();
    }

    private void showUI(JobsEnum job)
    {
        Transform uiTransform = null;
        foreach (Transform ui in trans)
        {
            if (ui.name == job.ToString())
            {
                ui.gameObject.SetActive(true);
                uiTransform = ui;
            }
            else
            {
                if (ui.name != "UIDoor")
                {
                    ui.gameObject.SetActive(false);
                }
            }
        }
        switch (job)
        {
            case JobsEnum.Thruster:
                uiTransform.Find("ThrusterButton").GetComponent<ThrusterButton>().Player = Player;
                break;
            default:
                break;
        }
    }

    public void LockUI()
    {
        lockedUI = true;
    }
}
