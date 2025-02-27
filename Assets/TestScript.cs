using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding: " + collision.collider.tag);

        // Find out which hand is poking
        // if (collision.collider.CompareTag("LeftHandPokeInteractor"))
        // {
        //     _leftHandBoxCollider = collision.collider as BoxCollider;
        //     Debug.Log("Left hand poking");
        // }
        // else if (collision.collider.CompareTag("RightHandPokeInteractor"))
        // {
        //     _rightHandBoxCollider = collision.collider as BoxCollider;
        //     Debug.Log("Right hand poking");
        // }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Not colliding: " + collision.collider.tag);

        // Find out which hand is poking
        // if (collision.collider.CompareTag("LeftHandPokeInteractor"))
        // {
        //     _leftHandBoxCollider = null;
        //     Debug.Log("Left hand stopped poking");
        // }
        // else if (collision.collider.CompareTag("RightHandPokeInteractor"))
        // {
        //     _rightHandBoxCollider = null;
        //     Debug.Log("Right hand stopped poking");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
