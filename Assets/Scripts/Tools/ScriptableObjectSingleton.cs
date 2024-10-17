using UnityEngine;

namespace GenericUtilities
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();

                    if (results.Length == 0)
                    {
                        //None exist
                        return null;
                    }
                    else if (results.Length > 1)
                    {
                        //Too many
                        return null;
                    }

                    _instance = results[0];
                    _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }

                return _instance;
            }
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void FirstInitialize()
        {
            //Only used to make sure this object is stored
        }
    }
}