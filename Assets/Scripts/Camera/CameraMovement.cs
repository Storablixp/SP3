using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private float yOffset = 1.5f;

    void Update()
    {
        if (playerTrans != null)
        {
            transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + yOffset, -10);
        }
    }
}
