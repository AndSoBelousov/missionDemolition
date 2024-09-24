using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    private void Start()
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        if (_rb != null) _rb.Sleep  ();
    }
}
