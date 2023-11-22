using UnityEngine;
namespace ProjectSRG
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance is null)
                    _instance = FindObjectOfType<T>();

                if (_instance is null)
                {
                    _instance = new GameObject().AddComponent<T>();
                    _instance.name = $"Singleton - {nameof(T)}";
                }

                return _instance;
            }
        }
        private static T _instance;
    }
}