using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float limitX = 5f;
    public float xSpeed = 5f;
    public float runningSpeed = 5f;
    public float animationSpeed;

    private Player player;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    void Start()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(player.dead == false)
            Move(!player.dead);

        if (player.health > 1)
        {
            playerAnimator.SetFloat("Run", 0.1f + animationSpeed);
            // playerAnimator.SetFloat("Running", 0.2f +콤보) 콤보가 높을수록 빠르게 달리도록 수정
        }else if(player.health == 1)
        {
            playerAnimator.SetFloat("InjuredRun", 0.1f + animationSpeed);
        }

    }
    private void Move(bool dead)
    {
        if (dead)
        {
            float newX;

            newX = transform.position.x + xSpeed * playerInput.touchXDelta * Time.fixedDeltaTime;
            newX = Mathf.Clamp(newX, -limitX, limitX);

            // z축 부분을 콤보 수에 따라 변하도록 코드 수정
            Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + runningSpeed * Time.fixedDeltaTime);
            playerRigidbody.position = newPosition;
        }        
    }
}
