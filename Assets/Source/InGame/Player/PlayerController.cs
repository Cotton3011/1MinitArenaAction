using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController:MonoBehaviour
{
	public float moveSpeed = 5.0f;
	CharacterController mController;
	Vector3 mMoveDirection;

	private void Start()
	{
		mController = GetComponent<CharacterController>();
	}

	private void Update()
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
		mMoveDirection = input.normalized * moveSpeed;
		mController.Move(mMoveDirection * Time.deltaTime);
	}


}
