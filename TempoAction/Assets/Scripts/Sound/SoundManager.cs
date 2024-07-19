using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
	// BGM용 오디오 소스
	private AudioSource _bgmSource;

	// SFX용 오디오 소스 풀
	private List<AudioSource> _sfxSources;
	private int _poolSize = 10; // 풀의 크기

	// 반복 재생용 오디오 소스
	private Dictionary<AudioClip, AudioSource> _loopingSFXSources;

	[SerializeField] private AudioMixer _audioMixer;

	[SerializeField] private List<AudioClip> _bgmClipsStorage = new List<AudioClip>();
	[SerializeField] private List<AudioClip> _sfxClipsStorage = new List<AudioClip>();


	private void Awake()
	{
		var objs = FindObjectsOfType<SoundManager>();
		if (objs.Length != 1)
		{
			Destroy(gameObject);
			print("싱글톤 삭제");
			return;
		}

		DontDestroyOnLoad(gameObject);


		// BGM 오디오 소스 초기화
		_bgmSource = gameObject.AddComponent<AudioSource>();
		_bgmSource.loop = true; // BGM은 기본적으로 루프 설정
		_bgmSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];

		// SFX 오디오 소스 풀 초기화
		_sfxSources = new List<AudioSource>();
		for (int i = 0; i < _poolSize; i++)
		{
			GameObject sfxObject = new GameObject("SFX_Source_" + i);
			sfxObject.transform.parent = transform; // SoundManager 아래에 두기
			AudioSource sfxSource = sfxObject.AddComponent<AudioSource>();
			sfxSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
			_sfxSources.Add(sfxSource);
		}

		// 반복 재생용 오디오 소스 초기화
		_loopingSFXSources = new Dictionary<AudioClip, AudioSource>();
	}

	public void SetMasterVolume(float volume = 1.0f)
	{
		_audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
	}

	public void SetBGMVolume(float volume = 1.0f)
	{
		_audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
	}

	public void SetSFXVolume(float volume = 1.0f)
	{
		_audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
	}


	private AudioClip FindBGMClip(string name)
	{
		foreach (AudioClip clip in _bgmClipsStorage)
		{
			if (clip.name == name)
			{
				return clip;
			}
		}

		return null;
	}


	private AudioClip FindSFXClip(string name)
	{
		foreach (AudioClip clip in _sfxClipsStorage)
		{
			if (clip.name == name)
			{
				return clip;
			}
		}

		return null;
	}

	// 사용 가능한 오디오 소스 찾기
	private AudioSource GetAvailableSFXSource()
	{
		foreach (AudioSource source in _sfxSources)
		{
			if (!source.isPlaying)
			{
				return source;
			}
		}
		// 모든 소스가 사용 중이면 null 반환
		return null;
	}

	// 효과음 재생 메서드 (오브젝트 풀링 사용)
	public void PlaySFX(string clipName)
	{
		AudioSource sfxSource = GetAvailableSFXSource();
		if (sfxSource != null)
		{
			sfxSource.clip = FindSFXClip(clipName);
			sfxSource.Play();
		}
		else
		{
			Debug.LogWarning("No available SFX source to play clip: " + clipName);
		}
	}

	// 반복 재생 효과음 재생 메서드
	public void PlayLoopingSFX(string clipName)
	{
		AudioClip clip = FindSFXClip(clipName);

		if (!_loopingSFXSources.ContainsKey(clip))
		{
			AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
			sfxSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
			sfxSource.clip = clip;
			sfxSource.loop = true;
			sfxSource.Play();
			_loopingSFXSources.Add(clip, sfxSource);
		}
	}

	// 반복 재생 효과음 중지 메서드
	public void StopLoopingSFX(string clipName)
	{
		AudioClip clip = FindSFXClip(clipName);

		if (_loopingSFXSources.ContainsKey(clip))
		{
			AudioSource sfxSource = _loopingSFXSources[clip];
			sfxSource.Stop();
			Destroy(sfxSource);
			_loopingSFXSources.Remove(clip);
		}
	}

	// 배경 음악 재생 메서드
	public void PlayBGM(string clipName, bool loop = true)
	{
		_bgmSource.clip = FindBGMClip(clipName);
		_bgmSource.loop = loop;
		_bgmSource.Play();
	}

	// 배경 음악 중지 메서드
	public void StopBGM()
	{
		_bgmSource.Stop();
	}

	// 배경 음악 일시 정지 메서드
	public void PauseBGM()
	{
		_bgmSource.Pause();
	}

	// 배경 음악 다시 재생 메서드
	public void ResumeBGM()
	{
		_bgmSource.UnPause();
	}

}