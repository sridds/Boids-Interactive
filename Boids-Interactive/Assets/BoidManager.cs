using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoidEntity _boidEntityPrefab;

    [Header("Settings")]
    [SerializeField] private Vector3 _boidBounds;
    [SerializeField] private int _maxBoidEntities;

    private BoidEntity[] _boidEntities;

    private void Start()
    {
        _boidEntities = new BoidEntity[_maxBoidEntities];

        for(int i = 0; i < _maxBoidEntities; i++)
        {
            _boidEntities[i] = Instantiate(_boidEntityPrefab, transform.position, Quaternion.identity);
        }
    }

    #region Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.1f);
        Gizmos.DrawCube(transform.position, _boidBounds);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.4f);
        Gizmos.DrawCube(transform.position, _boidBounds);
    }
    #endregion
}
