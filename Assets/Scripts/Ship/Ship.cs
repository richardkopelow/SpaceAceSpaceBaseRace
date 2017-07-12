using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public TeamEnum Team;
    public string ShipName;
    public Material RedMaterial;
    public Material BlueMaterial;
    public ShipComponent[] ShipComponents;

    private void Start()
    {
        Skin(Team == TeamEnum.Blue ? BlueMaterial : RedMaterial);
    }

    public void Skin(Material mat)
    {
        foreach (SpriteRenderer renderer in GetComponent<Transform>().GetComponentsInChildren<SpriteRenderer>())
        {
            if (renderer.name != "Arrow")
            {
                renderer.material = mat;
            }
        }
    }
}
