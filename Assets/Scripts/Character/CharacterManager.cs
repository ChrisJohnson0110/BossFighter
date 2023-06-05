using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;

        [Header("Flags")]
        public bool bIsPerformingAction = false;
        public bool bApplyRootMotion = false;
        public bool bCanRotate = true;
        public bool bCanMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            //if this character is being controlled from our side then assign network position to the position of our transform
            if (IsOwner == true)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else //if this character is being controlled from somewhere else then assign its position here locally to be the position of its network transform
            {
                transform.position = Vector3.SmoothDamp(transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.v3NetworkPositionVelocity, 
                    characterNetworkManager.fNetworkPositionSmoothTime);


                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.fNetworkRotationSmoothTime);



            }
        }
        
        protected virtual void LateUpdate()
        {

        }

    }
}
