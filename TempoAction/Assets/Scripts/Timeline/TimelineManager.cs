using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    [System.Serializable]
    public class Timeline
    {
        public string name;
        public PlayableAsset playable;
        public PlayableDirector director;
    }
    
    [SerializeField] private List<Timeline> _timelines;
    private PlayableDirector _currentDirector;

    void Start()
    {
    
    }

    public void PlayTimeline(string name)
    {
        if (_timelines.Count <= 0)
        {
            return;
        }

        foreach (Timeline timeline in _timelines)
        {
            if (timeline.name == name)
            {
                if (_currentDirector != null)
                {
                    _currentDirector.Stop();
                }
                _currentDirector = timeline.director;
                _currentDirector.playableAsset = timeline.playable;
                _currentDirector.Play();
                return;
            }
        }
        Debug.LogWarning("Timeline not found: " + name);
    }

    public void StopCurrentTimeline()
    {
        if (_currentDirector != null)
        {
            _currentDirector.Stop();
            _currentDirector = null;
        }
    }

    public void PauseCurrentTimeline()
    {
        if (_currentDirector != null)
        {
            _currentDirector.Pause();
        }
    }

    public void ResumeCurrentTimeline()
    {
        if (_currentDirector != null)
        {
            _currentDirector.Resume();
        }
    }
}
