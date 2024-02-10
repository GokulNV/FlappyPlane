using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible _collectiblePrefab;
    [SerializeField] private Transform _centerTransform;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private float _minDistanceBetweenCollectibles = 5f;
    [SerializeField] private float _minY = 1f;
    [SerializeField] private float _maxY = 5f;
    [SerializeField] private int _maxAttempts = 10;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private int _maxQueueSize = 10; // Maximum number of collectibles allowed

    private Queue<Collectible> _collectibleQueue = new Queue<Collectible>();
    private float _distanceToPlane;
    private Vector3 _centerPosition;

    private void Start()
    {
        _centerPosition = _centerTransform.position;
        Vector3 planePosition = _planeTransform.position;
        _distanceToPlane = Vector3.Distance(_centerPosition, planePosition);

        RandomizeSpawns();
    }

    private void RandomizeSpawns()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        while (true)
        {
            Vector3 spawnPosition = FindSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                SpawnCollectible(spawnPosition);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SpawnCollectible(Vector3 spawnPosition)
    {
        if (_collectibleQueue.Count >= _maxQueueSize)
        {
            // If the queue size limit is reached, reuse the first item in the queue
            var firstCollectible = _collectibleQueue.Dequeue();
            firstCollectible.transform.position = spawnPosition;
            firstCollectible.gameObject.SetActive(true);
            firstCollectible.OnCollected += Collected;
            _collectibleQueue.Enqueue(firstCollectible); // Re-enqueue the reused collectible
        }
        else
        {
            // Instantiate a new collectible and add it to the queue
            var newCollectible = Instantiate(_collectiblePrefab, spawnPosition, Quaternion.identity);
            newCollectible.OnCollected += Collected;
            _collectibleQueue.Enqueue(newCollectible);
        }
    }

    private Vector3 FindSpawnPosition()
    {
        Vector3 planePosition = _planeTransform.position;

        // Ensure the spawn position is at least the minimum distance away from the plane
        Vector3 spawnPosition = _centerPosition + (_planeTransform.position - _centerPosition).normalized * _distanceToPlane;

        // Calculate the Y position using a sine wave function
        float sineY = Mathf.Sin(Time.time) * (_maxY - _minY) / 2f + (_maxY + _minY) / 2f;

        // Spawn collectibles only if the sine wave value is positive (top half of the sine wave)
        if (sineY > (_maxY + _minY) / 2f)
        {
            spawnPosition.y = sineY;

            // Check if the distance between collectibles is above a certain value
            int attempts = 0;
            while (Physics.CheckSphere(spawnPosition, _minDistanceBetweenCollectibles, _obstacleMask) && attempts < _maxAttempts)
            {
                // If the spawn position is obstructed, try a different position
                spawnPosition = _centerPosition + (_planeTransform.position - _centerPosition).normalized * _distanceToPlane + Random.insideUnitSphere * _minDistanceBetweenCollectibles;

                sineY = Mathf.Sin(Time.time) * (_maxY - _minY) / 2f + (_maxY + _minY) / 2f;
                if (sineY > (_maxY + _minY) / 2f)
                {
                    spawnPosition.y = sineY;
                }

                attempts++;
            }

            if (attempts >= _maxAttempts)
            {
                Debug.LogWarning("Failed to find a suitable spawn position due to minimum distance constraint.");
                return Vector3.zero;
            }
            else
            {
                return spawnPosition;
            }
        }
        else
        {
            // If the sine wave value is negative or zero, return Vector3.zero to indicate no spawn position
            return Vector3.zero;
        }
    }

    private void Collected(Collectible collectible)
    {
        collectible.OnCollected -= Collected;
        collectible.gameObject.SetActive(false);
    }
}
