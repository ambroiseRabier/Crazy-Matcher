using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWatches : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Watches playerWatches = collision.gameObject.GetComponent<Watches>();
        if (playerWatches != null && !playerWatches.OnFire)
        {
            playerWatches.Alight();
        }

    }
}
