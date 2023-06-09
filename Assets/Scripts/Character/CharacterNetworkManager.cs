using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{

    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //can only edit if owned
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 v3NetworkPositionVelocity;
        public float fNetworkPositionSmoothTime = 0.1f;
        public float fNetworkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> fHorizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> fVerticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> fMoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> bIsSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        //called from client to server - request
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong a_iClientID, string a_sAnimationID, bool a_bApplyRootMotion)
        {
            //if this client is host then activate client rpc
            if (IsServer == true)
            {
                PlayActionAnimationForAllClientsClientRpc(a_iClientID, a_sAnimationID, a_bApplyRootMotion);
            }
        }

        //client rpc sent to all clients from server
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong a_iClientID, string a_sAnimationID, bool a_bApplyRootMotion)
        {
            //make sure not to run on client that sent it
            if (a_iClientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(a_sAnimationID, a_bApplyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string a_sAnimationID, bool a_bApplyRootMotion)
        {
            character.bApplyRootMotion = a_bApplyRootMotion;
            character.animator.CrossFade(a_sAnimationID, 0.2f); //make sure value same as character animation maanger cross fade value
        }

    }
}
