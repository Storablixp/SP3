using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTrans;
    [SerializeField] private int maxX;
    [SerializeField] private float yOffset = 1.5f;

    public void SetUp(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }

    void Update()
    {
        if (playerTrans != null)
        {
            //if (playerTrans.position.x > maxX)
            //{
            //    transform.position = new Vector3(maxX, playerTrans.position.y + yOffset, -10);
            //}
            //else if (playerTrans.position.x < -maxX)
            //{
            //    transform.position = new Vector3(-maxX, playerTrans.position.y + yOffset, -10);
            //}
            //else transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + yOffset, -10);
            transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + yOffset, -10);
        }
    }
}
