using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{
    public class PlayerUIManager : MonoBehaviour
    {
        static PlayerUIManager instance;

        public static PlayerUIManager Instance
        {
            get { return instance; }
        }

        [Header("Network Join")]
        [SerializeField] bool bStartGameAsClient;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (bStartGameAsClient == true)
            {
                bStartGameAsClient = false;
                //shut down host client to start as client
                NetworkManager.Singleton.Shutdown();
                //start as client
                NetworkManager.Singleton.StartClient();

            }
        }
    }
}
