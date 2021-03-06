﻿namespace GudKoodi.DeeperSkeeper.Entity
{
    using Event;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Anything that has HP.
    /// </summary>
    public class Living : MonoBehaviour, IDamageable
    {
        [FormerlySerializedAs("maxHealth")]
        public float MaxHealth;

        /// <summary>
        /// Temporary solution; type of the object for server communication.
        /// </summary>
        [Tooltip("Temporary solution; type of the object for server communication.")]
        public ObjectType ObjectType;

        /// <summary>
        /// Event called when this object's health goes below 0.
        /// </summary>
        [Tooltip("Event called when this object's health goes below 0.")]
        public ObjectUpdateRequested ObjectDestructRequested;

        private float currentHealth;

        private AudioSource hitAudio;

        /// <summary>
        /// Applies damage. Called outside the class on hit events.
        /// </summary>
        /// <param name="damage">float damage amount</param>
        public void ApplyDamage(float damage)
        {
            hitAudio.Play();
            currentHealth -= damage;
            if (currentHealth <= 0f)
            {
                this.ObjectDestructRequested.Trigger(this.gameObject, this.ObjectType);
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

        /// <summary>
        /// Kills the object.
        /// </summary>
        public void Kill()
        {
            SetKinematic(false);
            GetComponent<Animator>().enabled = false;
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                Destroy(gameObject, 5);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
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
            hitAudio = GetComponent<AudioSource>();
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
    }
}
