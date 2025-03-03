using UnityEngine;

public class MagnetPullLine : MonoBehaviour
{
    public MagnetKingBoss boss;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("Magnet Pull Interrupted!");
            boss.StunMagnetPull(); // Calls the function in MagnetKingBoss
            Destroy(other.gameObject); // Destroy the bullet on impact
        }
    }
}
