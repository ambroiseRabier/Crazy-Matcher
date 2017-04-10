using UnityEngine;

public class FireMatches : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Matches playerWatches = collision.gameObject.GetComponent<Matches>();
        if (playerWatches != null && !playerWatches.OnFire)
        {
            playerWatches.Alight();
        }

    }
}
