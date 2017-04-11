using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Burner : MonoBehaviour {
    private void OnTriggerEnter(Collider collision)
    {
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();

        if (burnable)
            burnable.TryStartBurn();
    }
}
