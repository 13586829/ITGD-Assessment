using UnityEngine;
using System.Collections;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; 
    public float spawnInterval = 10f; 
    public float cherrySpeed = 3f; 

    private float spawnTimer; 

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnCherry();
            spawnTimer = 0; 
        }
    }

    void SpawnCherry()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveCherry(cherry));
    }

    Vector2 GetRandomSpawnPosition()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;
        
        int side = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;
        switch (side)
        {
            case 0: // Top
                spawnPosition = new Vector2(Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight / 2 + 1);
                break;
            case 1: // Bottom
                spawnPosition = new Vector2(Random.Range(-screenWidth / 2, screenWidth / 2), -screenHeight / 2 - 1);
                break;
            case 2: // Left
                spawnPosition = new Vector2(-screenWidth / 2 - 1, Random.Range(-screenHeight / 2, screenHeight / 2));
                break;
            case 3: // Right
                spawnPosition = new Vector2(screenWidth / 2 + 1, Random.Range(-screenHeight / 2, screenHeight / 2));
                break;
        }
        return spawnPosition;
    }

    IEnumerator MoveCherry(GameObject cherry)
    {
        Vector2 endPosition = -cherry.transform.position;
        while (cherry != null && (Vector2)cherry.transform.position != endPosition)
        {
            cherry.transform.position = Vector2.MoveTowards(cherry.transform.position, endPosition, cherrySpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(cherry); 
    }
}
