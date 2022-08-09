using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Player : MonoBehaviour
{
    public int health { get; set; }
    public bool dead { get;  set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    public int clearBrokeObstacle=0;

    public ParticleSystem getEffect;

    public AudioClip throwZoneClip;
    public AudioClip smokingZoneClip;
    public AudioClip itemPickUpClip;
    public AudioClip smokerPickUpClip;
    public AudioClip policeZoneClip;
    public AudioClip collideObstacleClip;
    public AudioClip finishfanfareClip;
    public AudioClip deathClip;

    public LayerMask obstacleLayer;   
    public LayerMask attachableLayer;
    public LayerMask scoreZoneLayer;
    public LayerMask finishLineLayer;

    private PlayerMovement playerMovement;
    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;
    public BagScale bagScale;

    public List<Smoker> smokers;
    public List<cigaretteButt> cigaretteButts;
    public List<Obstacle> obstacles;
    // ScoroeZone scoreZone;

    void Start()
    {        
        dead = true;
        smokers = new List<Smoker>();
        cigaretteButts = new List<cigaretteButt>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        
    }
    public void StartRevive()
    {
        playerMovement.runningSpeed = 5f;
        dead = false;
        health = 2;
        playerAnimator.SetTrigger("StartGame");
    }
    public void Idle()
    {        
        playerAnimator.SetBool("Injured",false);
        playerAnimator.ResetTrigger("PickUpItem");
        playerAnimator.ResetTrigger("FallingBack");
        playerAnimator.SetTrigger("Idle");
        gameObject.transform.position = new Vector3(0, -3.4f, 3.6f);
        gameObject.transform.rotation = Quaternion.identity;
    }
    public void OnDamage(int damage)
    {
        //Debug.Log("health : "+health);

        // 대미지만큼 체력 감소
        health -= damage;
        // 받은 대미지만큼 흡연자나 꽁초 터뜨리기 처리
        //Debug.Log("health : " + health);
        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <=0 && !dead)
        {
            Die();
        }
        playerAnimator.SetBool("Injured", true);
    }
    public void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if(onDeath != null)
            onDeath();  // onDeath이벤트에 GameManager GameOver() 메서드 존재

        dead = true;

        // 콤보 슬라이더 비활성화
         UIManager.instance.comboSlider.gameObject.SetActive(false);

        // 사망음 재생 
        playerAudioPlayer.PlayOneShot(deathClip);
        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생 
        playerAnimator.SetTrigger($"Die{UnityEngine.Random.Range(1,3)}");        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 사망하지 않았으면 충돌처리 가능
        if (!dead)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) // IAttachable attach = other.GetComponent<IAttachable>();
            {
                Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    playerAudioPlayer.PlayOneShot(collideObstacleClip);
                    GameManager.instance.UpdatecomboScore(UnityEngine.Random.Range(-14, -7));
                    playerAnimator.SetTrigger("FallingBack");
                    playerMovement.runningSpeed = 2.5f;
                    playerMovement.animationSpeed = 0.1f;
                    int damage = obstacle.GetDamageValue();
                    OnDamage(damage);
                    if (smokers != null)
                    {
                        for (int i = smokers.Count-1; i >= 0; i--)
                        {
                            smokers[i].Drop(this.gameObject);
                        }
                    }
                    if (cigaretteButts != null)
                    {
                        for (int i = cigaretteButts.Count -1 ; i >=0; i--)
                        {
                            cigaretteButts[i].Drop(this.gameObject);
                            cigaretteButts.RemoveAt(i);
                        }
                    }
                }
                // 장애물은 벽, 롤링, waterZone
                // 충돌한 상대방을 따라오게 하려면 무슨 타입인지 알아야 한다. 
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Attachable"))
            {
                // IAttachable 컴포넌트 가져오기 시도
                IAttachable attachable = collision.gameObject.GetComponent<IAttachable>();

                //attachable != null;
                if (collision.gameObject.TryGetComponent(out Smoker smoker)) // 가져오기 성공했다면 메서드 실행
                {
                    attachable.OnAttach(gameObject);
                    playerAudioPlayer.PlayOneShot(smokerPickUpClip);

                    playerMovement.runningSpeed += 0.5f;
                    playerMovement.animationSpeed += 0.02f;

                    smokers.Add(collision.gameObject.GetComponent<Smoker>());
                    for (int i = 0; i < smokers.Count; i++)
                    {
                        smokers[i].smokerRunningSpeed = playerMovement.runningSpeed;
                        smokers[i].smokerAnimaionSpeed = playerMovement.animationSpeed;
                    }
                }
                else if (collision.gameObject.TryGetComponent(out cigaretteButt cigar))
                {
                    attachable.OnAttach(gameObject);
                    playerAudioPlayer.PlayOneShot(itemPickUpClip);
                    playerMovement.runningSpeed += 0.5f;
                    playerMovement.animationSpeed += 0.02f;
                    playerAnimator.SetTrigger("PickUpItem");


                    getEffect.Play();

                    cigaretteButts.Add(collision.gameObject.GetComponent<cigaretteButt>());
                    //Debug.Log($"cigarCount " + cigaretteButts.Count);                    
                    bagScale.GetSizeValue(cigaretteButts.Count);                    
                    for (int i = 0; i < smokers.Count; i++)
                    {
                        smokers[i].smokerRunningSpeed = playerMovement.runningSpeed + 0.05f;
                        smokers[i].smokerAnimaionSpeed = playerMovement.animationSpeed;
                    }
                }
            }
            else if (collision.gameObject.layer == scoreZoneLayer)
            {
                
            }//else if(other.gameObject.layer == finishLine){}
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (!dead)
        {
            // ScoreZone에 접근
            // 1. smokingZone 허용가능한 인원 수를 받아오고 그 수만큼만 목표물을 smokingArea로 바꿈
            // 2. throwZone 가진 모든 꽁초를 버린다. 
            if (other.gameObject.layer == LayerMask.NameToLayer("ScoreZone"))
            {
                ScoreZone scorezone = other.GetComponent<ScoreZone>();
                if (scorezone!=null)
                {
                    if (other.TryGetComponent(out SmokingZone smokingzone))
                    {
                        if (smokers.Count>0)
                        {
                            playerAudioPlayer.PlayOneShot(smokingZoneClip);
                            playerAnimator.SetTrigger("LookBack");
                        }
                        int allow;
                        int allowable = smokingzone.GetScoreValue();
                        if (allowable >= smokers.Count)
                            allow = smokers.Count - 1;
                        else allow = allowable - 1;
                        for (int i = allow; i >=0; i--)
                        {
                            smokers[i].Detach(smokingzone.gameObject);
                            smokers.RemoveAt(i);
                            // 플레이어를 조금 더 멀리서 보고 limitX도 넓히자. NavMeshAgent도 다시 하고
                        }                        
                    }
                    else if(other.TryGetComponent(out ThrowZone throwzone))
                    {
                        if (cigaretteButts.Count>0)
                        {
                            playerAudioPlayer.PlayOneShot(throwZoneClip);
                        }
                        for (int i = cigaretteButts.Count-1; i >= 0; i--)
                        {
                            cigaretteButts[i].Detach(throwzone.gameObject);
                            cigaretteButts.RemoveAt(i);
                        }
                        // 가지고 있던 담배 꽁초를 버리고 점수를 획득한다.
                        // 돈이 터지는 이펙트를 보이고 
                    }
                    return ;
                }
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) // 여기는 FinishLine 뒤의 Obstacle
            {
                Debug.Log($"GameManager.instance.finalScore : {GameManager.instance.finalScore}");
                
                Obstacle obstacle = other.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    GameManager.instance.finalScore -= obstacle.MinusCombo();
                    playerAudioPlayer.PlayOneShot(collideObstacleClip);
                    Debug.Log($"WGameManager.instance.finalScore : {GameManager.instance.finalScore}");
                    clearBrokeObstacle += 1;
                    if (GameManager.instance.finalScore <= 0)
                    {
                        playerMovement.runningSpeed = 0;
                        Clear();
                    }
                }
                                                       
            }else if(other.gameObject.layer == LayerMask.NameToLayer("FinishLine"))
            {
                playerAudioPlayer.PlayOneShot(finishfanfareClip);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("PoliceZone"))
            {
                PoliceZone policeZone = other.GetComponent<PoliceZone>();
                if (policeZone != null&&policeZone.policeNum==0)
                {                    
                    for (int i = smokers.Count-1; i >= 0; i--)
                    {
                        smokers[i].Detach(policeZone.gameObject);
                    }
                    playerAudioPlayer.PlayOneShot(policeZoneClip);
                    playerAnimator.SetTrigger("LookBack");
                }
                if (policeZone != null && policeZone.policeNum > 0)
                {
                    for (int i = smokers.Count - 1; i >= 0; i--)
                    {
                        smokers[i].Drop(this.gameObject); //targetPlayer == null 처리
                        policeZone.TriggerAndChase(smokers[i].transform);
                        // smokers[i].transform.position; // smokers의 위치를 각각 PoliceMan의 Chase메소드에 넘김
                    }
                    playerAudioPlayer.PlayOneShot(policeZoneClip);
                    playerAnimator.SetTrigger("LookBack");

                }
            }
        }
    }
    private void Clear()
    {
        this.transform.position = new Vector3(00, -1.45f, 395f);
        this.transform.rotation = new Quaternion(0,180,0,0);
        playerAnimator.SetTrigger($"Victory{UnityEngine.Random.Range(1, 4)}");    
        
        GameManager.instance.UpdateFinalScore();
        GameManager.instance.StageClear();
        GameManager.instance.UpdateMoneySum();

        DataManager.instanceData.curStage += 1;
        DataManager.instanceData.Save();
        Debug.Log(DataManager.instanceData.curStage);
    }
}    
