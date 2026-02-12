using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    private float minX = -20.50f;
    private float maxX = 20.50f;
    private float minY =42.98f;
    private float maxY = 42.98f;
    public float spawnInterval = 15f;
    
    private float spawnTimer = 0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval)
        {
            SpawnPowerUp();
            spawnTimer = 0f;
        }
    }

    void SpawnPowerUp()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 randomPos = new Vector3(randomX, randomY, transform.position.z);
        Instantiate(powerUpPrefab, randomPos, Quaternion.identity);
    }
}