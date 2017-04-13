using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerWater : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();

        if (burnable)
            burnable.TryExtinguish();
    }
}
