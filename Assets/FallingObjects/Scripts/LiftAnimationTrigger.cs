using UnityEngine;
using BNG;

public class LiftAnimationTrigger : MonoBehaviour
{

    private Animator craneAnimator;
    private Animator crateWithFallingObjectsAnimator;

    private GameObject Player;

    //private Grabbable grabbable;  // Vet ikke om denne kanskje er riktig for BNG Framework sin Player. Spaken fra BNG har i hvert fall denne komponenten.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        craneAnimator = GameObject.FindGameObjectWithTag("Crane").GetComponent<Animator>();
        crateWithFallingObjectsAnimator = GameObject.FindGameObjectWithTag("CrateWithFallingObjects").GetComponent<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player");

        //grabbable = gameObject.GetComponent<Grabbable>(); // Mulig man kan bruke denne for � sjekke om spaken er grepet av spilleren med "grabbable.BeingHeld", men usikker.
    }


    // Forel�pig er det satt opp slik at spaken aktiverer animasjonene n�r spilleren g�r inn i triggeren.
    // Vil endre denne til at man m� gripe spaken for � aktivere animasjonene.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            craneAnimator.SetTrigger("PullLiftLeverCrane");
            crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes");
            Debug.Log("Lift lever pulled");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
