using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CJ 
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        static WorldSaveGameManager instance;

        public static WorldSaveGameManager Instance
        {
            get { return instance; }
        }

        [SerializeField] int iWorldSceneIndex;

        /// <summary>
        /// ensure only one instance
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// keep this gameobject instance between scenes
        /// </summary>
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// load given world index async
        /// </summary>
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(iWorldSceneIndex);
            yield return null;
        }
    }
}


