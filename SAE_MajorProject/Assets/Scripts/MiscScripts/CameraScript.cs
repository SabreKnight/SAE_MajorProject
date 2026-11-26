using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    private Camera cameraRef;
    [SerializeField] private NetworkObject PlayerObject;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 1f;

    private void Awake()
    {
        if(cameraRef == null)
        {
            cameraRef = GetComponent<Camera>();
        }
    }

    public void SetPlayerTrack(NetworkObject playerobj)
    {
        PlayerObject = playerobj;
    }

    private void FixedUpdate()
    {
        LerpCamPosition();
    }

    private void LerpCamPosition()
    {
        if(PlayerObject != null)
        {
            Vector3 desiredPosition = PlayerObject.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 10f);
            transform.position = smoothedPosition;       
        }

    }



}
