using UnityEngine;

public class DamageTEST : MonoBehaviour
{
    [SerializeField] private int _damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);
        }
    }
}
