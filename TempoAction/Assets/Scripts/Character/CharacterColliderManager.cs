using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterColliderManager
{
    [Serializable]
    private class ColliderBundle
    {
        [field: SerializeField] public Define.ColliderType Key {  get; private set; }
        [field: SerializeField] public List<Collider> Colliders { get; set; }

        public void SetActiveColliders(bool isActive)
        {
            foreach(var collider in Colliders)
            {
                collider.enabled = isActive;
            }
        }
    }

    [SerializeField] private List<ColliderBundle> colliders = new List<ColliderBundle>();
    private Dictionary<Define.ColliderType, ColliderBundle> colliderDictionary = new Dictionary<Define.ColliderType, ColliderBundle>();

    public void Initialize()
    {
        colliderDictionary.Clear();

        foreach(var bundle in colliders)
        {
            if(colliderDictionary.ContainsKey(bundle.Key)) { continue; }

            colliderDictionary[bundle.Key] = bundle;
        }
    }

    public void SetActiveCollider(bool isActive, Define.ColliderType colliderType)
    {
        if (!colliderDictionary.ContainsKey(colliderType)) { return; }

        colliderDictionary[colliderType].SetActiveColliders(isActive);
    }
}
