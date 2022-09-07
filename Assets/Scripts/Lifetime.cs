using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float lifetimeSeconds = 5f;
    void Start()
    {
        StartCoroutine(DestroyAfterWait());        
    }

    IEnumerator DestroyAfterWait()
    {
        yield return new WaitForSeconds(Mathf.Max(0, lifetimeSeconds));
        Destroy(gameObject);
    }
}
