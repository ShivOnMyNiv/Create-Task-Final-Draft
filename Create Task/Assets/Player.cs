using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Sets the speed of the character   
    public float Speed;
    //Determines jump height
    public float JumpHeight;
    float JumpHeightStore;
    public float JumpDecay;
    //Maintains Jump
    public GameObject Spawn;
    bool IsSpawn;
    public static int ScoreReal;
    public Text ScoreDisplay;
    //Random location spawner and shows score
	float TimeCheck;
	public int WinCount;
	public int LossCount;
	public GameObject PlayerCharacter;
	public GameObject WinUI;
	public GameObject LossUI;
	public Text Wins;
	public Text Losses;
	//Meant to manage win or lose condition and handle UI components

    void Start() {
        JumpHeight = 400;
        JumpDecay = 60;
        JumpHeightStore = JumpHeight;
        //Sets Jump
        IsSpawn = false;
        //Allows characters to spawn
		TimeCheck = 0;
		Wins = WinUI.gameObject.GetComponent<Text>();
		Losses = LossUI.gameObject.GetComponent<Text>();
		WinCount = 0;
		LossCount = 0;
    }

    // Update is called once per frame
    void Update() {
        //Makes sure that certain functions won't run if the program isn't the player itself
        if (gameObject.CompareTag("Player")) {
            //Manages basic movement
            Movement();
            //Spawns scoring object
            Spawner();
        } else {
            //The functions here need to be used by other programs as well
            Spawner();
        }
        //This is for general putpose functions
        WinCheck();
    }

    void Movement() {
        //Moves Ball Right
        MoveRight();
        //Moves Ball Left
        MoveLeft();
        //Handles Jumping
        Jump();
    }

    void MoveRight() {
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += new Vector3(Speed, 0, 0);
        }
    }

    void MoveLeft() {
        if (Input.GetKey(KeyCode.RightArrow)) {
            //Ensures that program prioritizes moving right over moving left
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position -= new Vector3(Speed, 0, 0);
        }
    }

    void Jump() {
        if (GetComponent<Rigidbody2D>().velocity.y >= -0.1f && GetComponent<Rigidbody2D>().velocity.y <= 0.1f) {
            JumpHeight = JumpHeightStore;
        }

        if (Input.GetKey(KeyCode.UpArrow) && GetComponent<Rigidbody2D>().velocity.y >= -0.5f) {
            if (JumpHeight > 0) { 
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpHeight));
            }
            JumpHeight -= JumpDecay;
        }
    }

    void Spawner() {
        if (gameObject.CompareTag("Player")) { 
            Debug.Log("Spawned Object");
            Debug.Log(IsSpawn);
            if (IsSpawn == false) {
                Vector3 SpawnPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(-1.94f, 4f));
                Instantiate(Spawn, SpawnPosition, Quaternion.identity);
                IsSpawn = true;
            }
        }
        ScoreDisplay.text = "Score: " + ScoreReal.ToString();
    }

    //Manages the Spawn Collissions
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Spawn") && this.gameObject.CompareTag("Player")) {
            Destroy(col.gameObject);
            IsSpawn = false;
            ScoreReal++;
        }
    }

    void WinCheck() {
        if (ScoreReal >= 20) {
			Reset(true);
        }
        if (TimeCheck >= 20) {
			Reset(false);
        }
		TimeCheck += Time.deltaTime;
		Debug.Log (WinCount + LossCount);
    }

    //This function acts as my abstraction
	private void Reset(bool WinLose) {
        ScoreReal = 0;
		TimeCheck = 0; 
        Vector3 DefaultPosition = new Vector3(0, 2.5f, -1);
		PlayerCharacter.gameObject.transform.position = DefaultPosition;
		if (WinLose == true) {
			WinCount++;
		} else {
			LossCount++;
		}
		Wins.text = "Wins: " + WinCount.ToString();
		Losses.text = "Losses: " + LossCount.ToString();
        Destroy(Spawn);
    }
}

