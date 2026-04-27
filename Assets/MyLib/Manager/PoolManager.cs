using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NFramework;

public class PoolManager : SingletonMono<PoolManager>
{
    [SerializeField] private List<PoolObject> _poolObjects = new List<PoolObject>();
    private Dictionary<TypePool,PoolObject> _mapper=new Dictionary<TypePool, PoolObject>();

    private void OnValidate()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            _poolObjects[i].name = _poolObjects[i].typePool.ToString();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //init
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (_poolObjects[i].initCreate != 0)
            {
                for (int j = 0; j < _poolObjects[i].initCreate; j++)
                {
                    GameObject obj = Instantiate (_poolObjects [i].objectPool, Vector3.zero, Quaternion.identity)as GameObject;
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    _poolObjects[i].lstObjectWasPool.Add(obj);
                }
            }

            if (_mapper.ContainsKey(_poolObjects[i].typePool) == false)
            {
                _mapper.Add(_poolObjects[i].typePool, _poolObjects[i]);
            }
        }
    }

    public GameObject GetObjectByType(TypePool _typePool, Vector3 pos, Quaternion quater)
    {
        return GetObjectByTypePrivate(_typePool, pos, quater, false);
    }

    public void HideAllPool()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            for (int j = 0; j < _poolObjects[i].lstObjectWasPool.Count; j++)
            {
                _poolObjects[i].lstObjectWasPool[j].SetActive(false);
            }
        }
    }

    public void HidePool(TypePool typePool)
    {
        PoolObject poolObject = _mapper[typePool];
        for (int j = 0; j < poolObject.lstObjectWasPool.Count; j++)
        {
            poolObject.lstObjectWasPool[j].SetActive(false);
        }
    }

    private GameObject GetObjectByTypePrivate(TypePool typePool, Vector3 pos, Quaternion quater, bool init)
    {
        PoolObject poolObject = _mapper[typePool];

        if (!init)
        {
            for (int j = 0; j < poolObject.lstObjectWasPool.Count; j++)
            {
                GameObject obj1 = poolObject.lstObjectWasPool [j];
                if (!obj1.activeSelf)
                {
                    obj1.transform.position = pos;
                    obj1.transform.rotation = quater;
                    obj1.SetActive(true);
                    return obj1;
                }
            }
        }

        //init

        GameObject obj = Instantiate (poolObject.objectPool, Vector3.zero, Quaternion.identity)as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.position = pos;
        obj.transform.rotation = quater;
        obj.SetActive(true);

        poolObject.lstObjectWasPool.Add(obj);
        Debug.LogWarning("INIT POOL INSTACE = " + typePool);
        return obj;
    }
}

[System.Serializable]
public class PoolObject
{
    [HideInInspector] public string name;
    public TypePool typePool;
    public int initCreate = 5;
    public GameObject objectPool;
    [HideInInspector] public List<GameObject> lstObjectWasPool = new List<GameObject> ();

    public static PoolObject instance(PoolObject _pool)
    {
        PoolObject poolobject = new PoolObject ();
        poolobject.typePool = _pool.typePool;
        poolobject.initCreate = _pool.initCreate;
        poolobject.objectPool = _pool.objectPool;
        return poolobject;
    }
}