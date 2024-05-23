using UnityEngine;

public class AxeSpin : MonoBehaviour
{
    // The target object to spin towards
    public Transform destination;
    // Speed at which the object rotates
    [SerializeField]private float rotationSpeed;

    void Update()
    {
        if (isActiveAndEnabled)
        {
            // right now destination is not set anywhere
            if(destination != null)
            {
                LookAtTarget();
                Spin();
            }
        }
    }
    public void Spin()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    public void LookAtTarget()
    {
        transform.LookAt(destination);
    }
}
