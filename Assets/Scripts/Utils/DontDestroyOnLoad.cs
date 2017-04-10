using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sugar.Utils
{
    public class DontDestroyOnLoad : MonoBehaviour
    {

        private static List<string> gameObjectCreateName;

        void Awake()
        {
            if (gameObjectCreateName == null)
            {
                gameObjectCreateName = new List<string>();
            }
            else if (gameObjectCreateName.Contains(gameObject.name))
            {
                Destroy(gameObject);
                return;
            }

            gameObjectCreateName.Add(gameObject.name);

            DontDestroyOnLoad(gameObject);

        }
    }
}
