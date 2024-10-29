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