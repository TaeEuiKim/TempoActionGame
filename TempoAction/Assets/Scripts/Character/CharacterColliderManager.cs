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

    [Tooltip("Collider Dictionary에 들어간 콜라이더 중 Body 충돌을 담당하는 메인 콜라이더")]
    [SerializeField] private Collider mainCollider;
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

    public float GetHalfSizeForMain(Vector3 axis)
    {
        axis.Normalize();

        Vector3 sizeVector = Vector3.zero;

        if(mainCollider != null)
        {
            sizeVector = mainCollider.bounds.size;
        }

        float sizeX = sizeVector.x * axis.x;
        float sizeY = sizeVector.y * axis.y;
        float sizeZ = sizeVector.z * axis.z;
        Vector3 size = new Vector3(sizeX, sizeY, sizeZ);

        return size.magnitude * 0.5f;
    }
}
