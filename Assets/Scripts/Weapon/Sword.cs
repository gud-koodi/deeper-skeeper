
namespace GudKoodi.DeeperSkeeper.Weapon
{
    using System.Collections;
    using UnityEngine;

    public class Sword : Weapon
    {
        //public float sweepReach;
        //public float sweepLength;
        private bool isAttacking;

        private AudioSource swingSound;

        public override void Attack()
        {
            Debug.Log("VIUH!");
            //Collider[] colliders = Physics.OverlapCapsule(transform.position + transform.forward, transform.position + (transform.forward * 2), sweepLength );
            //DebugExtension.DebugCapsule(transform.position + transform.forward, transform.position + (transform.forward * 2), Color.red, sweepLength, 3);

            //for (int i = 0; i < colliders.Length; i++) {
            //    Collider c = colliders[i];
            //    c.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            //}
            isAttacking = true;
            StartCoroutine(SetAttackingFalse());
        }

        void Start()
        {
            swingSound = GetComponent<AudioSource>();
        }

        private IEnumerator SetAttackingFalse()
        {
            swingSound.Play();
            yield return new WaitForSeconds(AttackDuration);
            isAttacking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isAttacking && this.transform.root.tag != other.transform.root.tag)
            {
                other.gameObject.SendMessageUpwards("ApplyDamage", damage);
            }
        }
    }
}
