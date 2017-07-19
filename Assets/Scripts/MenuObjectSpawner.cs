using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectSpawner : MonoBehaviour
{
    public float Density = 0.03f;
    public float OrthographicHeight = 5f;
    public Rigidbody2D Asteroid;

    private void Start()
    {
        float orthographicWidth = OrthographicHeight * Screen.width / Screen.height;
        float area = OrthographicHeight * orthographicWidth;
        for (int i = 0; i < area * Density; i++)
        {
            float scale = 1 + Random.Range(-0.3f, 0.3f);
            Rigidbody2D asteroid = Instantiate<Rigidbody2D>(Asteroid);
            asteroid.GetComponent<Transform>().localScale = new Vector3(scale, scale, 1);
            asteroid.position = new Vector2((Random.value - 0.5f) * orthographicWidth, (Random.value - 0.5f) * OrthographicHeight);
            asteroid.velocity = Random.insideUnitCircle;
        }
    }
}
