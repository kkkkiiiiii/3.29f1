using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float touchXDelta { get; set; }

    void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다. 플레이 버튼을 눌러야만 시작
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            touchXDelta = 0;
            return;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            touchXDelta = Input.GetTouch(0).deltaPosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }
    }    
}
