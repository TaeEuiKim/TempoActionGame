using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "ScriptableObjects/Buff Data/Lightning")]
public class LightningBuff : BuffData
{
    public override void Enter()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

    }

    public override void Stay()
    {
        BuffManager.Instance.RemoveBuff(Define.BuffInfo.HEAL);
    }

    public override void Exit()
    {
        Platform.Change(Define.BuffInfo.NONE);
    }

}