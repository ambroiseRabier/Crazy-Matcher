using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Burner : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();

        if (burnable)
            burnable.TryStartBurn();
    }
}
