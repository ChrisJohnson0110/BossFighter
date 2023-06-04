using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();
            //if we do not own object do not control/edit
            if (IsOwner == false)
            {
                return;
            }

            //handle movement
            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if (IsOwner == false)
            {
                return;
            }

            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //if this player object owned by this client
            if (IsOwner == true)
            {
                PlayerCamera.Instance.player = this;
                PlayerInputManager.Instance.player = this;
            }
        }
    }
}
