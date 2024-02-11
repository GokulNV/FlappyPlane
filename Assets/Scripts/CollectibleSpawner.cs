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
    [SerializeField] private int _maxQueueSize = 10;

    private Queue<Collectible> _collectibleQueue = new Queue<Collectible>();
    private float _distanceToPlane;
    private Vector3 _centerPosition;

    /// <summary>
    /// Initializes necessary variables and starts spawning collectibles.
    /// </summary>
    private void Start()
    {
        _centerPosition = _centerTransform.position;
        Vector3 planePosition = _planeTransform.position;
        _distanceToPlane = Vector3.Distance(_centerPosition, planePosition);

        StartCoroutine(StartSpawning());
    }

    /// <summary>
    /// Coroutine responsible for continuously spawning collectibles.
    /// </summary>
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

    /// <summary>
    /// Spawns a collectible at the specified position.
    /// </summary>
    /// <param name="spawnPosition">The position to spawn the collectible.</param>
    private void SpawnCollectible(Vector3 spawnPosition)
    {
        if (_collectibleQueue.Count >= _maxQueueSize)
        {
            var firstCollectible = _collectibleQueue.Dequeue();
            firstCollectible.transform.position = spawnPosition;
            firstCollectible.gameObject.SetActive(true);
            _collectibleQueue.Enqueue(firstCollectible);
        }
        else
        {
            var newCollectible = Instantiate(_collectiblePrefab, spawnPosition, Quaternion.identity, transform);
            _collectibleQueue.Enqueue(newCollectible);
        }
    }

    /// <summary>
    /// Finds a suitable position to spawn a collectible.
    /// </summary>
    /// <returns>The position to spawn the collectible, or Vector3.zero if no suitable position is found.</returns>
    private Vector3 FindSpawnPosition()
    {
        Vector3 spawnPosition = _centerPosition + (_planeTransform.position - _centerPosition).normalized * _distanceToPlane;

        float sineY = Mathf.Sin(Time.time) * (_maxY - _minY) / 2f + (_maxY + _minY) / 2f;
        if (sineY > (_maxY + _minY) / 2f)
        {
            spawnPosition.y = sineY;

            int attempts = 0;
            while (Physics.CheckSphere(spawnPosition, _minDistanceBetweenCollectibles, _obstacleMask) && attempts < _maxAttempts)
            {
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
            return Vector3.zero;
        }
    }
}
