using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    public Rigidbody playerRigidbody;
    public float speed;

    public Weapon weapon;
    private Animator animator;
    public float hitSpeed;

    void Start() {
        animator = playerRigidbody.gameObject.GetComponent<Animator>();
        animator.SetFloat("hitSpeed", hitSpeed);
        weapon.AttackDuration = weapon.AttackDuration / hitSpeed;
        animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update() {
        ApplyMovement();
        ApplyRotation();
        HandleWeaponAttack();
    }

    private void ApplyMovement() {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");
        playerRigidbody.velocity = new Vector3(mH * speed, playerRigidbody.velocity.y, mV * speed);

    }

    private void ApplyRotation() {
        RotationByMouse();
    }

    private void RotationByMouse() {

        Vector3 mouse = Input.mousePosition;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 1f));
        //go.transform.position = mouseWorld;
        Vector3 newPos = mouseWorld - Camera.main.transform.position;
        newPos.y = 0f;
        playerRigidbody.rotation = Quaternion.FromToRotation(Vector3.forward, newPos);

    }

    private void RotationByMouse2() {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(playerRigidbody.transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Ta Daaa
        playerRigidbody.transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void HandleWeaponAttack() {
        if (Input.GetButtonDown("Fire1")) {
            weapon.Attack();
            animator.SetBool("isAttacking", true);
            StartCoroutine(WaitAttack());
        }
    }

    private IEnumerator WaitAttack() {
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(weapon.AttackDuration);
    }

}