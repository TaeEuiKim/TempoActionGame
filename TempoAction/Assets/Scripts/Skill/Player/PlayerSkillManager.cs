using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour, ISkillManager
{
    [field: SerializeField] public int MaxSkillSlot { get; protected set; }
    [SerializeField] private int MaxReserveSlot;

    [field: SerializeReference] public SkillSlot[] SkillSlots { get; protected set; }
    private Queue<ISkillRoot> reserveSlots;

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
        if (SkillSlots != null && SkillSlots?.Length != MaxSkillSlot)
        {
            var oldSlots = SkillSlots;
            SkillSlots = new PlayerSkillSlot[MaxSkillSlot];

            for (int i = 0; i < oldSlots.Length; i++)
            {
                if (i >= SkillSlots.Length) { continue; }

                SkillSlots[i] = oldSlots[i];
            }

            for (int i = oldSlots.Length; i < SkillSlots.Length; i++)
            {
                SkillSlots[i] = new PlayerSkillSlot();
            }
        }

        for (int i = 0; i < SkillSlots.Length; i++)
        {
            SkillSlots[i] = SkillSlots[i] as PlayerSkillSlot;
        }

        if (reserveSlots != null && reserveSlots?.Count != MaxReserveSlot)
        {
            var oldSlots = reserveSlots;
            reserveSlots = new Queue<ISkillRoot>(MaxReserveSlot);

            int oldSlotSize = oldSlots.Count;
            for (int i = 0; i < oldSlotSize; i++)
            {
                if (i >= MaxReserveSlot) { continue; }

                reserveSlots.Enqueue(oldSlots.Dequeue());
            }

            return;
        }

        reserveSlots = new Queue<ISkillRoot>(MaxReserveSlot);
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
        foreach (PlayerSkillSlot slot in SkillSlots)
        {
            slot.UseSkillKeyDown(this);
            if (slot.Skill is NormalSkill normalSkill)
            {
                normalSkill.UpdateTime(Time.deltaTime);
            }
        }

        if (interatedObject != null && Input.GetKeyDown(KeyCode.X))
        {
            AddSkill(interatedObject.GetSkill());
        }
    }

    public void AddSkill(ISkillRoot newSkill)
    {
        if(newSkill == null) { return; }

        // ��ų ���Կ��� �� �ڸ� Ž��
        for(int i = 0; i < SkillSlots.Length; i++)
        {
            // �� ���� ���� ���
            if (SkillSlots[i].Skill == null)
            {
                // ���
                SkillSlots[i].OnRemoved.AddListener(RemoveSkill);
                SkillSlots[i].SetSkill(newSkill);
                return;
            }
        }
        
        // �ڸ��� ���ٸ� ť(���� ��ų)�� ���
        reserveSlots.Enqueue(newSkill);
    }

    // ��ų ����
    private void RemoveSkill(ISkillRoot removedSkill)
    {
        if (removedSkill == null) { return; }

        // removedSkill ���� ����
        int index = -1;
        for (index = 0; index < SkillSlots.Length; index++)
        {
            if (SkillSlots[index].Skill == removedSkill)
            {
                SkillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                SkillSlots[index].RemoveSkill();
            }
        }

        // ���� ��ų ����
        if (reserveSlots.Count == 0) { return; }

        ISkillRoot nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }
}
