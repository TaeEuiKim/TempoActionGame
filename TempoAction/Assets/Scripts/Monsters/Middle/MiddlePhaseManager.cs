using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

public class MiddlePhaseManager : MonoBehaviour
{
    [SerializeField] private MiddleMonster _monster;          // Gyeongchae
    [SerializeField] private MiddleMonster _monster2;         // Cheong

    [Header("패턴용 위치 포인트")]
    [SerializeField] public Define.MiddleMonsterPoint[] _monsterPointName;
    [SerializeField] public Transform[] _monsterPointTrans;
    public Dictionary<Define.MiddleMonsterPoint, Transform> _middlePoint = new Dictionary<Define.MiddleMonsterPoint, Transform>();

    [SerializeField] private Define.MiddlePhaseState _currentPhaseState = Define.MiddlePhaseState.NONE;
    private Dictionary<Define.MiddlePhaseState, Middle_PhaseState> _phaseStateStorage = new Dictionary<Define.MiddlePhaseState, Middle_PhaseState>();

    // 컷씬
    [Header("시작 컷씬 오브젝트")]
    [SerializeField] public UnityEngine.UI.Image[] startSprites;
    [Header("마무리 컷씬 오브젝트")]
    [SerializeField] public UnityEngine.UI.Image[] endSprites;
    [Header("시작 컷씬 UI")]
    [SerializeField] public GameObject startSceneUI;
    [Header("마무리 컷씬 UI")]
    [SerializeField] public GameObject endSceneUI;
    [Header("페이드 패널")]
    [SerializeField] public UnityEngine.UI.Image FadePanel;
    private bool isCutScene;
    private Coroutine cutSceneCoroutine;

    [Space]
    [Header("중간 보스 시작 UI")]
    [SerializeField] public GameObject smashImg;
    [Header("중간 보스 체력 UI")]
    [SerializeField] private GameObject[] hpNumUI;
    [SerializeField] private UnityEngine.UI.Image[] hpBarUI;
    [SerializeField] private UnityEngine.UI.Image[] hpIllusionBarUI;
    [Header("크레딧")]
    [SerializeField] public GameObject credit;
    [Header("KO")]
    [SerializeField] public GameObject KOPanel;

    private List<float> _targetHealthList = new List<float>();
    private int _targetHealthIndex = 0;
    [HideInInspector] public CameraController _cameraController;

    private WaitForSeconds waitOneForSeconds = new WaitForSeconds(1f);

    public MiddleMonster Monster { get => _monster; }
    public MiddleMonster Monster2 { get => _monster2; }
    public List<float> TargetHealthList { get => _targetHealthList; }
    public int TargetHealthIndex { get => _targetHealthIndex; set => _targetHealthIndex = value; }
    private float curHealth = 0;
    private int _count = 1;

    public int phase = 1;
    private bool isEffect;

    private void Awake()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _phaseStateStorage.Add(Define.MiddlePhaseState.START, new Middle_PhaseStart(this));
        _phaseStateStorage.Add(Define.MiddlePhaseState.PHASE1, new Middle_Phase1(this));
        _phaseStateStorage.Add(Define.MiddlePhaseState.PHASE2, new Middle_Phase2(this));
        _phaseStateStorage.Add(Define.MiddlePhaseState.FINISH, new Middle_PhaseEnd(this));

