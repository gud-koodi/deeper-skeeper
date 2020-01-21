using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    public float damage;
    public float sweepReach;
    public float sweepLength;

    public override void Attack() {
        Debug.Log("VIUH!");
        Collider[] colliders = Physics.OverlapCapsule(transform.position + transform.forward, transform.position + (transform.forward * 2), sweepLength );
        DebugExtension.DebugCapsule(transform.position + transform.forward, transform.position + (transform.forward * 2), Color.red, sweepLength, 3);
        
        for (int i = 0; i < colliders.Length; i++) {
            Collider c = colliders[i];
            c.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

} 