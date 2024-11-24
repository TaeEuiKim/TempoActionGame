using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestSound : Singleton<TestSound>
{
    [SerializeField] private Clips[] soundSources;
    [SerializeField] private AudioSource[] sources;
    [SerializeField] private string sceneName;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Jump":
                sources[0].clip = soundSources[0].audioClips[0];
                sources[0].Play();
                sources[1].clip = soundSources[0].audioClips[1];
                sources[1].Play();
                break;
            case "DoubleJump":
                sources[2].clip = soundSources[0].audioClips[2];
                sources[2].Play();
                sources[3].clip = soundSources[0].audioClips[3];
                sources[3].Play();
                break;
            case "Start":
                sources[4].clip = soundSources[1].audioClips[0];
                sources[4].Play();
                break;
            case "Skill1":
                sources[5].clip = soundSources[2].audioClips[0];
                sources[5].Play();
                break;
            case "Skill1_Effect":
                sources[6].clip = soundSources[2].audioClips[1];
                sources[6].Play();
                break;
            case "Skill1_Hit":
                sources[7].clip = soundSources[2].audioClips[2];
                sources[7].Play();
                break;
            case "Skill1_Voice":
                sources[8].clip = soundSources[2].audioClips[3];
                sources[8].Play();
                break;
            case "Skill2":
                sources[9].clip = soundSources[3].audioClips[0];
                sources[9].Play();
                break;
            case "Skill2_Effect":
                sources[10].clip = soundSources[3].audioClips[1];
                sources[10].Play();
                break;
            case "Skill2_Hit":
                sources[11].clip = soundSources[3].audioClips[2];
                sources[11].Play();
                break;
            case "Skill2_Voice":
                sources[12].clip = soundSources[3].audioClips[3];
                sources[12].Play();
                break;
            case "Smash1":
                sources[13].clip = soundSources[4].audioClips[0];
                sources[13].Play();
                break;
            case "Smash1_Hit":
                sources[14].clip = soundSources[4].audioClips[2];
                sources[14].Play();
                break;
            case "Smash1_Voice":
                sources[15].clip = soundSources[4].audioClips[4];
                sources[15].Play();
                break;
            case "Smash1_2":
                sources[16].clip = soundSources[4].audioClips[1];
                sources[16].Play();
                break;
            case "Smash1_2_Hit":
                sources[17].clip = soundSources[4].audioClips[3];
                sources[17].Play();
                break;
            case "Smash1_2_Voice":
                sources[18].clip = soundSources[4].audioClips[5];
                sources[18].Play();
                break;
            case "Smash2":
                sources[19].clip = soundSources[5].audioClips[0];
                sources[19].Play();
                break;
            case "Smash2_Hit":
                sources[20].clip = soundSources[5].audioClips[1];
                sources[20].Play();
                break;
            case "Smash2_Voice":
                sources[21].clip = soundSources[5].audioClips[2];
                sources[21].Play();
                break;
            case "TitleBGM":
                sources[22].clip = soundSources[1].audioClips[1];
                sources[22].Play();
                break;
            case "UndergroundBGM":
                sources[23].clip = soundSources[1].audioClips[2];
                sources[23].Play();
                break;
            case "MiddleBGM":
                sources[24].clip = soundSources[1].audioClips[3];
                sources[24].Play();
                break;
            case "NormalMonster1_Attack1":
                sources[25].clip = soundSources[6].audioClips[0];
                sources[25].Play();
                break;
            case "NormalMonster1_Attack2":
                sources[26].clip = soundSources[6].audioClips[2];
                sources[26].Play();
                break;
            case "NormalMonster1_Attack1_Hit":
                sources[27].clip = soundSources[6].audioClips[1];
                sources[27].Play();
                break;
            case "NormalMonster1_Attack2_Hit":
                sources[28].clip = soundSources[6].audioClips[3];
                sources[28].Play();
                break;
            case "NormalMonster1_Beat1":
                sources[29].clip = soundSources[6].audioClips[4];
                sources[29].Play();
                break;
            case "NormalMonster1_Beat2":
                sources[30].clip = soundSources[6].audioClips[5];
                sources[30].Play();
                break;
            case "NormalMonster1_Skill":
                sources[31].clip = soundSources[6].audioClips[6];
                sources[31].Play();
                break;
            case "NormalMonster1_SkillHit":
                sources[32].clip = soundSources[6].audioClips[7];
                sources[32].Play();
                break;
            case "NormalMonster2_AttackHit":
                sources[33].clip = soundSources[7].audioClips[0];
                sources[33].Play();
                break;
            case "NormalMonster2_Beat1":
                sources[34].clip = soundSources[7].audioClips[1];
                sources[34].Play();
                break;
            case "NormalMonster2_Beat2":
                sources[35].clip = soundSources[7].audioClips[2];
                sources[35].Play();
                break;
            case "NormalMonster2_Skill":
                sources[36].clip = soundSources[7].audioClips[3];
                sources[36].Play();
                break;
            case "NormalMonster3_Beat1":
                sources[37].clip = soundSources[8].audioClips[0];
                sources[37].Play();
                break;
            case "NormalMonster3_Beat2":
                sources[38].clip = soundSources[8].audioClips[1];
                sources[38].Play();
                break;
            case "NormalMonster3_Dead1":
                sources[39].clip = soundSources[8].audioClips[2];
                sources[39].Play();
                break;
            case "NormalMonster3_Dead2":
                sources[40].clip = soundSources[8].audioClips[3];
                sources[40].Play();
                break;
            case "NormalMonster3_Hit":
                sources[41].clip = soundSources[8].audioClips[4];
                sources[41].Play();
                break;
            case "NormalMonster3_HitVoice1":
                sources[42].clip = soundSources[8].audioClips[5];
                sources[42].Play();
                break;
            case "NormalMonster3_HitVoice2":
                sources[43].clip = soundSources[8].audioClips[6];
                sources[43].Play();
                break;
            case "NormalMonster3_Attack1":
                sources[44].clip = soundSources[8].audioClips[7];
                sources[44].Play();
                break;
            case "NormalMonster3_Attack2":
                sources[45].clip = soundSources[8].audioClips[8];
                sources[45].Play();
                break;
            case "Heal":
                sources[46].clip = soundSources[9].audioClips[0];
                sources[46].Play();
                break;
            case "Homerun_Dash":
                sources[47].clip = soundSources[10].audioClips[0];
                sources[47].Play();
                break;
            case "Homerun_Hit":
                sources[48].clip = soundSources[10].audioClips[1];
                sources[48].Play();
                break;
            case "Homerun_Swing":
                sources[49].clip = soundSources[10].audioClips[2];
                sources[49].Play();
                break;
            case "Homerun_Voice":
                sources[50].clip = soundSources[10].audioClips[3];
                sources[50].Play();
                break;
            case "Longjump_Fall":
                sources[51].clip = soundSources[10].audioClips[4];
                sources[51].Play();
                break;
            case "Longjump_Jump":
                sources[52].clip = soundSources[10].audioClips[5];
                sources[52].Play();
                break;
            case "Longjump_Kang":
                sources[53].clip = soundSources[10].audioClips[6];
                sources[53].Play();
                break;
            case "Longjump_Voice":
                sources[54].clip = soundSources[10].audioClips[7];
                sources[54].Play();
                break;
            case "ChungMove1":
                sources[55].clip = soundSources[10].audioClips[8];
                sources[55].Play();
                break;
            case "ChungMove2":
                sources[56].clip = soundSources[10].audioClips[9];
                sources[56].Play();
                break;
            case "Takedown_1":
                sources[57].clip = soundSources[10].audioClips[10];
                sources[57].Play();
                break;
            case "Takedown_1Voice":
                sources[58].clip = soundSources[10].audioClips[11];
                sources[58].Play();
                break;
            case "Takedown_2":
                sources[59].clip = soundSources[10].audioClips[12];
                sources[59].Play();
                break;
            case "Takedown_2Voice":
                sources[60].clip = soundSources[10].audioClips[13];
                sources[60].Play();
                break;
            case "Takedown_sign":
                sources[61].clip = soundSources[10].audioClips[14];
                sources[61].Play();
                break;
            case "Takedown_signVoice":
                sources[62].clip = soundSources[10].audioClips[15];
                sources[62].Play();
                break;
            case "Takedown_3":
                sources[63].clip = soundSources[10].audioClips[16];
                sources[63].Play();
                break;
            case "Takedown_3Voice":
                sources[64].clip = soundSources[10].audioClips[17];
                sources[64].Play();
                break;
            case "AimAttack_Guid":
                sources[65].clip = soundSources[11].audioClips[0];
                sources[65].Play();
                break;
            case "AimAttack_Voice":
                sources[66].clip = soundSources[11].audioClips[1];
                sources[66].Play();
                break;
            case "AimAttack_Boom":
                sources[67].clip = soundSources[11].audioClips[2];
                sources[67].Play();
                break;
            case "AimAttack_Firing":
                sources[68].clip = soundSources[11].audioClips[3];
                sources[68].Play();
                break;
            case "AimAttack_Flying":
                sources[69].clip = soundSources[11].audioClips[4];
                sources[69].Play();
                break;
            case "AimAttack_Ready":
                sources[70].clip = soundSources[11].audioClips[5];
                sources[70].Play();
                break;
            case "Shelling_Boom":
                sources[71].clip = soundSources[11].audioClips[6];
                sources[71].Play();
                break;
            case "Shelling_Droping":
                sources[72].clip = soundSources[11].audioClips[7];
                sources[72].Play();
                break;
            case "Shelling_Firing":
                sources[73].clip = soundSources[11].audioClips[8];
                sources[73].Play();
                break;
            case "Shelling_Voice":
                sources[74].clip = soundSources[11].audioClips[9];
                sources[74].Play();
                break;
            case "Swing":
                sources[75].clip = soundSources[11].audioClips[10];
                sources[75].Play();
                break;
            case "Step":
                sources[76].clip = soundSources[11].audioClips[11];
                sources[76].Play();
                break;
            case "SwingHit":
                sources[77].clip = soundSources[11].audioClips[12];
                sources[77].Play();
                break;
            case "JumpBoss":
                sources[78].clip = soundSources[11].audioClips[13];
                sources[78].Play();
                break;
            case "Launch":
                sources[79].clip = soundSources[11].audioClips[14];
                sources[79].Play();
                break;
            case "OniFire":
                sources[80].clip = soundSources[12].audioClips[0];
                sources[80].Play();
                break;
            case "SmashHit1":
                sources[81].clip = soundSources[12].audioClips[1];
                sources[81].Play();
                break;
            case "SmashHit2":
                sources[82].clip = soundSources[12].audioClips[2];
                sources[82].Play();
                break;
            case "SmashSwing":
                sources[83].clip = soundSources[12].audioClips[3];
                sources[83].Play();
                break;
            case "Backdash":
                sources[84].clip = soundSources[12].audioClips[4];
                sources[84].Play();
                break;
            case "Frontdash":
                sources[85].clip = soundSources[12].audioClips[5];
                sources[85].Play();
                break;
            case "RushHit":
                sources[86].clip = soundSources[12].audioClips[6];
                sources[86].Play();
                break;
            case "RushHit2":
                sources[87].clip = soundSources[12].audioClips[7];
                sources[87].Play();
                break;
            case "Posing":
                sources[88].clip = soundSources[12].audioClips[8];
                sources[88].Play();
                break;
            case "Skill3_Final":
                sources[89].clip = soundSources[13].audioClips[0];
                sources[89].Play();
                break;
            case "ParryingReady":
                sources[90].clip = soundSources[13].audioClips[1];
                sources[90].Play();
                break;
            case "UltimateOn":
                sources[91].clip = soundSources[13].audioClips[2];
                sources[91].Play();
                break;
            case "RushFinishVoice":
                sources[92].clip = soundSources[12].audioClips[9];
                sources[92].Play();
                break;
            case "RushFinishHit":
                sources[93].clip = soundSources[12].audioClips[10];
                sources[93].Play();
                break;
            default:
                break;
        }
    }

    public void StopBGMSound(string name)
    {
        switch (name)
        {
            case "TitleBGM":
                sources[22].Stop();
                break;
            case "UndergroundBGM":
                sources[23].Stop();
                break;
            case "MiddleBGM":
                sources[24].Stop();
                break;
            default:
                break;
        }
    }
}

[Serializable]
public class Clips
{
    public string clipName;
    public AudioClip[] audioClips;
}