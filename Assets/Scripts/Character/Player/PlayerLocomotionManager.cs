using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        //from input manager
        public float fVerticalMovement;
        public float fHorizontalMovement;
        public float fMoveAmount;

        private Vector3 v3MovementDirection;
        private Vector3 v3TargetRotationDirection;

        [SerializeField] float fWalkingSpeed = 2f;
        [SerializeField] float fRunningSpeed = 5f;
        [SerializeField] float fRotationSpeed = 15f;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            //arial movement


        }

        private void GetVerticalAndHorizontalMovement()
        {
            fVerticalMovement = PlayerInputManager.Instance.fVerticalInput;
            fHorizontalMovement = PlayerInputManager.Instance.fHorizontalInput;
            //can also do fMoveAmount here

            //clamp
        }


        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalMovement();

            //movement dir based on camera persp & input
            v3MovementDirection = PlayerCamera.Instance.transform.forward * fVerticalMovement;
            v3MovementDirection = v3MovementDirection + PlayerCamera.Instance.transform.right * fHorizontalMovement;
            v3MovementDirection.Normalize();
            v3MovementDirection.y = 0;

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

        private void HandleRotation()
        {
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

    }
}
