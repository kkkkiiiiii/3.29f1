using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    // ScoreZone은 ThrowZone, SmokingZone에서 공통적으로 콤보점수와 스테이지 점수를 증가시킨다.
    // 가져온 꽁초 수, 흡연자 수에 따라서 점수를 다르게 준다. 
    public virtual int GetScoreValue()
    {

        return 0;
    }
}
