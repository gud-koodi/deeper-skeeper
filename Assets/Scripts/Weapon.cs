using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    public float AttackDuration { get; set; }
    public float attackAnimationDuration;
    public float damage;
    public abstract void Attack();
    void Awake()
    {
        AttackDuration = attackAnimationDuration;
    }
}