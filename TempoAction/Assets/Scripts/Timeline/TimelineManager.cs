using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    private Dictionary<string, PlayableDirector> _timelineStorage = new Dictionary<string, PlayableDirector>();
    private PlayableDirector _currentDirector;


    void Start()
    {
        foreach (Transform timeline in transform)
        {
            Timeline temp = timeline.GetComponent<Timeline>();
           _timelineStorage.Add(temp.Name, temp.Director);
        }
    }

    public void PlayTimeline(string name)
    {
        if (_timelineStorage.Count <= 0)
        {
            return;
        }

        foreach (var curTimeline in _timelineStorage)
        {
            if (curTimeline.Key == name)
            {
                if (_currentDirector != null)
                {
                    _currentDirector.Stop();
                }
                _currentDirector = curTimeline.Value;
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
