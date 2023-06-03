using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace CJ
{

    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //can only edit if owned
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 v3NetworkPositionVelocity;
        public float fNetworkPositionSmoothTime = 0.1f;
        public float fNetworkRotationSmoothTime = 0.1f;


    }
}
