using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	// target impact on game
	public int scoreAmount = 0;
	public float timeAmount = 0.0f;

    public float health = 50f;//the initial health of the target
    public float healthDamageAtHit = 10f;

	// explosion when hit?
	public GameObject explosionPrefab;
    /** This is the one being called by the raycast hit in the Gun script
     *  
     * */
    public void TakeDamage() {
        Debug.Log("Target Hit!");
        health -= healthDamageAtHit; //apply health damage
        if (health <= 0f)//Die!
        {
			if (explosionPrefab)
			{
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate(explosionPrefab, transform.position, transform.rotation);
			}
			//destroy the target
			Destroy(gameObject);

            //  if there is a game manager 
            if (GameManager.gm)
            {
                //exit if the game is over
                if (GameManager.gm.gameIsOver)
                    return;
                //if game is not over, make adjustments based on target properties
                GameManager.gm.targetHit(scoreAmount, timeAmount);
                Debug.Log("Scored " + scoreAmount);
            }
 
        }
    }

	// when collided with another gameObject
    // This is the original collider-based collision detection
	void OnCollisionEnter(Collision newCollision)
	{
		//What would you do?
	}
}
