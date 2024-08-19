using UnityEngine;

public class Spider : MonoBehaviour
{
    public int damage;
    public PlayerController playerController;

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			playerController.TakeDamage(damage);
		}
	}
}
