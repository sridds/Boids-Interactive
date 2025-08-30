using UnityEngine;

[System.Serializable]
public struct BoidSettings
{
    public Vector3 boidBounds;
    public float minSpeed;
    public float maxSpeed;
    public float rotationSpeed;

    [Header("Range")]
    public float seperationRange;
    public float alignmentRange;
    public float cohesionRange;

    [Header("Constants")]
    public float seperationK;
    public float alignmentK;
    public float cohesionK;
}

public class BoidManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoidEntity _boidEntityPrefab;

    [Header("Settings")]
    [SerializeField] private int _maxBoidEntities;

    [Header("Boid Settings")]
    [SerializeField] BoidSettings _settings;

    private BoidEntity[] _boidEntities;

    private void Start()
    {
        _boidEntities = new BoidEntity[_maxBoidEntities];

        for(int i = 0; i < _maxBoidEntities; i++)
        {
            _boidEntities[i] = Instantiate(_boidEntityPrefab, GetRandomPointInBounds(), Quaternion.identity);
            _boidEntities[i].Init(_settings);
        }
    }

    private void Update()
    {
        for(int i = 0; i < _maxBoidEntities; i++)
        {
            _boidEntities[i].UpdateBoid(_boidEntities, i);
        }
    }

    private Vector3 GetRandomPointInBounds()
    {
        float x = Random.Range(-_settings.boidBounds.x, _settings.boidBounds.x);
        float y = Random.Range(-_settings.boidBounds.y, _settings.boidBounds.y);
        float z = Random.Range(-_settings.boidBounds.z, _settings.boidBounds.z);

        return (new Vector3(x, y, z) / 2.0f) + transform.position;
    }

    #region Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.1f);
        Gizmos.DrawCube(transform.position, _settings.boidBounds);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.4f);
        Gizmos.DrawCube(transform.position, _settings.boidBounds);
    }
    #endregion
}
