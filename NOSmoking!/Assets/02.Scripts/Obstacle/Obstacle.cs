using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int powerHitToPlayer=1;
    public int breakObstacleMinusCombo;
    public ParticleSystem wallExplosion;

    private Player player; // 부딪힌 플레이어의 체력 1을 깎는 역할


    private void Start()
    {
        player = GetComponent<Player>();
    }

    public int GetDamageValue()
    {        
        wallExplosion.Play();
        this.gameObject.SetActive(false);
        return powerHitToPlayer;
    }

    public int MinusCombo()
    {
        wallExplosion.Play();
        this.gameObject.SetActive(false);
        return breakObstacleMinusCombo;
    }
}
