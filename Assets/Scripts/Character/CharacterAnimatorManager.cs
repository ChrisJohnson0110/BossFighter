using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float fVertical;
        float fHorizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float a_fHorizontalValue, float a_fVerticalValue)
        {
            character.animator.SetFloat("Horizontal", a_fHorizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", a_fVerticalValue, 0.1f, Time.deltaTime);

            //can snap values refer tohttps://www.youtube.com/watch?v=i4msADmNMOM&list=PLD_vBJjpCwJvP9F9CeDRiLs08a3ldTpW5&index=6&ab_channel=SebastianGraves
            //13 minutes into video
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
