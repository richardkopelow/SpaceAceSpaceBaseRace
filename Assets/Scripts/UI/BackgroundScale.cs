using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    void Start()
    {
        Transform trans = GetComponent<Transform>();
        Camera cam = trans.parent.GetComponent<Camera>();
        float yScale = cam.orthographicSize * 2;
        float xScale = cam.aspect * yScale;
        trans.localScale = new Vector3(xScale, yScale, 1);
    }
}
