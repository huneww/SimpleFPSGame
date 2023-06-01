using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();

        rigid.AddForce(Vector3.up * 100, ForceMode.Impulse);
    }
}
