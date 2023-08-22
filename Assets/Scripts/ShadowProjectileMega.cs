using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//°µÓ°Ä§·¨Çò
public class ShadowProjectileMega : MonoBehaviour
{
    public float moveSpeed;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }
}
