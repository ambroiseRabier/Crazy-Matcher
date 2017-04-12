
namespace Utils
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour  where T : Singleton<T>
    {

        #region Variables

        public enum SingletonMode {
            replaceExisting,
            destroySelfIfAlreadyExists
        }

        public static T instance;

        [Header("Singleton")]
        [SerializeField]
        [Range(0, 1)]
        private SingletonMode m_SingletonMode = SingletonMode.destroySelfIfAlreadyExists;
        //[SerializeField]
        //private bool m_dontDestroyGameObjectOnLoad = true;
        
        protected Transform m_transform;

        #endregion
        

        protected virtual void Awake()
        {
            switch (m_SingletonMode)
            {
                case SingletonMode.destroySelfIfAlreadyExists:
                    if (instance != null)
                        Destroy(gameObject);
                    else instance = this as T;
                    break;
                default:
                case SingletonMode.replaceExisting:
                    if (instance != null)
                        Destroy(instance.gameObject);
                    instance = this as T;
                    break;
            }

            //if (m_dontDestroyGameObjectOnLoad)
            //    DontDestroyOnLoad(gameObject);

            m_transform = transform;
        }
    }
}
