using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Serializatio<T>
{
    public Serializatio(List<T> _data) => data = _data;
    public List<T> data;
}
[System.Serializable]
public class data
{
    public data(string _Money, string _Stage, string _Price)
    {
        Money = _Money;
        Stage = _Stage;
        Price = _Price;
    }
    public string Money, Stage, Price;    
}
public class DataManager : MonoBehaviour
{
    // 이 스크립트는 현재 스테이지, 착용한 모자 인덱스, 가진 돈에 대한 정보를 가지고 있는다. DontDestroyOnLoad
    // ShopScene에서는 ShopManager가 DataManager를 업데이트한다. 
    public TextAsset CashDatabase;
    public List<data> dataList, myDataList;    

    public static DataManager instanceData;
    public List<int> AcquiredNumList;
    public int curStage = 0;
    public int curHatIndex = -1;
    public int moneySum;
    public int priceData = 0;

    string filePath;
    private void Awake()
    {
        if (instanceData != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instanceData = this;

            DontDestroyOnLoad(gameObject);
        }
        filePath = Application.persistentDataPath + "/MyDataText.txt";
        print(filePath);
        Load();
        moneySum = int.Parse(myDataList[0].Money);
        curStage = int.Parse(myDataList[0].Stage);
        priceData = int.Parse(myDataList[0].Price);
        Debug.Log(moneySum);
        Debug.Log(curStage);
        Debug.Log(priceData);
        Save();
    }
    /*string cashJdata = File.ReadAllText(Application.dataPath + "/Resources/CashDataText.txt");
            = JsonConvert.DeserializeObject<List<data>>(cashJdata);
        DataManager.instanceData.moneySum = 
    */
    private void Start()
    {
        /*
        string cashJdata = File.ReadAllText(Application.dataPath + "/Resources/CashDataText.txt");
        Debug.Log($"moneySum : {moneySum}");        
        myDataList = JsonConvert.DeserializeObject<List<data>>(cashJdata);
        moneySum = int.Parse(myDataList[0].Money);
        curStage = int.Parse(myDataList[0].Stage);
        string[] line = CashDatabase.text.Substring(0, CashDatabase.text.Length).Split('\n');
        Debug.Log(line.Length);
        Debug.Log(line[0]);
        Debug.Log(line[1]);
        Debug.Log(line[2]);
        dataList.Add(new data(line[0], line[1], line[2]));
        */
    }
    public void Save()
    {
        if (curStage == 0)
        {
            moneySum = int.Parse(myDataList[0].Money);
            curStage = int.Parse(myDataList[0].Stage);
            priceData = int.Parse(myDataList[0].Price);
            Debug.Log("00");
        }
        myDataList[0].Money = moneySum.ToString();
        myDataList[0].Stage = curStage.ToString();
        myDataList[0].Price = priceData.ToString();

        Debug.Log(myDataList[0].Money);
        Debug.Log(myDataList[0].Stage);
        Debug.Log(myDataList[0].Price);
        string jdata = JsonUtility.ToJson(new Serializatio<data>(myDataList));
        File.WriteAllText(filePath, jdata);               
    }
    void Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("datafilePath is null");
            ResetData();
            return;
        }
        string jdata = File.ReadAllText(filePath);
        myDataList = JsonUtility.FromJson<Serializatio<data>>(jdata).data;
        //string jdata = File.ReadAllText(Application.dataPath + "/Resources/myCashDataText.txt");
        //myDataList = JsonConvert.DeserializeObject<List<data>>(jdata);
    }
    void ResetData()
    {
        string[] line = CashDatabase.text.Substring(0, CashDatabase.text.Length).Split('\n');
        Debug.Log(line[0]);
        Debug.Log(line[1]);
        Debug.Log(line[2]);
        myDataList.Add(new data(line[0], line[1], line[2]));

        Save();
        print(myDataList);
        Load();
    }
}
