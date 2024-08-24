using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float TotalDamage { get; set; }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print(TotalDamage);
            other.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }
}
