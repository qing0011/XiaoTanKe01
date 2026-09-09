using UnityEngine;

public class Flower : MonoBehaviour
{
    public AudioClip collectSound; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collectSound != null)
            {
                AudioManager.Instance.PlaySoundAtPosition(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}