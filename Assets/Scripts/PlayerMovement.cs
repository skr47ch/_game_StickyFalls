﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D playerRigidBody;
	public Transform checkGround;
	public LayerMask whatIsGround;
	public bool isSpinoff = false;

	public Camera cameraM;
	public float maxSpeed = 5f;
	public float groundRadius = 0.1f;
//	public float jumpForce = 500f;
	public int ExchangePoints = 0;
	private bool GroundColor = false;   //0->Blue, 1->Red

	bool isGrounded = false;

	public float jumpHeight;
	private float jumpSpeed;
	Vector2 velocity;

	private Vector3 newCameraPosition;
	private Bounds camerabound;

	void Start() {
		//playerRigidBody = GetComponent<Rigidbody2D> ();
		//transform = GetComponent<Transform> ();
		jumpSpeed = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight) + 0.1f;
	}

	void FixedUpdate() {
		Movement ();
	}

	void Update() {
		camerabound = CameraExtensions.OrthographicBounds(cameraM);
		 
		if(playerRigidBody.position.x > camerabound.max.x) 	MoveCamera(camerabound.size.x, 0);
		if(playerRigidBody.position.x < camerabound.min.x) 	MoveCamera(-camerabound.size.x, 0);
		if(playerRigidBody.position.y > camerabound.max.y) 	MoveCamera(0, camerabound.size.y);
		if(playerRigidBody.position.y < camerabound.min.y) 	MoveCamera(0, -camerabound.size.y);


		if(ExchangePoints < 0) 	Dead();
//		if(this.gameObject) Debug.Log(ExchangePoints);
		CheckJump();
	}

	void Movement() {
		isGrounded = Physics2D.OverlapCircle (checkGround.position, groundRadius, whatIsGround);
		float move = Input.GetAxis ("Horizontal");
		playerRigidBody.velocity = new Vector2 (move * maxSpeed, playerRigidBody.velocity.y);
	}

	void CheckJump() {
//		Debug.Log(isGrounded);
		if (isGrounded && Input.GetButton("Jump")){
			velocity = playerRigidBody.velocity;
			velocity.y = jumpSpeed;
			playerRigidBody.velocity = velocity;
//			Debug.Log("Jumping");
//			playerRigidBody.AddForce(new Vector2(0, jumpForce));
		}
	}

	void OnCollisionEnter2D(Collision2D collide){
		if(!GroundColor && collide.gameObject.CompareTag("RedGround")){
			GroundColor = true;
			ExchangePoints -= 1;
		}
		if(GroundColor && collide.gameObject.CompareTag("BlueGround")){
			GroundColor = false;
			ExchangePoints -= 1;
		}
		if(collide.gameObject.CompareTag("DeadGround")){
			Dead();
		}
	}

	void OnTriggerEnter2D(Collider2D collide){
//		Debug.Log("Got Point");
		if(collide.gameObject.CompareTag("ExchangePoints")){
			Destroy(collide.gameObject);
			ExchangePoints += 1;
		}
	}

	void Dead() {
		Debug.Log("You Ded");
		Destroy(this.gameObject);
	}

	void MoveCamera(float xBound, float yBound) {
		newCameraPosition = cameraM.transform.position;

		newCameraPosition.x = cameraM.transform.position.x + xBound;
		newCameraPosition.y = cameraM.transform.position.y + yBound;

		cameraM.transform.position = newCameraPosition;
	}
}
