using UnityEngine;

public class Player : MonoBehaviour
{

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("HorizontalP2") * Time.deltaTime, Input.GetAxis("VerticalP2") * Time.deltaTime);
    }
}   
