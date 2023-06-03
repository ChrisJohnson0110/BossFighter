using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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
    }
}
