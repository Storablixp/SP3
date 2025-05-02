using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTrans;
    [SerializeField] private float yOffset = 1.5f;

    public void SetUp(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }

    void Update()
    {
        if (playerTrans != null)
        {
            transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + yOffset, -10);
        }
    }
}
