using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base MonoBehaviour Singleton class 
/// </summary>
/// <typeparam name="T">Type of singletno</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    /// <summary>
    /// Static instance of this object in the scene
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// if the object should be do not destory on load or not 
    /// </summary>
    [SerializeField]
    private bool doNotDestoryOnLoad = true;

    /// <summary>
    /// Does this singleston Exists?
    /// </summary>
    /// <returns>if this singleton exists</returns>
    public static bool Exist
    {
        get { return instance != null; }
    }

    /// <summary>
    /// Get The instance of the singleton
    /// </summary>
    public static T Instance()
    {
        return Find(); 
    }

    protected virtual void Awake()
    {
        // create singleton 
        if (instance == null)
        {
            instance = gameObject.GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }

        if (doNotDestoryOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Creates the singleton in the scene if none are present or if forced 
    /// </summary>
    /// <param name="forceNew">Forces the creation of the singleton</param>
    public static void Create(bool forceNew = false)
    {
        if (instance == null || forceNew)
        {
            // Checks if a singleton has already been made and is in the scene
            if (forceNew == false)
            {
                instance = (T)FindObjectOfType(typeof(T));
            }

            if (FindObjectsOfType(typeof(T)).Length > 1)
            {
                return;
            }

            // Will create a new object with this singleton class if required 
            if (instance == null || forceNew)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<T>();
                singleton.name = "(singleton) " + typeof(T).ToString();
            }
            else
            {
               
            }
        }
    }


    /// <summary>
    /// Find the given singleton
    /// </summary>
    /// <returns>Found Singleton</returns>
    private static T Find()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
        }
        return instance;
    }

    /// <summary>
    /// Destroy's and kills given singleton
    /// </summary>
    public static void Destroy()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            Destroy(instance);
            instance = null;
        }
    }

}

