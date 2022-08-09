using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCigar : MonoBehaviour
{
    public ParticleSystem smokeEffect; // 담배 연기 효과
    
    private void Start()
    {
        smokeEffect.Play();
    }   
}
