using UnityEngine;
public abstract class Weapon : MonoBehaviour {
    public float AttackDuration { get; set; }
    public float attackDuration;
    public float damage;
    public abstract void Attack();
    public virtual void Start() {
        AttackDuration = attackDuration;
    }
}