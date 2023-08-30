using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    protected static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new T();
            }
            return m_Instance;
        }
    }
}

public class SingletonGameobject<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                T obj = GameObject.FindObjectOfType<T>();
                if (obj != null)
                {
                    m_Instance = obj;
                }
                else
                {
                    var new_obj = new GameObject();
                    new_obj.hideFlags = HideFlags.HideAndDontSave;
                    m_Instance = new_obj.AddComponent<T>();
                }
            }

            return m_Instance;
        }
    }
}