using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDoor : MonoBehaviour
{
    public bool IsOpen;
    public Transform Top;
    public Transform Bottom;

    private Vector3 topOriginal;
    private Vector3 bottomOriginal;

    private void Start()
    {
        topOriginal = Top.position;
        bottomOriginal = Bottom.position;
    }

    public Coroutine Close()
    {
        return StartCoroutine(closeCoroutine());
    }

    private IEnumerator closeCoroutine()
    {
        Vector3 topStart = Top.position;
        Vector3 bottomStart = Bottom.position;

        float time = 0;

        while (time < 1)
        {
            Top.position = Vector3.Lerp(topStart, topOriginal, time);
            Bottom.position = Vector3.Lerp(bottomStart, bottomOriginal, time);
            time += Time.deltaTime;
            yield return null;
        }
        Top.position = topOriginal;
        Bottom.position = bottomOriginal;

        IsOpen = false;
    }

    public Coroutine Open()
    {
        return StartCoroutine(openCoroutine());
    }

    private IEnumerator openCoroutine()
    {
        Vector3 topStart = Top.position;
        Vector3 bottomStart = Bottom.position;
        Vector3 topEnd = new Vector3(topStart.x, Screen.height * 13 / 6, topStart.z);
        Vector3 bottomEnd = new Vector3(bottomStart.x, -Screen.height * 7 / 6, bottomStart.z);

        float time = 0;

        while (time < 1)
        {
            Top.position = Vector3.Lerp(topStart, topEnd, time);
            Bottom.position = Vector3.Lerp(bottomStart, bottomEnd, time);
            time += Time.deltaTime;
            yield return null;
        }
        Top.position = topEnd;
        Bottom.position = bottomEnd;

        IsOpen = true;
    }
}
