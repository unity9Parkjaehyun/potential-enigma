using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    // Á¡ÇÁ ±¸Çö 

    Rigidbody m_Rigidbody;
    public GameObject spring;
    public float jumpForce = 100f;
    public int jumpCount = 0;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        jumpCount++;

        if (jumpCount % 3 == 0)
        {

            jumpForce = 1000f;

        }
        else
        {
            jumpForce = 100;

        }
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.velocity = Vector3.zero;  //¶³¾îÁö´Â ÈûÀ» »ó¼â
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


    }
}
