using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallPartBehaviour : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startRot;
    float startXRot;
    public bool broken;
    Rigidbody rb;
    private float breakForce = 4f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation.eulerAngles;
        startXRot = transform.rotation.eulerAngles.x;
    }
    public void BreakPart(Vector3 direction){
        // then, unfreeze the rigidbodys position and x rotation
        UnFreezeRigidbody();
        // launch the rigidbody with addForce (towards the base, not outside)(inside is -z direction)
        rb.AddForce(direction * breakForce, ForceMode.Impulse);
        // freeze it back after two seconds
        StartCoroutine(FreezeRigidbody(rb));
        // finish break
        broken = true;
    }
    IEnumerator FreezeRigidbody(Rigidbody rb){
        yield return new WaitForSeconds(2);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void UnFreezeRigidbody(){
        // Remove position constraints on x, y, z axes
        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;

        // Remove rotation constraint on x axis
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
    }
    public void RepairPart(){
        // use stored location and float values to move back to original position
        // unfreeze the rigidbody
        UnFreezeRigidbody();
        // move part to that location with DOmove and Dorotate. Freeze after done.
        transform.DOMove(startPos, 2).SetEase(Ease.OutQuad).OnComplete(() => rb.constraints = RigidbodyConstraints.FreezeAll);
        Vector3 rot = new Vector3(startXRot, transform.rotation.y, transform.rotation.z);
        transform.DORotate(startRot, 2).SetEase(Ease.OutQuad).OnComplete(() => rb.constraints = RigidbodyConstraints.FreezeAll);
        // finish repair
        broken = false;
    }
}
