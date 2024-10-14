using UnityEngine;

public class MonoSinglton<Singleton> : MonoBehaviour where Singleton : MonoBehaviour
{
    private static Singleton _instance;

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                Singleton[] foundSingletons = FindObjectsOfType<Singleton>();

                if (foundSingletons.Length == 1)
                {
                    _instance = foundSingletons[0];
                }
                else if (foundSingletons.Length == 0)
                {
                    Debug.Log("Instance of singleton type does not exist");
                }
                else
                {
                    Debug.Log("Warning: multiple instances of singleton exist");
                    _instance = foundSingletons[0];
                }
            }

            return _instance;
        }
    }
}
