using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator = null;
    public bool isOpen = false; //pour le saveSystem apres

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isOpen = true;
        _animator.SetBool("isOpen", isOpen);
    }

    private void OnTriggerExit(Collider other)
    {
        isOpen = false;
        _animator.SetBool("isOpen", isOpen);
    }
}
