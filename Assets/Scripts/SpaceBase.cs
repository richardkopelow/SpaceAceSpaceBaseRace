using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBase : MonoBehaviour
{
    
    void Start()
    {

    }
    
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            GameManger.Instance.EndGame(collision.gameObject.GetComponent<Ship>().Team);
        }
    }
}
