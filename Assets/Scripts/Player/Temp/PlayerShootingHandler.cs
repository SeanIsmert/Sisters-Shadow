using AIController;
using UnityEngine;

public class PlayerShootingHandler : MonoBehaviour//,IShooting
{
    [SerializeField] private Transform _gunBarrel;

    private Animator _animator;
    private bool _aiming;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Aim()
    {
        _animator.SetBool("Aiming", true);
        _aiming = true;
    }
    public void AimRelease()
    {
        _animator.SetBool("Aiming", false);
        _aiming = false;
    }

    public void Shoot(int amount)
    {
        if (!_aiming )
            return;
        //if (PlayerManager.instance.bullets <= 0)
        //    return;
        if (_animator.GetCurrentAnimatorStateInfo(2).IsName("Pistol Shoot"))
            return;
        
        _animator.Play("Pistol Shoot");

        //PlayerManager.instance.Shoot(amount);

        RaycastHit hit;


        if (Physics.Raycast(_gunBarrel.position, _gunBarrel.forward, out hit, 25))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = hit.transform.GetComponentInParent<IDamageable>();
            }
            if (damageable == null)
            {
                damageable = hit.transform.GetComponentInChildren<IDamageable>();
            }

            //damageable?.TakeDamage(-1);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, .1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("AI"))
            {
                var sense = collider.GetComponent<Sense>();
                if (sense != null)
                {
                    sense.isVisible = true;
                }
            }
        }
    }
}
