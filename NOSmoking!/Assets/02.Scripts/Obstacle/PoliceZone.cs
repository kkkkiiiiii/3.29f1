using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceZone : MonoBehaviour
{
    // PoliceZone에 들어가면 추적하고 있던 흡연자의 위치를 폴리스존으로 옮긴다. Vector3.Lerp
    // 콤보 점수 깎는다. 
    public int policeNum;

    private float xposition;
    private float yposition;
    private float zposition;
    private Vector3 position;
    
    public GameObject[] policePrefab;

    public List<PoliceMan> policeMen;
    private void Start()
    {

        if (policeNum>0)
        {
            CreatePoliceMan();
        }
    }

    private void CreatePoliceMan()
    {

        for (int i = 0; i < policeNum; i++)
        {
            int index = Random.Range(0, 2);
            float x = Random.Range(-0.8f, 0.8f);
            float z = Random.Range(-2f, 0.2f);
            position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            GameObject policemanGameObject = Instantiate(policePrefab[index], position, Quaternion.AngleAxis(Random.Range(140f,220f),Vector3.up));
            PoliceMan policeman = policemanGameObject.GetComponent<PoliceMan>();
            GameManager.instance.exsitingPoliceMan.Add(policemanGameObject);
            policeman.policmanAnimator.SetTrigger($"Talking{Random.Range(1, 3)}");
            if (policeman!= null)
            {
                policeMen.Add(policeman);
            }                                   
        }        
    }

    public void TriggerAndChase(Transform smokerPosition)
    {
        if (policeMen.Count == 0)
            return;        
        policeMen[0].ChaseSmoker(smokerPosition.gameObject);
        policeMen.RemoveAt(0);
    }

    
    // policeNum이 1이상의 숫자가 있다면 policeZone에 그 수 만큼의 policePrefab으로 소환하고 Player의 smokers리스트의 
    // 오브젝트에 접근해서 위치를 바꾼다. 달리는 애니메이션을 취하고 Vector3.lerp로 자연스럽게 충돌한다. 
    // 충돌하면 오브젝트 지우고 잡는 이펙트(폭파?) 

    // policeZone에 들어오면 느낌표 이펙트 추가! 
}
