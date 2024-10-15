using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBallController : MonoBehaviour
{
    public float speed = 5.0f;  //move speed
    public float jumpForce = 10.0f;

    private Rigidbody playerRb; //reference for manipulating the physics of the player

    private Vector3 movement; //a Vector3 direction, for movement from input (need to set at Update)
    private float moveHorizontal;// left <--> Right movement from left / right arrow keys (need to set at Update)
    private float moveVertical; //forward <--> backward movement from up/down arrow keys (need to set at Update)

    //isOnGround: a flag denoting whether the player is on ground or not
    private bool isOnGround = true;

    //gameOver: a flag denoting game over or not
    public bool gameOver = false;

    //gameManagerScript: a reference to that script so as to interact 
    private GameManager gameManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        //obtain referece for playerRb, refer to the RigidBody component of the GameObject the script is attached to
        playerRb = GetComponent<Rigidbody>();
        //Obtain reference for gameManagerScript
        gameManagerScript = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get  movement from Input
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        //Construct the movement Vector
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //Apply force at the movement direction to make it move
        playerRb.AddForce(movement * speed);

        //Jumping: if the space bar is pressed, and the player is on ground, and the game is NOT over, then jump
        //The inOnGround check is to prevent double jumping
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            //add impulse force to the "up" direction
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //once jump: not on ground 
            isOnGround = false;
        }

        //If falling down the platform: Game over
        //Now it's checking the y value. There are other ways to do the job
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
            gameManagerScript.PlayerDied();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if the player (i.e., ball) collides with any object tagged as Ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        //If the RollerBall colides with any treasure (i.e., those game objects tagged as "Treasure")
        //Destroy the treasure and collect point(s)
        //The same strategy can be used to collect various treasures
        /*if (collision.gameObject.CompareTag("Treasure"))
        {
            Destroy(collision.gameObject);
            gameManagerScript.Collect(1);
        }*/
    }
}
