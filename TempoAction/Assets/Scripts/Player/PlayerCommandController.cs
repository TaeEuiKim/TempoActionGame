using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCommandController
{
    private Player _player;
    private SkillCommand _skillCommand;

    public PlayerCommandController(Player player, SkillCommand command)
    {
        _player = player;
        _skillCommand = command;
    }

    public void StartCommandTime(float checkTime, int skillid)
    {
        _player.Controller.isMove = false;
        _player.Controller.isJump = false;

        PlayerInputManager.Instance.isCommand = true;

        CoroutineRunner.Instance.StartCoroutine(CheckCommandTime(checkTime, skillid));
    }

    private IEnumerator CheckCommandTime(float checkTime, int skillid)
    {
        float checkTimer = 0f;
        while (checkTimer < checkTime)
        {
            if (CheckAttackCommand(PlayerInputManager.Instance.GetCommandKey(), skillid))
            {
                Debug.LogError("1");
                _player.Controller.isMove = true;
                _player.Controller.isJump = true;
                yield break;
            }

            checkTimer += Time.deltaTime;
            yield return null;
        }

        _player.Controller.isMove = true;
        _player.Controller.isJump = true;
        yield return null;
    }

    private bool CheckAttackCommand(List<KeyCode> keyList, int skillid)
    {
        // 이전 스킬이 연계가 가능한 스킬인지, skillid와 일치하는 스킬 아이디인지 검사, 입력된 키가 전부 같으면 해당 스킬아이디 반환
        //var matchingSkillId = _skillCommand.commandDatas
        //    .Where(cd => cd.PossibleSkillId.Contains(skillid))
        //    .Where(cd => cd.KeyCodes.Length == keyList.Count &&
        //                 keyList.Zip(cd.KeyCodes, (key1, key2) =>
        //                 {
        //                     KeyCode tempKey = key2;
        //                     if (_player.IsLeftDirection() && key2 == KeyCode.RightArrow)
        //                     {
        //                         tempKey = KeyCode.LeftArrow;
        //                     }
        //                     return key1 == tempKey;
        //                 }).All(isEqual => isEqual))
        //    .Select(cd => cd.SkillId)
        //    .FirstOrDefault(CheckUseSkill);

        var matchingSkillId = _skillCommand.commandDatas
            .Where(cd => cd.PossibleSkillId.Contains(skillid))
            .Where(cd => ContainsSubsequence(keyList, cd.KeyCodes))
            .Select(cd => cd.SkillId)
            .FirstOrDefault(CheckUseSkill);

        var skillId = _skillCommand.commandDatas;

        if (matchingSkillId != default)
        {
            _player.Ani.SetInteger("CommandCount", matchingSkillId);
            return true;
        }

        return false;
    }

    private bool ContainsSubsequence(List<KeyCode> source, KeyCode[] target)
    {
        if (target.Length == 0 || source.Count < target.Length)
            return false;

        bool isLeftDir = _player.IsLeftDirection();
        List<KeyCode> adjustedTarget = target.Select(key => (isLeftDir && key == KeyCode.RightArrow) ? KeyCode.LeftArrow : key).ToList();

        return Enumerable.Range(0, source.Count - adjustedTarget.Count + 1)
            .Any(i => source.Skip(i).Take(adjustedTarget.Count + 1)
            .SequenceEqual(adjustedTarget));
    }

    private bool CheckUseSkill(int skillid)
    {
        PlayerInputManager.Instance.ResetCommandKey();
        int count = _player.Ani.GetInteger("CommandCount");
        switch (count)
        {
            case 0:
                if (skillid == 1)
                {
                    TestSound.Instance.PlaySound("Smash1");
                    TestSound.Instance.PlaySound("Smash1_Voice");
                    return true;
                }
                if (skillid == 4)
                {
                    TestSound.Instance.PlaySound("Smash2");
                    TestSound.Instance.PlaySound("Smash2_Voice");
                    return true;
                }
                break;
            case 1:
                if (skillid == count + 1)
                {
                    if (skillid == 2)
                    {
                        TestSound.Instance.PlaySound("Smash1_2");
                        TestSound.Instance.PlaySound("Smash1_2_Voice");
                    }
                    return true;
                }
                break;
            case 2: 
            case 4:
                if (skillid == count + 1)
                {
                    UseSkill(skillid);
                    return true;
                }
                break;
            default:
                break;
        }

        return false;
    }

    private void UseSkill(int skillid)
    {
        switch (skillid)
        {
            case 3:
                if (_player.SkillManager.SkillSlots[0].Skill != null)
                {
                    _player.Ani.SetBool("IsCommandSkill", true);
                    _player.SkillManager.SkillSlots[0].UseSkillInstant(_player);
                    _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
                }
                break;
            case 5:
                if (_player.SkillManager.SkillSlots[1].Skill != null)
                {
                    _player.Ani.SetBool("IsCommandSkill", true);
                    _player.SkillManager.SkillSlots[1].UseSkillInstant(_player);
                    _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
                }
                break;
            default:
                break;
        }
    }
}
