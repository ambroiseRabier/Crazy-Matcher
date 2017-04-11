using UnityEngine;

public class FireMatches : MonoBehaviour {
    private void OnTriggerEnter(Collider collision)
    {
        Matches playerWatches = collision.gameObject.GetComponent<Matches>();

        if (playerWatches != null && !playerWatches.IsOnFire)
            playerWatches.Alight();
    }
}
