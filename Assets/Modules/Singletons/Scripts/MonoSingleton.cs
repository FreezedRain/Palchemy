using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{ 
    private static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning($"Found second instance of {typeof(T)}!");
            Destroy(gameObject);
        }
        _instance = this as T;
    }
}