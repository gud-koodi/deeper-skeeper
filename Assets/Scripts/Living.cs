using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour, IDamageable {
    public float maxHealth;
    private float currentHealth;

    public void ApplyDamage(float damage) {
        Debug.Log("DAMAGE");
        currentHealth -= damage;
        if (currentHealth <= 0f) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {

    }

    private void Init() {
        currentHealth = maxHealth;
    }
}