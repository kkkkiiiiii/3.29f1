using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Smoker : MonoBehaviour, IAttachable
{
    public float smokerRunningSpeed = 5f;
    public float smokerAnimaionSpeed;
    public GameObject cigarPivot;
    private Player targetPlayer;
    private ScoreZone targetScoreZone;
    private PoliceZone targetPoliceZone;
    private NavMeshAgent pathFinder;
    // private ScoreZone targetScoreZone;
    private SmokerHand smokerhand;

    public AudioClip beTakenSound; // 플레이어한테 잡혔을 때 소리
    public AudioClip cheerUpSound; // SmokingZone에 도착했을 때 소리

    //private PlayerMovement playerMovement;
    private Animator smokerAnimator; 
    private Renderer smokerRenderer; // 렌더러 -> 색 변경
    // 추적 대상이 존재하는 지 알려주는 프로퍼티

    // && Vector3.Distance(target.transform.position,this.transform.position)<10
    
    public bool hasTarget 
    {
        get
        {
            if (targetPlayer == null)
                return false;
            else
                return true;            
        }        
    }
    public bool dead { get; private set; }
    private void OnEnable()
    {
        dead = false;
    }
    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        smokerAnimator = GetComponent<Animator>();
        smokerRenderer = GetComponentInChildren<Renderer>();
    }
    private void Start()
    {
        smokerhand = GetComponent<SmokerHand>();
        StartCoroutine(UpdatePath());        
    }
    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        smokerAnimator.SetBool("HasTarget", hasTarget);
    }
    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        //Debug.Log(hasTarget);       

        // 플레이어를 따라가는 코드
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.speed = smokerRunningSpeed;
                if(targetPlayer != null)
                {
                    pathFinder.SetDestination(targetPlayer.transform.position);
                    smokerAnimator.SetFloat("Run", smokerAnimaionSpeed);
                }                 
            }
            else if(!hasTarget)
            {
                if (targetScoreZone != null)
                {
                    pathFinder.SetDestination(targetScoreZone.transform.position);
                    smokerAnimator.SetTrigger($"cheering{Random.Range(1, 3)}");
                }
                else if (targetPoliceZone != null)
                {
                    //pathFinder.SetDestination(targetPoliceZone.tr)
                }
                pathFinder.isStopped = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Drop(GameObject player)
    {
        // 꽁초, 흡연자 그냥 떨어뜨림. → 게임 매니저로 접근 → 점수, 콤보 감소→ UI UpdateScore 처리
        targetPlayer = null;
    }
    public void OnAttach(GameObject runner)
    {
        targetScoreZone = null;
        targetPlayer = runner.GetComponent<Player>();              

        // 꽁초, 흡연자 플레이어 뒤에 붙인다. → 게임 매니저로 접근 → 점수 증가→ UI UpdateScore 처리
        Player player = runner.GetComponent<Player>();        

        if (player != null)
        {
            //Debug.Log("GetComponent<Player>");
            GameManager.instance.UpdateStageScore(1);
            GameManager.instance.UpdatecomboScore(1);
        }
    }
    public void Detach(GameObject Zone)
    {
        targetPlayer = null;
        if (Zone.TryGetComponent<ScoreZone>(out ScoreZone ScoreZone))
        {
            smokerhand.isArrive = true;
            targetScoreZone = ScoreZone.GetComponent<ScoreZone>();
            //ScoreZone scorezone1 = Zone.GetComponent<ScoreZone>();
            cigarPivot.gameObject.transform.position = new Vector3(0, 0, 0);
            if (targetScoreZone != null)
            {
                GameManager.instance.UpdateStageScore(1);
                GameManager.instance.UpdatecomboScore(2);
            }
        }
        if (Zone.TryGetComponent<PoliceZone>(out PoliceZone policeZone))
        {
            smokerhand.isArrive = true;
            targetPoliceZone = policeZone.GetComponent<PoliceZone>();
            cigarPivot.gameObject.transform.position = new Vector3(0, 0, 0);
            if (targetPoliceZone != null)
            {
                GameManager.instance.UpdateStageScore(-1);
            }

        }
        // 꽁초, 흡연자 scoreZone에 놔둔다. → 게임 매니저로 접근 → 점수, 콤보 증가→ UI UpdateScore 처리
        //hasTarget = true; // Target을 scoreZone으로 바꾼다.
        //targetScoreZone = scoreZone.GetComponent<ScoreZone>();
    }

}
