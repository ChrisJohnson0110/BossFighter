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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(iWorldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return iWorldSceneIndex;
        }



    }
}


