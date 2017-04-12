using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Burner : MonoBehaviour {

    public Matches fireOwner;

    private void OnTriggerEnter(Collider collision)
    {
        print(collision.name);
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();

        if (burnable)
        {
            if (fireOwner != null)
            {
                burnable.matchesBurnMe = fireOwner;

                print(fireOwner.name);
            }

            burnable.TryStartBurn();
        }
    }
}
