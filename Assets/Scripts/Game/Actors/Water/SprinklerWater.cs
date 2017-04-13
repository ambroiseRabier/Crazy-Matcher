using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerWater : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {
        Matches matches = collision.gameObject.GetComponent<Matches>();

        if (matches)
            matches.TryExtinguish(true);
    }
}
