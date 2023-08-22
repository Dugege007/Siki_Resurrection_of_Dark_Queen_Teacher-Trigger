using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public float destoryTime;

    private void Start()
    {
        Destroy(gameObject, 5);
    }
}
