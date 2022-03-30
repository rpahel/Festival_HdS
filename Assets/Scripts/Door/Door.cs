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
        _animator.SetBool("isOpen", true);
        isOpen = true;
    }

    private void OnTriggerExit(Collider2D other)
    {
        _animator.SetBool("isOpen", false);
        isOpen = false;
    }
}
