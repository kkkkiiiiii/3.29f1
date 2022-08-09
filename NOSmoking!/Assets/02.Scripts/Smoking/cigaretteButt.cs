using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cigaretteButt : MonoBehaviour, IAttachable
{   
    public ParticleSystem cigaretteParticle;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cigaretteParticle = rb.GetComponentInChildren<ParticleSystem>();
    }
    public void Drop(GameObject runner) // 장애물에 피격 시 점수 깎게 한다. 장애물 마다 다른 공격력을 가질 수 있다. 
    {
        // 꽁초, 흡연자 그냥 떨어뜨림. → 게임 매니저로 접근 → 점수, 콤보 감소→ UI UpdateScore 처리
        Player player = runner.GetComponent<Player>();
        Debug.Log("cigar.Drop");
        if (player != null)
        {
            this.gameObject.SetActive(true);
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(1f, 2f);
            float randomZ = Random.Range(-1f, 3f);
            
            this.gameObject.transform.position = runner.transform.position+ new Vector3(2,1,0);
            rb.AddForce(new Vector3(randomX, randomY, randomZ), ForceMode.Impulse);
            //Instantiate 말고 그냥 위치를 플레이어 근처로 옮기자             
        }
    }
    public void OnAttach(GameObject runner) // 점수추가는 흡연자, 꽁초 다르게 아이템마다 점수 다르게 하기 위해서
    {
        // 꽁초, 흡연자 플레이어 뒤에 붙인다. → 게임 매니저로 접근 → 점수 증가→ UI UpdateScore 처리
        // 전달받은 게임 오브젝트로부터 Player 컴포넌트 가져오기 시도
        Player player = runner.GetComponent<Player>();

        // Player 컴포넌트가 있다면
        if (player != null)
        {            
            GameManager.instance.UpdateStageScore(1);
            GameManager.instance.UpdatecomboScore(1);
        }       
        gameObject.SetActive(false);
        // 추가 : 플레이어 손에 담배꽁초를 더하는 작업, 리스트로 프리팹생성해서 플레이어 손의 위치에 붙인다.
    }
    public void Detach(GameObject scoreZone)
    {
        ThrowZone throwZone = scoreZone.GetComponent<ThrowZone>();       
        if (throwZone != null)
        {
            GameManager.instance.UpdateStageScore(1);
            GameManager.instance.UpdatecomboScore(2);
        }


        // 꽁초, 흡연자 scoreZone에 놔둔다. → 게임 매니저로 접근 → 점수, 콤보 증가→ UI UpdateScore 처리

    }
}
