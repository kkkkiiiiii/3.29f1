using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokingZone : ScoreZone
{
    // smokers 리스트에 접근해서 허용 인원만큼 Detach한다.
    public override int GetScoreValue()
    {
        return Random.Range(5, 10);
    }
}
