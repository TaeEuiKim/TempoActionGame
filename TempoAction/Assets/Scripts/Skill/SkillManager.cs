using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private int MaxSkillSlot;
    [SerializeField] private int MaxReserveSlot;

    [SerializeField] private SkillSlot[] skillSlots;
    private Queue<SkillBase> reserveSlots;

    private SkillObject interatedObject;

    // temp
    public Collider hitbox;
    public GameObject offingHitbox;
    public Collider offingHitbox2;
    public Transform target;
    [SerializeField] private GameObject[] effects; // 0: ready, 1: rush, 2: attack
    [HideInInspector]public GameObject[] instiatedEffects; // 0: ready, 1: rush, 2: attack
    [HideInInspector] public GameObject effectsParent;

    private void Start()
    {
        instiatedEffects = new GameObject[effects.Length];
        effectsParent = new GameObject("Effects");
        effectsParent.transform.parent = transform;
        effectsParent.transform.localPosition = Vector3.zero;
        for (int i = 0; i < effects.Length; i++)
        {
            instiatedEffects[i] = Instantiate(effects[i], effectsParent.transform);
            instiatedEffects[i].SetActive(false);
        }

        Initialize();
    }

    public void Initialize()
    {
        if (skillSlots != null && skillSlots?.Length != MaxSkillSlot)
        {
            var oldSlots = skillSlots;
            skillSlots = new SkillSlot[MaxSkillSlot];

            for(int i = 0; i < oldSlots.Length; i++)
            {
                if(i >= skillSlots.Length) { continue; }

                skillSlots[i] = oldSlots[i];
            }
        }

        if (reserveSlots != null && reserveSlots?.Count != MaxReserveSlot)
        {
            var oldSlots = reserveSlots;
            reserveSlots = new Queue<SkillBase>(MaxReserveSlot);

            int oldSlotSize = oldSlots.Count;
            for (int i = 0; i < oldSlotSize; i++)
            {
                if (i >= MaxReserveSlot) { continue; }

                reserveSlots.Enqueue(oldSlots.Dequeue());
            }

            return;
        }

        reserveSlots = new Queue<SkillBase>(MaxReserveSlot); 
    }

    public void InteractObject(SkillObject skillObject)
    {
        interatedObject = skillObject;
    }

    public void DeInteractObject()
    {
        interatedObject = null;
    }

    private void Update()
    {
       Debug.Log(GetComponent<Rigidbody>().velocity);

        foreach (var slot in skillSlots)
        {
            slot.UseSkillKeyDown(this);
            if (slot.skill is NormalSkill normalSkill)
            {
                normalSkill.UpdateTime(Time.deltaTime);
            }
        }

        if (interatedObject != null && Input.GetKeyDown(KeyCode.X))
        {
            AddSkill(interatedObject.GetSkill());
        }
    }

    public void AddSkill(SkillBase newSkill)
    {
        if(newSkill == null) { return; }

        // ��ų ���Կ��� �� �ڸ� Ž��
        for(int i = 0; i < skillSlots.Length; i++)
        {
            // �� ���� ���� ���
            if (skillSlots[i].skill == null)
            {
                // ���
                skillSlots[i].OnRemoved.AddListener(RemoveSkill);
                skillSlots[i].SetSkill(newSkill);
                return;
            }
        }
        
        // �ڸ��� ���ٸ� ť(���� ��ų)�� ���
        reserveSlots.Enqueue(newSkill);
    }

    // ��ų ����
    private void RemoveSkill(SkillBase removedSkill)
    {
        if (removedSkill == null) { return; }

        // removedSkill ���� ����
        int index = -1;
        for (index = 0; index < skillSlots.Length; index++)
        {
            if (skillSlots[index].skill == removedSkill)
            {
                skillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                skillSlots[index].RemoveSkill();
            }
        }

        // ���� ��ų ����
        if (reserveSlots.Count == 0) { return; }
        
        SkillBase nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }
}
