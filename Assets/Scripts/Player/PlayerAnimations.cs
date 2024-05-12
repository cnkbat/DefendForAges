using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimations : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator anim;
    public Vector3 direction;
    public Vector3 moveDirection;
    public Vector2 originalVector, directionVector;
    public Vector2 projectionVector, perpendicularVector;
    float vx, vz;
    void Update()
    {
        direction = this.transform.forward; // z and x 1 to -1
        Animating();
    }

    void Animating()
    {
        // get the direction of the player. DONE => direction
        // get the movement direction of player.
        moveDirection = playerMovement.GetDirection();
        // separate the movement into two directions that combine into the main direction. These two directions will be parallel and perpendicular to the main direction.
        
        originalVector = new Vector2(moveDirection.x, moveDirection.z);
        directionVector = new Vector2(direction.x, direction.z);
        //float projectionScalar = Vector2.Dot(originalVector, directionVector) / Vector2.Dot(directionVector, directionVector);
        float projectionScalar = Vector2.Dot(originalVector, directionVector);

        projectionVector = projectionScalar * directionVector;
        perpendicularVector = originalVector - projectionVector;



        // set Vx to perpendicular direction and Vz to parallel direction.

        vx = perpendicularVector.magnitude;
        vz = projectionVector.magnitude;
        Debug.Log("Vz: " + vz + " Vx: " + vx);
        anim.SetFloat("Vx", vx);
        anim.SetFloat("Vz", vz);
        //anim.SetFloat("Vx", vx, 0.05f, Time.deltaTime);
        //anim.SetFloat("Vz", vz, 0.05f, Time.deltaTime);
    }
}
