using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody playerRigidbody;
    public float speed = 15f;
    public float hitSpeed = 3f;
    public Weapon weapon;
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        animator = playerRigidbody.gameObject.GetComponent<Animator>();
        animator.SetFloat("hitSpeed", hitSpeed);
        weapon.AttackDuration = weapon.AttackDuration / hitSpeed;
        isAttacking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyMovement();
        if (!isAttacking)
        {
            ApplyRotation();
        }
        HandleWeaponAttack();
    }

    private void ApplyMovement()
    {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");
        if (playerRigidbody.velocity.y >= -10)
        {
            Vector3 movement = (isAttacking) ? Vector3.zero : speed * new Vector3(mH, 0, mV).normalized;
            playerRigidbody.velocity = movement + Vector3.up * playerRigidbody.velocity.y;
        }
        
        if (System.Math.Abs(mH) > 0 || System.Math.Abs(mV) > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void ApplyRotation()
    {
        RotationByMouse();
    }

    /// <summary>
    /// Turns the player towards the point cast from mouse to the point on same y-level as the player.
    /// </summary>
    private void RotationByMouse()
    {
        Vector3 mouse = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(new Vector3(mouse.x, mouse.y));

        //  origin + distance * direction = target -- target.y is player's y-coordinate
        float distance = (playerRigidbody.position.y - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 target = mouseRay.GetPoint(distance) - playerRigidbody.position;
        target.y = 0; // Just in case
        playerRigidbody.rotation = Quaternion.LookRotation(target);
    }

    private void HandleWeaponAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            weapon.Attack();
            animator.SetBool("isAttacking", true);
            isAttacking = true;
            StartCoroutine(WaitAttack());     
        }
    }

    private IEnumerator WaitAttack()
    {
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(weapon.AttackDuration);
        isAttacking = false;
    }

}