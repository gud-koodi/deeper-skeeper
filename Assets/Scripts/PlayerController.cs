using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody playerRigidbody;
    public float speed;

    public Weapon weapon;
    private Animator animator;
    public float hitSpeed;

    void Start()
    {
        animator = playerRigidbody.gameObject.GetComponent<Animator>();
        animator.SetFloat("hitSpeed", hitSpeed);
        weapon.AttackDuration = weapon.AttackDuration / hitSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
        ApplyRotation();
        HandleWeaponAttack();
    }

    private void ApplyMovement()
    {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");
        if (playerRigidbody.velocity.y < -10)
        {
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        }
        else
        {
            playerRigidbody.velocity = new Vector3(mH * speed, playerRigidbody.velocity.y, mV * speed);
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

    private void RotationByMouse()
    {

        Vector3 mouse = Input.mousePosition;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 1f));
        //go.transform.position = mouseWorld;
        Vector3 newPos = mouseWorld - Camera.main.transform.position;
        newPos.y = 0f;
        playerRigidbody.rotation = Quaternion.FromToRotation(Vector3.forward, newPos);

    }

    private void HandleWeaponAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            weapon.Attack();
            animator.SetBool("isAttacking", true);
            StartCoroutine(WaitAttack());
        }
    }

    private IEnumerator WaitAttack()
    {
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(weapon.AttackDuration);
    }

}