#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelDesignHelperPosition : MonoBehaviour
{
    [SerializeField] float YAxisPos;

    void Update()
    {
        if (!Application.isPlaying)
        {
            transform.position = new(transform.position.x, YAxisPos, transform.position.z);
        }
    }
}
#endif