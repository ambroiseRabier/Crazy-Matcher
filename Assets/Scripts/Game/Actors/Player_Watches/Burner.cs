using Events;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Burner : MonoBehaviour {
    
    public Matches fireOwner;

    private void OnTriggerEnter(Collider collision)
    {
        
        Burnable burnable = collision.gameObject.GetComponent<Burnable>();

        if (burnable)
        {
            if (fireOwner != null)
            {
                burnable.matchesBurnMe = fireOwner;

                // if (only) player burn another matche vibration on gamepad is used.
                if (fireOwner.Controller != null)
                    GlobalEventBus.onLightningMatcheByPlayer.Invoke();
            }

            burnable.TryStartBurn();
        }
    }
}
