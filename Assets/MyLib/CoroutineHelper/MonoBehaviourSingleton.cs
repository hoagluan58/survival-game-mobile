using UnityEngine;
public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
{
    private static TSelfType m_Instance = null;
    public static TSelfType Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (TSelfType)FindObjectOfType(typeof(TSelfType));
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject(typeof(TSelfType).Name)).AddComponent<TSelfType>();
                }
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as TSelfType;
           // if (!m_IsNeedDestroy)
            {
                //If I am the first instance, make me the Singleton
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != m_Instance)
                Destroy(gameObject);
        }
    }
}