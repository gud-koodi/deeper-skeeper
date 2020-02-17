using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour, IDamageable
{
    public float maxHealth;
    private float currentHealth;

    public void ApplyDamage(float damage)
    {
        Debug.Log("DAMAGE");
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        currentHealth = maxHealth;
        SetKinematic(true);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
            rb.GetComponent<Collider>().enabled = !newValue;
        }
    }

    private void Die()
    {
        SetKinematic(false);
        GetComponent<Animator>().enabled = false;
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }
        Destroy(gameObject, 5);
    }
}