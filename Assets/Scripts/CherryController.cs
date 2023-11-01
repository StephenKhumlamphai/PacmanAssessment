using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject eggPrefab;
    public Transform levelCenter;
    public float eggSpawnInterval = 10.0f;
    public float eggSpeed = 5.0f;
    private Camera mainCamera;
    private float spawnTimer = 0.0f;

    void Start()
    {
        mainCamera = Camera.main;
        spawnTimer = eggSpawnInterval;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= eggSpawnInterval)
        {
            SpawnEgg();
            spawnTimer = 0.0f;
        }
    }

    void SpawnEgg()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        GameObject egg = Instantiate(eggPrefab, spawnPoint, Quaternion.identity);
        Vector3 moveDirection = (levelCenter.position - spawnPoint).normalized;
        Rigidbody2D eggRigidbody = egg.GetComponent<Rigidbody2D>();
        eggRigidbody.velocity = moveDirection * eggSpeed;
        StartCoroutine(CheckEggOutOfBounds(egg));
    }

    IEnumerator CheckEggOutOfBounds(GameObject egg)
    {
        while (IsInCameraView(egg.transform.position))
        {
            yield return null;
        }

        Destroy(egg);
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

        List<Vector3> spawnSides = new List<Vector3>
        {
            new Vector3(-cameraWidth * 0.5f, Random.Range(-cameraHeight * 0.5f, cameraHeight * 0.5f), 0),
            new Vector3(cameraWidth * 0.5f, Random.Range(-cameraHeight * 0.5f, cameraHeight * 0.5f), 0),
            new Vector3(Random.Range(-cameraWidth * 0.5f, cameraWidth * 0.5f), -cameraHeight * 0.5f, 0),
            new Vector3(Random.Range(-cameraWidth * 0.5f, cameraWidth * 0.5f), cameraHeight * 0.5f, 0)
        };

        return spawnSides[Random.Range(0, spawnSides.Count)];
    }
}
