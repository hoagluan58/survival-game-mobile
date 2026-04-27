using UnityEngine;

public class MyPath : MonoBehaviour
{
    [SerializeField] private Transform[] _paths;

    public Vector3[] GetPaths()
    {
        Vector3[] vPaths = new Vector3[_paths.Length];
        for (int i = 0; i < vPaths.Length; i++)
        {
            vPaths[i] = _paths[i].position;
        }
        return vPaths;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < _paths.Length - 1; i++)
        {
            Gizmos.DrawLine(_paths[i].position, _paths[i + 1].position);
        }
    }
#endif
}
