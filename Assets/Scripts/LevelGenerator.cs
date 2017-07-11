using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform SpaceBase;
    public Rigidbody2D Asteroid;
    public float MaxRadius = 10f;
    public float MinRadius = 5f;
    public float BufferRadius = 2f;
    public float AstroidDensityMax = 0.02f;
    public float AstroidDensityMin = 0.01f;

    public void Generate()
    {
        //Spawn Base
        Transform baseTrans = Instantiate<Transform>(SpaceBase);
        float angle = Random.Range(0f, 2 * Mathf.PI);
        float radius = Random.Range(MinRadius, MaxRadius);
        baseTrans.position = new Vector3(Mathf.Cos(angle),Mathf.Sin(angle))*radius;
        
        //Asteroid Field
        float density = Random.Range(AstroidDensityMin, AstroidDensityMax);
        float area = Mathf.PI * (radius * radius - BufferRadius*BufferRadius);
        int asteroids = Mathf.RoundToInt(area * density);
        for (int i = 0; i < asteroids; i++)
        {
            float scale = 1 + Random.Range(-0.3f, 0.3f);
            Rigidbody2D asteroid = Instantiate<Rigidbody2D>(Asteroid);
            Asteroid.GetComponent<Transform>().localScale = new Vector3(scale, scale, 1);
            float astAngle = Random.Range(0f, 2*Mathf.PI);
            float astRadius = Random.Range(BufferRadius, MaxRadius);
            asteroid.position = new Vector3(Mathf.Cos(astAngle), Mathf.Sin(astAngle)) * astRadius;
        }
    }
}
