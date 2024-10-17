using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour, ISkillManager
{
    [field: SerializeField] public int MaxSkillSlot { get; protected set; }
    [field: SerializeField] public int MaxReserveSlot { get; protected set; }

    [field: SerializeReference] public SkillSlot[] SkillSlots { get; protected set; }
    public Queue<ISkillRoot> reserveSlots {  get; protected set; }

    private SkillObject interatedObject;
    private PlayerView _view;

    private bool isSkill = true;
    private float skillTimer = 0;

    /*// temp
    public Collider hitbox;
    public GameObject offingHitbox;
    public Collider offingHitbox2;
    public Transform target;
    [SerializeField] private GameObject[] effects; // 0: ready, 1: rush, 2: attack
    [HideInInspector]public GameObject[] instiatedEffects; // 0: ready, 1: rush, 2: attack
    [HideInInspector] public GameObject effectsParent;*/

    private void Start()
    {
        /*instiatedEffects = new GameObject[effects.Length];
        effectsParent = new GameObject("Effects");
        effectsParent.transform.parent = transform;
        effectsParent.transform.localPosition = Vector3.zero;
        for (int i = 0; i < effects.Length; i++)
        {
            instiatedEffects[i] = Instantiate(effects[i], effectsParent.transform);
            instiatedEffects[i].SetActive(false);
        }*/

        _view = GetComponent<PlayerView>();
        Initialize();
        StartCoroutine(SkillTimer());
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
        if (interatedObject != null)
        {
            interatedObject = skillObject;
        }
    }

    public void DeInteractObject()
    {
        interatedObject = null;
    }

    public void OnUpdate(CharacterBase characterBase)
    {
        foreach (PlayerSkillSlot slot in SkillSlots)
        {
            slot.UseSkillKeyDown(characterBase, isSkill);
            if (slot.Skill is NormalSkill normalSkill)
            {
                normalSkill.UpdateTime(Time.deltaTime);
            }
        }
    }

    public void AddSkill(ISkillRoot newSkill)
    {
        if(newSkill == null) { return; }

        // 스킬 슬롯에서 빈 자리 탐색
        for(int i = 0; i < SkillSlots.Length; i++)
        {
            // 빈 곳이 있을 경우
            if (SkillSlots[i].Skill == null)
            {
                // 등록
                SkillSlots[i].OnRemoved.AddListener(RemoveSkill);
                SkillSlots[i].SetSkill(newSkill);
                if (newSkill.GetSkillId() == 51)
                {
                    _view.ChangeMainSkillIcon(i, false);
                }

                return;
            }
        }
        
        // 자리가 없다면 큐(예비 스킬)에 등록
        reserveSlots.Enqueue(newSkill);
        if (newSkill.GetSkillId() == 51)
        {
            _view.ChangeSubSkillIcon(reserveSlots.Count - 1, false);
        }
    }

    public void LoadSkill(SkillSlot[] skillSlots, Queue<ISkillRoot> reserveSlots)
    {
        SkillSlots = skillSlots;
        for (int i = 0; i < SkillSlots.Length; i++)
        {
            SkillSlots[i].OnRemoved.AddListener(RemoveSkill);
        }
        this.reserveSlots = reserveSlots;
    }

    // 스킬 제거
    private void RemoveSkill(ISkillRoot removedSkill)
    {
        if (removedSkill == null) { return; }

        // removedSkill 제거 과정
        int index = -1;
        for (index = 0; index < SkillSlots.Length; index++)
        {
            if (SkillSlots[index].Skill == removedSkill)
            {
                SkillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                SkillSlots[index].RemoveSkill();

                if (removedSkill.GetSkillId() == 51)
                {
                    _view.ChangeMainSkillIcon(index, true);
                }
            }
        }

        // 예비 스킬 장착
        if (reserveSlots.Count == 0) { return; }

        _view.ChangeSubSkillIcon(reserveSlots.Count - 1, true);
        ISkillRoot nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }

    public void SetIsSkill(bool isSk)
    {
        isSkill = isSk;
    }

    private IEnumerator SkillTimer()
    {
        while (true)
        {
            if (isSkill == false)
            {
                skillTimer += Time.deltaTime;
                if (skillTimer > 1f)
                {
                    skillTimer = 0f;
                    isSkill = true;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
