using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
   
    protected EnemyStat _stat;
    public EnemyStat Stat { get { return _stat; } }

    private PlayerManager _player;

    private float _direction = -1;
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _stat = GetComponent<EnemyStat>();
    }


    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = _player.transform.position - transform.position;

        Vector3 cross = Vector3.Cross(Vector3.forward, direction);
     
        if (cross.y > 0)
        {
            if (_direction != 1)
            {
                transform.GetChild(0).DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
                _direction = 1;
            }
        }
        else if (cross.y < 0)
        {
            if (_direction != -1)
            {
                transform.GetChild(0).DOLocalRotate(new Vector3(0, -90, 0), 0.1f);
                _direction = -1;
            }
        }
    }
}
