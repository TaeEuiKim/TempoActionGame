using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : BaseObject
{
    [Header("Èú·®")]
    [SerializeField] private float healAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Player>().Heal(healAmount);
            ObjectPool.Instance.Remove(this.gameObject);
        }
    }
}
