using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    private Camera cameraRef;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 1f;

    private void Awake()
    {
        if(cameraRef == null)
        {
            cameraRef = GetComponent<Camera>();
        }
    }

    private void OnEnable()
    {
        if(PlayerObject == null)
        {
            PlayerObject = GameManager.Instance.Playerobj;
        }        
    }

    private void FixedUpdate()
    {
        LerpCamPosition();
    }

    private void LerpCamPosition()
    {
          

        Vector3 desiredPosition = PlayerObject.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 10f);
        transform.position = smoothedPosition;
    }



}
