using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; // Reference to the cherry prefab
    public Transform levelCenter; // Center point of the level
    public float cherrySpawnInterval = 10.0f; // Time interval for spawning cherries
    public float cherrySpeed = 5.0f; // Speed at which the cherry moves

    private Camera mainCamera;
    private float spawnTimer = 0.0f;

    void Start()
    {
        mainCamera = Camera.main;

        // Start spawning cherries immediately
        spawnTimer = cherrySpawnInterval;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a cherry
        if (spawnTimer >= cherrySpawnInterval)
        {
            SpawnCherry();
            spawnTimer = 0.0f; // Reset the timer
        }
    }

    void SpawnCherry()
    {
        // Determine a random spawn point just outside of the camera view
        Vector3 spawnPoint = GetRandomSpawnPoint();

        // Instantiate the cherry at the spawn point
        GameObject cherry = Instantiate(cherryPrefab, spawnPoint, Quaternion.identity);

        // Calculate the direction for the cherry to move
        Vector3 moveDirection = (levelCenter.position - spawnPoint).normalized;

        // Get the cherry's Rigidbody2D and set its velocity for linear lerping
        Rigidbody2D cherryRigidbody = cherry.GetComponent<Rigidbody2D>();
        cherryRigidbody.velocity = moveDirection * cherrySpeed;

        // Start a coroutine to check if the cherry is out of camera view and destroy it
        StartCoroutine(CheckCherryOutOfBounds(cherry));
    }

    IEnumerator CheckCherryOutOfBounds(GameObject cherry)
    {
        // Check if the cherry is still within the camera view
        while (IsInCameraView(cherry.transform.position))
        {
            yield return null;
        }

        // Cherry is out of camera view, destroy it
        Destroy(cherry);
    }

    bool IsInCameraView(Vector3 position)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    Vector3 GetRandomSpawnPoint()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Define possible spawn sides
        List<Vector3> spawnSides = new List<Vector3>
        {
            new Vector3(-cameraWidth * 0.5f, Random.Range(-cameraHeight * 0.5f, cameraHeight * 0.5f), 0),
            new Vector3(cameraWidth * 0.5f, Random.Range(-cameraHeight * 0.5f, cameraHeight * 0.5f), 0),
            new Vector3(Random.Range(-cameraWidth * 0.5f, cameraWidth * 0.5f), -cameraHeight * 0.5f, 0),
            new Vector3(Random.Range(-cameraWidth * 0.5f, cameraWidth * 0.5f), cameraHeight * 0.5f, 0)
        };

        // Randomly select one of the spawn sides
        return spawnSides[Random.Range(0, spawnSides.Count)];
    }
}
