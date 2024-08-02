using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePlatform : MonoBehaviour
{
    [System.Serializable]
    private class buffInfoList
    {
        //[Header("¹öÇÁ Á¤º¸ ¸®½ºÆ®")]
        public List<Define.BuffInfo> infoList = new List<Define.BuffInfo>();
    }
   // [Header("ÇÃ·§Æû Á¤º¸ ¸®½ºÆ®")]
    [SerializeField] private List<buffInfoList> _platformList = new List<buffInfoList>();
    private List<BuffPlatform> _curPlatformList = new List<BuffPlatform>();

   // [Header("ÇÁ¸®ÆÕ")]
    [SerializeField] private GameObject _platformPrefab;


    //[Header("ÇÃ·§ÆûÀÌ ¹Ù²î´Â ½Ã°£")]
    [SerializeField] private float _changeTime; // ¹Ù²î´Â ½Ã°£
    private float _timer;

    private int _index = 0;
    public int Index 
    {
        get => _index;
        set
        {
            _index = value;
            _index = _index % _platformList.Count;
        }
    }


    void Update()
    {
        if (_timer >= _changeTime)
        {
            Change();
            _timer = 0;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public void Create() // ÃÊ±â ÇÃ·§Æû »ý¼º ÇÔ¼ö
    {
        if (_platformList.Count == 0) return;
        if (_platformList[_index].infoList.Count == 0) return;

        Delete();

        float width = _platformPrefab.GetComponent<Renderer>().bounds.size.x;

        int half = _platformList[_index].infoList.Count / 2;
        float posX = transform.position.x - (width * half);

        if (_platformList[_index].infoList.Count % 2 == 0) // »ý¼º °³¼ö°¡ Â¦¼öÀÏ °æ¿ì
        {
            posX += width / 2;
        }

        for (int i = 0; i< _platformList[_index].infoList.Count; i++)
        {
            GameObject temp = Instantiate(_platformPrefab, new Vector3(posX, transform.position.y, transform.position.z), Quaternion.identity, transform);

            BuffPlatform platform = temp.GetComponent<BuffPlatform>();
            platform.Change(_platformList[_index].infoList[i]);

            _curPlatformList.Add(platform.GetComponent<BuffPlatform>());
            posX += width;
        }

        Index++;
    }
    public void Delete()
    {
        for (int i = transform.childCount-1; i>=0;i--)
        {
            Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void Change() // ÇÃ·§Æû ±³Ã¼ ÇÔ¼ö
    {
        if (_curPlatformList.Count <= 1) return;

        for (int i = 0; i < _curPlatformList.Count; i++)
        {
            _curPlatformList[i].Change(_platformList[_index].infoList[i]);
        }

        Index++;
    }

    private void OnDrawGizmos()
    {
        if (_platformList.Count == 0) return;

        Vector3 size = _platformPrefab.GetComponent<Renderer>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x * _platformList[0].infoList.Count, size.y, size.z));
    }
}
