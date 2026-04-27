using UnityEngine;

public class RandomObject : MonoBehaviour
{
    [SerializeField] private GameObject[] _allObjects;

    private void Start()
    {
        //for (int i = 0; i < _allObjects.Length; i++)
        //{
        //    _allObjects[i].SetActive(false);
        //}
        //_allObjects[Random.Range(0, _allObjects.Length)].SetActive(true);
    }
}
