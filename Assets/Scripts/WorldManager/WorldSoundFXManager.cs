using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        static WorldSoundFXManager instance;
        public static WorldSoundFXManager Instance 
        {
            get { return instance; }
        }

        [Header("Action Sounds")]
        public AudioClip rollSFX;


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

    }
}