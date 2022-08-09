using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMan : MonoBehaviour
{
    // policePrefab에 넣을 경찰 하나 하나에 적용할 스크립트 
    // smoker와 충돌하면 그 오브젝트 접근해서 없애고 이펙트 발생
    // 기본 상태와 접근해서 잡으러 가는 애니메이션     
    private new CapsuleCollider collider;
    public Animator policmanAnimator;
    private GameObject smokerGameObject;    

    public ParticleSystem catchSmokerParticle;

    public float chasingSpeed = 1f;
    private float Duration = 3f;
    private float elapsedTime;
    public bool isChasing= false;

    private void Awake()
    {
        collider = this.gameObject.GetComponent<CapsuleCollider>();
        policmanAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(isChasing)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / Duration;
            this.transform.position = Vector3.Lerp(this.transform.position, smokerGameObject.transform.position,chasingSpeed* percentageComplete);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Attachable"))
        {
            // IAttachable 컴포넌트 가져오기 시도
            IAttachable attachable = collision.gameObject.GetComponent<IAttachable>();

            //attachable != null;
            if (collision.gameObject.TryGetComponent(out Smoker smoker)) // 가져오기 성공했다면 메서드 실행
            {
                smoker.gameObject.SetActive(false);
                catchSmokerParticle.Play();
                collider = null;
                policmanAnimator.SetTrigger($"Chasing{Random.Range(1, 3)}");
                GameManager.instance.UpdatecomboScore(-1);
            }
        }
    }
    
    public void ChaseSmoker(GameObject targetSmoker)
    {
        policmanAnimator.SetTrigger("Chase");
        smokerGameObject = targetSmoker;
        isChasing = true;
        Debug.Log(isChasing);
    }
}
