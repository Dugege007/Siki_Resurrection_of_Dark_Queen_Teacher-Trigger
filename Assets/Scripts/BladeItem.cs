using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeItem : MonoBehaviour
{
    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController= other.GetComponent<PlayerController>();
            if (playerController.currentState==State.Blademan)
            {
                playerController.HasNewBlade();
                Destroy(gameObject);
            }
        }
    }
}
