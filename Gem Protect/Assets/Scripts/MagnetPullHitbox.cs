using UnityEngine;

public class MagnetPullHitbox : MonoBehaviour
{
    public MagnetKingBoss boss;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Player Shot the Magnet Line! Pull Disabled!");
            boss.StunMagnetPull(); // Disable magnet pull
            Destroy(other.gameObject); // Destroy bullet on impact
        }
    }
}
