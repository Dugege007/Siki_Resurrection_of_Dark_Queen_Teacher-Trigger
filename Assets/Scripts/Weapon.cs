using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public PlayerController owner;
    public GameObject effectGO;

    public bool needDestroy;
    public float destroyTime;
    public int damageValue;

    private void Start()
    {
        if (destroyTime > 0)
        {
            Destroy(gameObject, 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Debug.Log(" ‹µΩ…À∫¶");
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.CanExecute())
            {
                int random = Random.Range(1, 5);
                owner.Execute(random);
                playerController.BeExecuted(random, owner.transform);
            }
            else
            {
                playerController.TakeDamage(damageValue, other.ClosestPoint(transform.position));
            }

            if (needDestroy)
            {
                if (effectGO != null)
                    Instantiate(effectGO, other.ClosestPoint(transform.position), Quaternion.identity);
                Destroy(gameObject, destroyTime);
            }
        }
    }
}
