using System.Collections;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _collectiblePrefab;
    private int _totalCount = 10;
    private ObjectPools<Collectible> _collectiblePool;

    private void Start()
    {
        _collectiblePool = new ObjectPools<Collectible>(_collectiblePrefab, transform);
        RandomizeSpawns();
    }

    private void RandomizeSpawns()
    {
        // Random.Range(0, 3)
        switch (0)
        {
            case 0:
                StartCoroutine(SpawnInLine());
                break;
            case 1:
                // StartCoroutine(SpawnParabolic());
                break;
            case 2:
                // StartCoroutine(SpawnInverseParabolic());
                break;
            default:
                // StartCoroutine(SpawnInLine());
                break;
        }
    }


    private IEnumerator SpawnInLine()
    {
        int count = 0;
        while (count < _totalCount)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 0f, 0f);
            var spawnedCollectible = _collectiblePool.GetObject();
            spawnedCollectible.gameObject.transform.position = spawnPos;
            spawnedCollectible.OnCollected += Collected;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3f);
        RandomizeSpawns();
    }

    // private IEnumerator SpawnParabolic()
    // {

    // }

    // private IEnumerator SpawnInverseParabolic()
    // {

    // }

    private void Collected(Collectible collectible)
    {
        collectible.OnCollected -= Collected;
        _collectiblePool.ReleaseObject(collectible);
    }
}