using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public Transform[] spawnClusters;
    public GameObject rockPrefab;
    public GameObject smallrockPrefab;
    public float rockVelocity = 10f;
    public int minPerCluster = 3;
    public int maxPerCluster = 9;
    public float respawnInterval = 0.5f;

    private float lastSpawn = 0;

    // Update is called once per frame
    void Update()
    {
        float dt = Time.realtimeSinceStartup;
        if (dt - lastSpawn > respawnInterval)
        {
            SpawnRocks();
            lastSpawn = dt;
        }

    }

    void SpawnRocks()
    {
        for (int i = 0; i < spawnClusters.Length; i++)
        {
            float force = Random.Range(1, 30);

            for (int x = 0; x < Random.Range(minPerCluster, maxPerCluster); x++)
            {
                var rock = Instantiate(rockPrefab, spawnClusters[i].position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0), Quaternion.identity);
                var rb = rock.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, -force * rockVelocity));

                for (int y = 0; y < Random.Range(minPerCluster, maxPerCluster * 2); y++)
                {
                    var smallrock = Instantiate(smallrockPrefab, spawnClusters[i].position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0), Quaternion.identity);
                    var srb = smallrock.GetComponent<Rigidbody2D>();
                    srb.AddForce(new Vector2(0, -force * rockVelocity));

                }
            }
        }

    }
}
