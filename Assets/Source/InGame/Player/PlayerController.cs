using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController:MonoBehaviour
{
	public float MoveSpeed = 5.0f;
	public float AttackCoolDown = 0.5f;

	public enum AnimState
	{
		Idle,
		Move,
		Attack,
		Damage,
	}

	public void AttackEnd()
	{
		mIsAttacking = false;
	}

	void Start()
	{
		mController = GetComponent<CharacterController>();
		mAnimator = GetComponent<AnimatorWrapper>();
	}

	void Update()
	{
		//Maoave
		PlayerMove();
		//Attack
		PlayerAttack();
	}

	/// <summary>
	/// Move
	/// </summary>
	void PlayerMove()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Vector3 input = new Vector3(h, 0, v);

		if (input.magnitude > 0.1f)
		{
			//Rotate In Direction
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), Time.deltaTime * 10f);
		}

		//Move
		mMoveDirection = input.normalized * MoveSpeed;
		mController.Move(mMoveDirection * Time.deltaTime);
	}

	/// <summary>
	/// Attack
	/// </summary>
	void PlayerAttack()
	{
		if (Time.time < mNextAttackTime) return;
		if (Input.GetMouseButtonDown(0))
		{
			mAnimator.Play(AnimStateID.Attack, 0, 0, ()=> { });
			mIsAttacking = true;
			mNextAttackTime = Time.time + AttackCoolDown;
		}
	}


	

	bool mIsAttacking = false;
	float mNextAttackTime = 0f;
	CharacterController mController;
	Vector3 mMoveDirection;
	AnimatorWrapper mAnimator;

}
