using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJ
{
    public class PlayerCamera : MonoBehaviour
    {
        static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;


        //change to impact camera performance
        [Header("Camera Settings")]
        private float fCameraSmoothSPeed = 1f; //larger number means it will take camera longer to move
        [SerializeField] float fLeftAndRightRotationSpeed = 220f;
        [SerializeField] float fUpAndDownRotationSpeed = 220f;
        [SerializeField] float fMinimumPivot = -30f; //lowest down angle
        [SerializeField] float fMaximumPivot = 60f; //highest up angle
        [SerializeField] float fCameraCollisionRadius = 0.2f;
        [SerializeField] float fCameraMoveTime = 0.2f; //collision move time
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 v3CameraVelocity;
        private Vector3 v3CameraObjectPosition; //moves camera object to this position / when colliding
        [SerializeField] float fLeftAndRightLookAngle;
        [SerializeField] float fUpAndDownLookAngle;
        private float fCameraZPosition; //value used for camera collision
        private float fTargetCameraZPositon; //value used for camera collision


        public static PlayerCamera Instance
        {
            get { return instance; }
        }

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
            fCameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowPlayer();

                HandleRotations();

                HandleCollisions();
            }

        }

        private void HandleFollowPlayer()
        {
            Vector3 v3TargetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, 
                ref v3CameraVelocity, fCameraSmoothSPeed * Time.deltaTime);
            transform.position = v3TargetCameraPosition;
        }

        private void HandleRotations()
        {
            //if locked on force rotation towards target
            //else normal

            //rotate left and rght based on horizonal movement of joystick
            fLeftAndRightLookAngle += (PlayerInputManager.Instance.fCameraHorizontalInput * fLeftAndRightRotationSpeed) * Time.deltaTime;
            //rotate up and down based on horizonal movement of joystick
            fUpAndDownLookAngle -= (PlayerInputManager.Instance.fCameraVerticalInput * fUpAndDownRotationSpeed) * Time.deltaTime;
            //clamp the up and down look angle 
            fUpAndDownLookAngle = Mathf.Clamp(fUpAndDownLookAngle, fMinimumPivot, fMaximumPivot);


            Vector3 v3CameraRotation = Vector3.zero;
            Quaternion qTargetRotation;

            //rotate this gameobject on left/right
            v3CameraRotation.y = fLeftAndRightLookAngle;
            qTargetRotation = Quaternion.Euler(v3CameraRotation);
            transform.rotation = qTargetRotation;


            //rotate the pivot gameobject up/down
            v3CameraRotation = Vector3.zero;
            v3CameraRotation.x = fUpAndDownLookAngle;
            qTargetRotation = Quaternion.Euler(v3CameraRotation);
            cameraPivotTransform.localRotation = qTargetRotation;
        }

        private void HandleCollisions()
        {
            fTargetCameraZPositon = fCameraZPosition;
            RaycastHit hit;
            //direction for collision check
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();
            //check if object in direction ^
            if (Physics.SphereCast(cameraPivotTransform.position, fCameraCollisionRadius, direction, out hit, Mathf.Abs(fTargetCameraZPositon), collideWithLayers))
            {
                //if there is, move away
                float fDistanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                fTargetCameraZPositon = -(fDistanceFromHitObject - fCameraCollisionRadius);
            }

            //if our target position is less than collision radius
            //subtract collsion radius - snap back
            if (Mathf.Abs(fTargetCameraZPositon) < fCameraCollisionRadius)
            {
                fTargetCameraZPositon = -fCameraCollisionRadius;
            }

            //apply final position using lerp over time
            v3CameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, fTargetCameraZPositon, fCameraMoveTime);
            cameraObject.transform.localPosition = v3CameraObjectPosition;


        }



    }
}
