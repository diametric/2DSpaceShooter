using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    class Star
    {
        public GameObject starObject;
        public float speed;
    }

    public float spawnFrequency = 0.5f;
    public int spawnMin = 5;
    public int spawnMax = 10;
    public GameObject starPrefab;
    public GameObject spawnPoint;

    private readonly List<Star> stars = new List<Star>();
    private readonly List<Star> cleanup = new List<Star>();
    private float lastSpawn = 0;

    void Update()
    {
        float dt = Time.realtimeSinceStartup;
        if (dt - lastSpawn > spawnFrequency)
        {
            SpawnStars();
            lastSpawn = dt;
        }

    }

    private void FixedUpdate()
    {
        foreach (var star in stars)
        {
            float step = star.speed * Time.fixedDeltaTime;

            Vector2 oldPos = star.starObject.transform.position;
            oldPos.y = Mathf.MoveTowards(oldPos.y, -2, step);
            star.starObject.transform.position = oldPos;

            // We made it!
            if (oldPos.y <= -2)
            {
                cleanup.Add(star);
                Destroy(star.starObject);
            }
        }

        foreach (var deadStar in cleanup)
        {
            stars.Remove(deadStar);
        }
        cleanup.Clear();
    }

    void SpawnStars()
    {
        for (int i = 0; i < Random.Range(spawnMin, spawnMax); i++)
        {
            var starObj = Instantiate(starPrefab, spawnPoint.transform.position + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity);

            float size = Random.Range(0.8f, 3f);
            starObj.transform.localScale = new Vector3(size, size, size);

            var star = new Star
            {
                starObject = starObj,
                speed = Random.Range(1, 5)
            };

            stars.Add(star);
        }

    }
}
