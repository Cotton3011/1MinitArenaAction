using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController:MonoBehaviour
{
	
	public PlayerState State => mState;

	public enum PlayerState
	{
		Idle,
		Move,
		Attack,
	}

	void Start()
	{
		mController = GetComponent<CharacterController>();
		mAnimator = GetComponent<AnimatorWrapper>();
	}

	void Update()
	{
		switch (mState)
		{
			case PlayerState.Idle:
				IdleUpdate();
				break;
			case PlayerState.Move:
				MoveUpdate();
				break;
			case PlayerState.Attack:
				AttackUpdate();
				break;
		}

		
	}

	/// <summary>
	/// Idle
	/// </summary>
	void IdleUpdate()
	{
		ReadInput();

		if (mInputDir.magnitude > 0.1f)
		{
			ChangeState(PlayerState.Move);
		}

		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			ChangeState(PlayerState.Attack);
		}
	}

	/// <summary>
	/// Move
	/// </summary>
	void MoveUpdate()
	{
		ReadInput();

		if (mInputDir.magnitude < 0.1f) //offInput -> idle
		{
			ChangeState(PlayerState.Idle);
			return;
		}

		//Move
		Vector3 velocity = mInputDir * MOVE_SPEED;
		velocity.y -= GRAVITY * Time.deltaTime; //gravity
		mController.Move(velocity * Time.deltaTime);

		//Rotate
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(mInputDir), Time.deltaTime * 10f);
	}

	/// <summary>
	/// Attack
	/// </summary>
	void AttackUpdate()
	{
		// 攻撃完了まで一切の入力無視
	}

	void ReadInput()
	{
		Vector2 moveInput = Vector2.zero;
		
		// キーボード入力
		if (Keyboard.current != null)
		{
			if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
			if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
			if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;
			if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;
		}
		
		float h = moveInput.x;
		float v = moveInput.y;

		mInputDir = new Vector3(h, 0, v).normalized;
	}

	/// <summary>
	/// Anim StateChange
	/// </summary>
	/// <param name="next"></param>
	void ChangeState(PlayerState next)
	{
		mState = next;

		switch (next)
		{
			case PlayerState.Idle:
				mAnimator.Play(AnimStateID.Idle, 0, 0, () => { });
				break;
			case PlayerState.Move:
				mAnimator.Play(AnimStateID.Player_Move, 0, 0, () => { });
				break;
			case PlayerState.Attack:
				mAnimator.Play(AnimStateID.Player_Attack, 0, 0, () => 
				{
					ChangeState(PlayerState.Idle);
				});
				break;
		}
	}

	const float MOVE_SPEED = 5.0f;
	const float GRAVITY = -9.81f;
	const float ATTACK_COOL_DOWN = 0.5f;
	PlayerState mState = PlayerState.Idle;
	Vector3 mInputDir;
	Vector3 mMove;
	bool mIsAttacking = false;
	float mNextAttackTime = 0f;
	CharacterController mController;
	AnimatorWrapper mAnimator;

}
