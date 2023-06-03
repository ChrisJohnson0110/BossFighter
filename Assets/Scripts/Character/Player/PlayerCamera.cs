using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerCamera : MonoBehaviour
    {
        static PlayerCamera instance;

        public Camera cameraObject;

        public static PlayerCamera Instance
        {
            get { return instance; }
        }

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
