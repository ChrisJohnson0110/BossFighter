using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CJ 
{
    public class PlayerInputManager : MonoBehaviour
    {
        static PlayerInputManager instance;

        public static PlayerInputManager Instance
        {
            get { return instance; }
        }

        PlayerControls playerControls;

        [SerializeField] Vector2 movement;
        [SerializeField] public float fVerticalInput;
        [SerializeField] public float fHorizontalInput;
        [SerializeField] public float fMoveAmount;

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

            //on scene changed run
            SceneManager.activeSceneChanged += OnSceneChanged;

            instance.enabled = false;
        }

        /// <summary>
        /// run on scene being changed
        /// </summary>
        private void OnSceneChanged(Scene a_oldScene, Scene a_newScene)
        {
            //if loading world scene
            if (a_newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                instance.enabled = true; //enable player contols
            }
            else
            {
                instance.enabled = false; //disable player contols
            }
        }

        private void OnEnable()
        {
            //controls setup
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>(); //on joystickmove update vector 2
            }
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            //if object destroyed unsubscribe
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnApplicationFocus(bool focus)
        {
            //if game open enable / disable controls
            if (enabled == true)
            {
                if (focus == true)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            fVerticalInput = movement.y;
            fHorizontalInput = movement.x;

            //get total amount of movement
            fMoveAmount = Mathf.Clamp01(Mathf.Abs(fVerticalInput) + Mathf.Abs(fHorizontalInput));

            //clamp values - 0, 0.5, 1
            if (fMoveAmount <= 0.5f && fMoveAmount > 0)
            {
                fMoveAmount = 0.5f;
            }
            else if (fMoveAmount > 0.5f && fMoveAmount <= 1)
            {
                fMoveAmount = 1;
            }

        }

    }
}


