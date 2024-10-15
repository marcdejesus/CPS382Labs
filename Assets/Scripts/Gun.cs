using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 10f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect; //The effect that would happen at hit
    // Reference to AudioClip to play
    public AudioClip shootSFX;

    private float nextTimeToFire = 0f;//the time you have to wait for trigger next fire

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    void Shoot()
    {
        //play muzzle flash effect if set
        if (muzzleFlash)
        {
            muzzleFlash.Play();
        }
        // play sound effect if set
        if (shootSFX)
        {   
            // dynamically create a new gameObject with an AudioSource
            // this automatically destroys itself once the audio is done
            AudioSource.PlayClipAtPoint(shootSFX, gameObject.transform.position);
        }
        //Raycast
        RaycastHit hit;
        //Shoot out a ray from the camera position, forward, store result in the "hit" variable
        //If the ray hit something, it returns true
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            //Display information when a target is hit
            Debug.Log("Hit this guy: " + hit.transform.name);
            //If whoever got hit is tagged as Enemy
            if (hit.transform.tag == "Enemy")
            {
                //Get the enemy behavior from "hit"
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage();//What the damage is depends on the TargetBehavior which is the same as those defined in OnCollisionEnter
                }
                if (hit.rigidbody != null)//If the object has rigidbody (i.e., could have physical effect)
                {
                    //apply force to the target
                    hit.rigidbody.AddForce(hit.normal * impactForce);
                }
            }
            //Create effect at hit.point (i.e., the location that got hit at)
            if (impactEffect)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
        }
    }
}
