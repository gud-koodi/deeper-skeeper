namespace GudKoodi.DeeperSkeeper.Entity
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Anything that has HP.
    /// </summary>
    public class Living : MonoBehaviour, IDamageable
    {   
        [FormerlySerializedAs("maxHealth")]
        public float MaxHealth;
        private float currentHealth;

        /// <summary>
        /// Applies damage. Called outside the class on hit events.
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(float damage)
        {
            Debug.Log("DAMAGE");
            currentHealth -= damage;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Get current hp percentile.
        /// </summary>
        /// <returns>float in range 0...1</returns>
        public float GetHpPercent()
        {
            return currentHealth / MaxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        private void Init()
        {
            currentHealth = MaxHealth;
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
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            }

            Destroy(gameObject, 5);
        }
    }
}
