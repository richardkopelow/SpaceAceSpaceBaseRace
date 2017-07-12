using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform trans;

    private Transform spaceBase;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        trans = GetComponent<Transform>();
        spriteRenderer = trans.Find("Arrow").GetComponent<SpriteRenderer>();
        spaceBase = GameObject.Find("SpaceBase").GetComponent<Transform>();
        foreach (Transform child in trans)
        {
            child.gameObject.layer = gameObject.layer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lookAt(spaceBase.position);
        spriteRenderer.material.SetVector("_TargetPosition", spaceBase.position);
    }

    void lookAt(Vector3 position)
    {
        Vector3 diff = position - trans.position;
        float deg = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90;
        trans.rotation = Quaternion.AngleAxis(deg, trans.forward);
    }
}
