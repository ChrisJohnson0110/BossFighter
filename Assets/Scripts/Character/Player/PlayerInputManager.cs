using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CJ 
{
    public class PlayerInputManager : MonoBehaviour
    {
        static PlayerInputManager instance;
        public PlayerManager player;
        public static PlayerInputManager Instance
        {
            get { return instance; }
        }

        PlayerControls playerControls;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 v2CameraInput;
        [SerializeField] public float fCameraVerticalInput;
        [SerializeField] public float fCameraHorizontalInput;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 v2MovementInput;
        [SerializeField] public float fVerticalInput;
        [SerializeField] public float fHorizontalInput;

        [SerializeField] public float fMoveAmount;

        [Header("Player Action Input")]
        [SerializeField] bool bDodgeInput = false;
        [SerializeField] bool bSprintInput = false;


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
                playerControls.PlayerMovement.Movement.performed += i => v2MovementInput = i.ReadValue<Vector2>(); //on left joystickmove update vector 2
                playerControls.PlayerCamera.Movement.performed += i => v2CameraInput = i.ReadValue<Vector2>(); //on right joystickmove update vector 2
                playerControls.PlayerActions.Dodge.performed += i => bDodgeInput = true; //on o press
                playerControls.PlayerActions.Sprint.performed += i => bSprintInput = true; //sprint down
                playerControls.PlayerActions.Sprint.canceled += i => bSprintInput = false; //sprint up

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
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprinting();
        }

        //movements

        private void HandlePlayerMovementInput()
        {
            fVerticalInput = v2MovementInput.y;
            fHorizontalInput = v2MovementInput.x;

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

            //pass 0 on horizontal to only get non strafing movement
            //use horizontal when locked on for strafe

            if (player == null)
            {
                return;
            }

            //if not locked only use move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, fMoveAmount, player.playerNetworkManager.bIsSprinting.Value);

            //if we are locked on pass on horizontal as well
        }

        private void HandleCameraMovementInput()
        {
            fCameraVerticalInput = v2CameraInput.y;
            fCameraHorizontalInput = v2CameraInput.x;


        }

        //actions

        private void HandleDodgeInput()
        {
            if (bDodgeInput == true)
            {
                bDodgeInput = false;

                //to add if menu open disable ?

                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprinting()
        {
            if (bSprintInput == true)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.bIsSprinting.Value = false;
            }
        }

    }
}


