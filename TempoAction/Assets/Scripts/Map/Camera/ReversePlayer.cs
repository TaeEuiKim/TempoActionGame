using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePlayer : MonoBehaviour
{
    [Header("화면 전환 속도")]
    [SerializeField] float ReturnSpeed = 0f;

    private float angle = 0;
    private float tempAngle = 0;
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            tempAngle = player.Ani.GetFloat("Speed");

            // Left
            if (player.transform.rotation.y <= 0 && player.Controller.Direction == -1)
            {
                angle += ReturnSpeed * tempAngle * Time.deltaTime;
                if (angle >= 0) 
                { 
                    angle = 0;
                    player.isTurn = false;
                }
            }
            // Right
            else if (player.transform.rotation.y >= -90 && player.Controller.Direction == 1)
            {
                angle -= ReturnSpeed * tempAngle * Time.deltaTime;
                if (angle <= -90) 
                {
                    angle = -90;
                    player.isTurn = true;
                }
            }

            player.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