        for (int i = 0; i < _monsterPointName.Length; ++i)
        {
            _middlePoint.Add(_monsterPointName[i], _monsterPointTrans[i]);
        }
    }

    private void Start()
    {
        curHealth = Monster2.Stat.MaxHp;
        _targetHealthList.Add(Monster2.Stat.MaxHp * 0.8f);
        _targetHealthList.Add(Monster2.Stat.MaxHp * 0.6f);
        _targetHealthList.Add(Monster2.Stat.MaxHp * 0.4f);
        _targetHealthList.Add(Monster2.Stat.MaxHp * 0.2f);
        _targetHealthList.Add(0);

        _monster.middlePoint = _middlePoint;
        _monster2.middlePoint = _middlePoint;

        cutSceneCoroutine = StartCoroutine(CutSceneStart());
    }

    private void Update()
    {
        if (_currentPhaseState != Define.MiddlePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Stay();
        }

        if (isCutScene && PlayerInputManager.Instance.cancel)
        {
            PlayerInputManager.Instance.cancel = false;
            SkipCutScene();
        }
    }

    public void ChangeStageState(Define.MiddlePhaseState state)
    {
        if (_currentPhaseState != Define.MiddlePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Exit();
        }
        _currentPhaseState = state;
        _phaseStateStorage[_currentPhaseState]?.Enter();
    }

    public float GetHp()
    {
        return curHealth;
    }

    public void SetHp(float value)
    {
        curHealth -= value;
        if (curHealth <= 0)
        {
            Monster2.ChangeCurrentState(Define.MiddleMonsterState.DIE);
            return;
        }

        float n = 1 - ((-(300 * (_count - 1)) + (Monster2.Stat.MaxHp - curHealth)) / 300);

        if (n < 0)
        {
            StartCoroutine(UpdateHealthBar(0, _count));
            _count++;

            StartCoroutine(UpdateHealthBar(1 + n, _count));
            return;
        }

        StartCoroutine(UpdateHealthBar(n, _count));
    }

    private IEnumerator UpdateHealthBar(float value, int count)
    {
        float time = 0.02f;
        float fillAmount = value;

        if (fillAmount < 0 || value == 0)
        {
            fillAmount = 0;
        }

        StartCoroutine(UpdateIllusionBar(fillAmount, count));

        while (hpBarUI[hpBarUI.Length - count].fillAmount >= fillAmount)
        {
            hpBarUI[hpBarUI.Length - count].fillAmount -= time;

            yield return time;
        }

        hpBarUI[hpBarUI.Length - count].fillAmount = fillAmount;

        yield return null;
    }

    private IEnumerator UpdateIllusionBar(float value, int count)
    {
        float time = 0.02f;

        yield return new WaitForSeconds(0.05f);

        while (hpIllusionBarUI[hpIllusionBarUI.Length - count].fillAmount >= value)
        {
            hpIllusionBarUI[hpIllusionBarUI.Length - count].fillAmount -= time;

            yield return time;
        }

        hpIllusionBarUI[hpIllusionBarUI.Length - count].fillAmount = value;

        yield return null;
    }

    private IEnumerator CutSceneStart()
    {
        float alpha = 0;

        if (Monster.Player.GetComponent<Player>().View.UiEffect.activeSelf)
        {
            Monster.Player.GetComponent<Player>().View.UiEffect.SetActive(false);
            isEffect = true;
        }

        isCutScene = true;
        SetPlayerControll(true);

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < startSprites.Length; i++)
        {
            while (startSprites[i].color.a < 1)
            {
                alpha += Time.fixedDeltaTime;
                startSprites[i].color = new Color(1, 1, 1, alpha);

                yield return null;
            }

            alpha = 0;

            yield return waitOneForSeconds;
        }

        startSceneUI.SetActive(false);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        smashImg.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        smashImg.SetActive(false);
        
        float alpha = 1;
        isCutScene = false;

        SetPlayerControll(false);

        while (FadePanel.color.a > 0)
        {
            alpha -= Time.fixedDeltaTime;
            FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        float speed = 13f;

        yield return waitOneForSeconds;

        ChangeStageState(Define.MiddlePhaseState.START);
        if (isEffect)
        {
            Monster.Player.GetComponent<Player>().View.UiEffect.SetActive(true);
        }

        yield return null;
    }

    public void SkipCutScene()
    {
        StopCoroutine(cutSceneCoroutine);
        startSceneUI.SetActive(false);

        StartCoroutine(FadeOut());
    }

    private void SetPlayerControll(bool isKnockBack)
    {
        Monster.Player.GetComponent<Player>().PlayerSt.IsKnockedBack = isKnockBack;
    }
}
