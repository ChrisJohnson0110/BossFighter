using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class CharacterManager : MonoBehaviour
    {
        /// <summary>
        /// ensure player persists between scenes
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
