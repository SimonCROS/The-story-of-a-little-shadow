using UnityEngine;

public class CharacterEvents : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            SendMessageUpwards("CharacterGroundEnter");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            SendMessageUpwards("CharacterGroundExit");
        }
    }
}
