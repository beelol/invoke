using UnityEngine;
using System.Collections;

public class UnitAnimator : MonoBehaviour {

    public float animatorSpeed = 1f;
    private Transform _transform;
    private Animator animator;

    public int moveID;
    public int attackID;
    public int stopID;
    public int dieID;

	//
	void Start () {
        if (!GetComponent<Animator>())
            return;

        animator = GetComponent<Animator>();
        animator.speed = animatorSpeed;
        _transform = transform;

        moveID = Animator.StringToHash("Moving");
        attackID = Animator.StringToHash("Attacking");
        stopID = Animator.StringToHash("Stopping");
        dieID = Animator.StringToHash("Dying");
	}

    public void Move()
    {
        if (!GetComponent<Animator>())
            return;
        animator.SetBool(attackID, false);
        animator.SetBool(stopID, false);
        animator.SetBool(dieID, false);
        animator.SetBool(moveID, true);
    }

    public void Attack()
    {
        if (!GetComponent<Animator>())
            return;
        animator.SetBool(dieID, false);
        animator.SetBool(moveID, false);
        animator.SetBool(stopID, false);
        animator.SetBool(attackID, true);
        animator.ForceStateNormalizedTime(0);
    }

    public void Stop()
    {
        if (!GetComponent<Animator>())
            return;
        animator.SetBool(dieID, false);
        animator.SetBool(moveID, false);
        animator.SetBool(attackID, false);
        animator.SetBool(stopID, true);
    }

    public void Die()
    {
        if (!GetComponent<Animator>())
            return;
        animator.SetBool(moveID, false);
        animator.SetBool(attackID, false);
        animator.SetBool(stopID, false);
        animator.SetBool(dieID, true);
    }
}
