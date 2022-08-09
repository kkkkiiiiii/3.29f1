using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{
    public GameObject[] mapBGPrefab;
    public GameObject[] stageBGPrefab;
    private GameObject stagePB;
    private GameObject mapPB;
    public bool isGameOver { get; private set; }
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            { 
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>(); 
            }
            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }    
    public int finalScore = 0;
    private int stageScore;
    private int comboScore;

    public int getMoney=0;
    private int Diamond;

    public Player player;
    public List<GameObject> exsitingPoliceMan;
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 자신을 파괴
        if(instance != this)
            Destroy(gameObject);
        isGameOver = true;

        
    }
    void Start()
    {
        FindObjectOfType<Player>().onDeath += GameOver;
        UpdateMoneySum();
        UIManager.instance.UpdateStageNum();

        stagePB = Instantiate(stageBGPrefab[DataManager.instanceData.curStage]);
        Debug.Log(DataManager.instanceData.curStage);
        mapPB = Instantiate(mapBGPrefab[Random.Range(0, 5)]);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateStageScore();
        //UpdatecomboScore();
    }
    public void UpdateStageScore(int newScore)
    {
        if (!isGameOver)
        {
            // 점수 추가
            stageScore += newScore;
            finalScore += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateStageScoreText(stageScore);
        }
    }
    public void UpdatecomboScore(int newScore)
    {
        if (!isGameOver)
        {
            // 점수 추가
            comboScore += newScore;
            finalScore += newScore;
            if(comboScore <= 0)
                comboScore = 0;
            //Debug.Log($"GameManager : " + comboScore);
            // 콤보 점수 UI 텍스트 갱신
            UIManager.instance.UpdateComboScoreText(comboScore);            
        }
    }    
    public void UpdateMoneySum()
    {        
        getMoney = finalScore * player.clearBrokeObstacle;
        DataManager.instanceData.moneySum += getMoney;

        UIManager.instance.UpdateMoneySumText(DataManager.instanceData.moneySum);
        DataManager.instanceData.Save();
    }
    public void UpdateFinalScore()
    {
        finalScore = stageScore + comboScore;
        UIManager.instance.UpdateFinalScoreText(stageScore, finalScore);
    }
    public void StageClear() 
    {
        isGameOver = true;
        UIManager.instance.SetActiveGameClearUI(true);
        DestroyPoliceMan();
    }
    public void StartGame()
    {
        player.StartRevive();
        isGameOver = false;
        UIManager.instance.SetActiveMainUI(false);        
    }
    // 게임 오버 처리
    public void GameOver()
    {
        isGameOver = true;
        // 플레이어를 터트리는 효과 추가

        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
        exsitingPoliceMan.Clear();
        DestroyPoliceMan();
    }
    private void DestroyPoliceMan()
    {
        Debug.Log("Destroy");
        for (int i = exsitingPoliceMan.Count - 1; i >= 0; i--)
        {
            Destroy(exsitingPoliceMan[i]);
            Debug.Log("Destroy : " + i);
            exsitingPoliceMan.RemoveAt(i);
        }
        
    }
    public void GoMainUI()
    {
        stageScore = 0;
        comboScore = 0;
        UIManager.instance.UpdateStageScoreText(0);
        UIManager.instance.UpdateComboScoreText(0);
        UIManager.instance.SetActiveMainUI(true);
        UIManager.instance.SetActiveGameClearUI(false);
        UIManager.instance.SetActiveGameoverUI(false);
        player.Idle();

        UIManager.instance.UpdateStageNum();
        SpawnMapBG();
    }
    private void SpawnMapBG()
    {
        if (stagePB != null)
        {
            Destroy(stagePB);
            stagePB = Instantiate(stageBGPrefab[DataManager.instanceData.curStage]);
        }
            
        if (mapPB != null)
        {
            Destroy(mapPB);
            mapPB = Instantiate(mapBGPrefab[Random.Range(0, 6)]);
        }
        
    }
}
