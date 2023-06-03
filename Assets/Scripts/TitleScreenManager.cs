using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ 
{
    public class TitleScreenManager : MonoBehaviour
    {
        /// <summary>
        /// Start the network manager as a host
        /// </summary>
        public void StartNewWorkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        /// <summary>
        /// load new game
        /// </summary>
        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }
}


