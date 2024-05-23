using UnityEngine;

public class AxeSpin : MonoBehaviour
{


    // Speed at which the object rotates
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        if (isActiveAndEnabled)
        {
            Spin();
        }
    }
    public void Spin()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
