using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MiddlePhaseManager : MonoBehaviour
{
    [SerializeField] private MiddleMonster _monster;          // Gyeongchae
    [SerializeField] private MiddleMonster _monster2;         // Cheong

    [Header("���Ͽ� ��ġ ����Ʈ")]
    [SerializeField] public Define.MiddleMonsterPoint[] _monsterPointName;
    [SerializeField] public Transform[] _monsterPointTrans;
    public Dictionary<Define.MiddleMonsterPoint, Transform> _middlePoint = new Dictionary<Define.MiddleMonsterPoint, Transform>();

    [SerializeField] private Define.MiddlePhaseState _currentPhaseState = Define.MiddlePhaseState.NONE;
    private Dictionary<Define.MiddlePhaseState, Middle_PhaseState> _phaseStateStorage = new Dictionary<Define.MiddlePhaseState, Middle_PhaseState>();

    // �ƾ�
    [Header("���� �ƾ� ������Ʈ")]
    [SerializeField] public UnityEngine.UI.Image[] startSprites;
    [Header("������ �ƾ� ������Ʈ")]
    [SerializeField] public UnityEngine.UI.Image[] endSprites;
    [Header("���� �ƾ� UI")]
    [SerializeField] public GameObject startSceneUI;
    [Header("������ �ƾ� UI")]
    [SerializeField] public GameObject endSceneUI;
    [Header("���̵� �г�")]
    [SerializeField] public UnityEngine.UI.Image FadePanel;
    private bool isCutScene;

    private List<float> _targetHealthList = new List<float>();
    private int _targetHealthIndex = 0;
    [HideInInspector] public CameraController _cameraController;

    public MiddleMonster Monster { get => _monster; }
    public MiddleMonster Monster2 { get => _monster2; }
    public List<float> TargetHealthList { get => _targetHealthList; }
    public int TargetHealthIndex { get => _targetHealthIndex; set => _targetHealthIndex = value; }

    private void Awake()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _phaseStateStorage.Add(Define.MiddlePhaseState.START, new Middle_PhaseStart(this));
        _phaseStateStorage.Add(Define.MiddlePhaseState.PHASE1, new Middle_Phase1(this));
        _phaseStateStorage.Add(Define.MiddlePhaseState.FINISH, new Middle_PhaseEnd(this));

        for (int i = 0; i < _monsterPointName.Length; ++i)
        {
            _middlePoint.Add(_monsterPointName[i], _monsterPointTrans[i]);
        }
    }

    private void Start()
    {
        _targetHealthList.Add(Monster2.Stat.MaxHp * 0.5f);
        _targetHealthList.Add(0);

        _monster.middlePoint = _middlePoint;
        _monster2.middlePoint = _middlePoint;

        //ChangeStageState(Define.MiddlePhaseState.START);

        StartCoroutine(CutSceneStart());
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
            StopCoroutine(CutSceneStart());
            startSceneUI.SetActive(false);

            StartCoroutine(FadeOut());
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

    private IEnumerator CutSceneStart()
    {
        float alpha = 0;
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

            yield return new WaitForSeconds(1f);
        }

        startSceneUI.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1;
        isCutScene = false;

        SetPlayerControll(false);

        while (FadePanel.color.a > 0)
        {
            alpha -= Time.fixedDeltaTime;
            FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        ChangeStageState(Define.MiddlePhaseState.START);

        yield return null;
    }

    private void SetPlayerControll(bool isKnockBack)
    {
        Monster.Player.GetComponent<Player>().PlayerSt.IsKnockedBack = isKnockBack;
    }
}
