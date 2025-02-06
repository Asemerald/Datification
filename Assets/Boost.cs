using UnityEngine;

public class Boost : MonoBehaviour
{

    [SerializeField] private Animator boostAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            boostAnimator.SetBool("isTaken",true);
        }
    }
}
