using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int fVertical;
        int fHorizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            fVertical = Animator.StringToHash("Vertical");
            fHorizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorMovementParameters(float a_fHorizontalValue, float a_fVerticalValue, bool a_bIsSprinting)
        {
            float fHorizontalAmount = a_fHorizontalValue;
            float fVerticalAmount = a_fVerticalValue;

            if (a_bIsSprinting == true)
            {
                fVerticalAmount = 2f;
            }

            character.animator.SetFloat(fHorizontal, fHorizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(fVertical, fVerticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetAnimation(
            string a_sTargetAnimaition, 
            bool a_bPerformingAction, 
            bool a_bApplyRootMotion = true, 
            bool a_bCanRotate = false, 
            bool a_bCanMove = false)
        {
            character.bApplyRootMotion = a_bApplyRootMotion;
            character.animator.CrossFade(a_sTargetAnimaition, 0.2f);
            //can be used to stop character to stop new action
            character.bIsPerformingAction = a_bPerformingAction;
            character.bCanRotate = a_bCanRotate;
            character.bCanMove = a_bCanMove;

            //tell host we played animation
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId,
                a_sTargetAnimaition, a_bApplyRootMotion);


        }


    }
}
