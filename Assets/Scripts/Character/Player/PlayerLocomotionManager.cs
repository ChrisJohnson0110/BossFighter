using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        //from input manager
        [HideInInspector] public float fVerticalMovement;
        [HideInInspector] public float fHorizontalMovement;
        [HideInInspector] public float fMoveAmount;

        [Header("Movement Settings")]
        private Vector3 v3MovementDirection;
        private Vector3 v3TargetRotationDirection;

        [SerializeField] float fWalkingSpeed = 2f;
        [SerializeField] float fRunningSpeed = 5f;
        [SerializeField] float fSprintingSpeed = 8f;
        [SerializeField] float fRotationSpeed = 15f;

        [Header("Dodge")]
        private Vector3 v3RollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner == true)
            {
                player.characterNetworkManager.fVerticalMovement.Value = fVerticalMovement;
                player.characterNetworkManager.fHorizontalMovement.Value = fHorizontalMovement;
                player.characterNetworkManager.fMoveAmount.Value = fMoveAmount;
            }
            else
            {
                fVerticalMovement = player.characterNetworkManager.fVerticalMovement.Value;
                fHorizontalMovement = player.characterNetworkManager.fHorizontalMovement.Value;
                fMoveAmount = player.characterNetworkManager.fMoveAmount.Value;
                
                //if not locked on
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, fMoveAmount, player.playerNetworkManager.bIsSprinting.Value);

                //if locked on
            }
        }

        public void HandleAllMovement()
        {
            
            HandleGroundedMovement();
            HandleRotation();
            //arial movement


        }

        private void GetMovementValues()
        {
            fVerticalMovement = PlayerInputManager.Instance.fVerticalInput;
            fHorizontalMovement = PlayerInputManager.Instance.fHorizontalInput;
            fMoveAmount = PlayerInputManager.Instance.fMoveAmount;

            //clamp
        }

        private void HandleGroundedMovement()
        {
            if (player.bCanMove == false)
            {
                return;
            }
            GetMovementValues();

            //movement dir based on camera persp & input
            v3MovementDirection = PlayerCamera.Instance.transform.forward * fVerticalMovement;
            v3MovementDirection = v3MovementDirection + PlayerCamera.Instance.transform.right * fHorizontalMovement;
            v3MovementDirection.Normalize();
            v3MovementDirection.y = 0;

            if (player.playerNetworkManager.bIsSprinting.Value == true)
            {
                player.characterController.Move(v3MovementDirection * fSprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.Instance.fMoveAmount > 0.5f)
                {
                    //move - running
                    player.characterController.Move(v3MovementDirection * fRunningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.Instance.fMoveAmount <= 0.5f)
                {
                    //move - walking
                    player.characterController.Move(v3MovementDirection * fWalkingSpeed * Time.deltaTime);
                }
            }
            


        }

        private void HandleRotation()
        {
            if (player.bCanRotate == false)
            {
                return;
            }

            v3TargetRotationDirection = Vector3.zero;
            v3TargetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * fVerticalMovement;
            v3TargetRotationDirection = v3TargetRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * fHorizontalMovement;
            v3TargetRotationDirection.Normalize();
            v3TargetRotationDirection.y = 0;

            if (v3TargetRotationDirection == Vector3.zero)
            {
                v3TargetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(v3TargetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, fRotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting()
        {
            if (player.bIsPerformingAction == true)
            {
                player.playerNetworkManager.bIsSprinting.Value = false;
            }



            if (fMoveAmount >= 0.5f)
            {
                player.playerNetworkManager.bIsSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.bIsSprinting.Value = false;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.bIsPerformingAction == true)
            {
                return;
            }
            //if moving roll / otherwise should backstep
            if (fMoveAmount > 0)
            {
                v3RollDirection = PlayerCamera.Instance.cameraObject.transform.forward * fVerticalMovement;
                v3RollDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.fHorizontalInput;
                v3RollDirection.y = 0;
                v3RollDirection.Normalize();

                Quaternion qPlayerRotation = Quaternion.LookRotation(v3RollDirection);
                player.transform.rotation = qPlayerRotation;

                player.playerAnimatorManager.PlayTargetAnimation("RollForward", true, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("BackStep", true, true);
            }
        }

    }
}
