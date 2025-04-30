using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Animator handAnim;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !handAnim.GetCurrentAnimatorStateInfo(0).IsName("Hand_Swing"))
        {
            handAnim.Play("Hand_Swing");
        }
    }
}
