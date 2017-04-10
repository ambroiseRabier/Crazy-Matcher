using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWatches : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerWatches playerWatches = collision.gameObject.GetComponent<PlayerWatches>();
        if (playerWatches != null && !playerWatches.OnFire)
        {
            playerWatches.Alight();
        }

    }
}
