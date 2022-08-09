using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;       
}

[System.Serializable]
public class Item
{
    public Item(string _Num, string _Name, string _Explain, bool _isAcquired ,bool _isUsing)
    {
        Num = _Num;
        Name = _Name; 
        Explain = _Explain;             
        isAcquired = _isAcquired;
        isUsing = _isUsing;
    }
    public string Num, Name, Explain;
    public bool isAcquired ,isUsing;
}
public class ShopManager : MonoBehaviour
{
    public int currentPlayerIndex;
    public int currentPageIndex = 0;
    public int prevUsingIndex;
    public int curUsingIndex;
    public GameObject[] Page;
    public Button buyButton;
    public Button wearingButton;
    public Button removingButton;
    private int Price = 0;

    private AudioSource ShopAudio;
    public AudioClip clickClip;
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, isntAcquiredList;
    public GameObject[] UsingImage;
    public Image[] TabImage, ItemImage;
    public Material TabIdleMaterial, TabSelectMaterial;
    public PageSwiper pageSwiper;

    private int curSelectedNum=0;

    string filePath;
    void Start()
    {
        ShopAudio = GetComponent<AudioSource>();
        // 전체 아이템 리스트 불러오기
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            AllItemList.Add(new Item(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
        }
        filePath = Application.persistentDataPath + "/MyItemText.txt";
        print(filePath);
        Load();
        DataManager.instanceData.AcquiredNumList.Sort();
        for (int i = 0; i < MyItemList.Count; i++)
        {
            if (MyItemList[i].isAcquired == true)
            {
                Debug.Log(i);
                UsingImage[i].SetActive(false);
            }            
                        
            for (int j = 0; j < DataManager.instanceData.AcquiredNumList.Count; j++)
            {
                if(DataManager.instanceData.AcquiredNumList[j] == i)
                    UsingImage[i].SetActive(false);                
            }
        }
        /*
        foreach (ShopHatInfo Hat in Hats)
        {
            Hat.isLocked = PlayerPrefs.GetInt(Hat.name,0) == 0 ? false : true;           
        }

        currentPlayerIndex = PlayerPrefs.GetInt("SelectedChar", 0);

        foreach (GameObject HatModule in HatsModules)
        {
            HatModule.SetActive(false);
        }
        HatsModules[currentPlayerIndex].SetActive(true);*/
    }
    void Save()
    {        
        string itemdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));

        DataManager.instanceData.Save();
        File.WriteAllText(filePath, itemdata);
        TabClick(curSelectedNum);
    }
    void Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("itemfilePath is null");
            ResetItem();
            return;
        }
        string jdata = File.ReadAllText(filePath);
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
        TabClick(curSelectedNum);
    }
    public void ResetItem()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            MyItemList.Add(new Item(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
        }
        print(MyItemList);
        Save();
        Load();
    }

    // 탭 클릭하면 이 아이템이 선택되었는 지 material 색변경으로 시각적으로 보여주고 구매한 상품이면 Buy버튼을 지우고 착용버튼을 보여준다. 
    // 구매하지 않은 상품이면 Buy버튼을 보여주고 착용버튼을 지운다.
    public void TabClick(int tabNum)
    {
        curSelectedNum = tabNum;        
        for (int i = 0; i < TabImage.Length; i++)
        {            
            TabImage[i].material = i == tabNum ? TabSelectMaterial : TabIdleMaterial;
        }
        Debug.Log("tabNum :" + tabNum);
        if (MyItemList[tabNum].isAcquired == true)
        {
            buyButton.gameObject.SetActive(false);
            if (MyItemList[tabNum].isUsing == true)
            {
                wearingButton.gameObject.SetActive(false);
                removingButton.gameObject.SetActive(true);
            }
            else if (MyItemList[tabNum].isUsing == false)
            {
                wearingButton.gameObject.SetActive(true);
                removingButton.gameObject.SetActive(false);
            }
        }
        else if(MyItemList[tabNum].isAcquired == false)
        {
            buyButton.gameObject.SetActive(true);
            wearingButton.gameObject.SetActive(false);
        }        
        ShopAudio.PlayOneShot(clickClip);
    }
    // 현재 페이지 수를 알아오고 그 페이지 안에 있는 isAcquired = false인 것 중에서 랜덤하게 하나를 획득한다.
    // 페이지 0 이면 MyItemList의 0 ~ 8번 인덱스의 아이템의 획득여부를 조사하고 i*pageNum 
    // 해당 페이지의 isAcquired == false인 아이템을 리스트로 받는다. 0페이지의 false 리스트에서 랜덤해서 하나를 사는 처리를 한다.
    public void BuyRandomItem()
    {
        if (DataManager.instanceData.moneySum >= Price)
        {
            Debug.Log("Buy");
            Debug.Log($"PanelIndex :{pageSwiper.PanelIndex}");
            // isAcquired == false 받아오기
            for (int i = 0 + 9* pageSwiper.PanelIndex; i < 9 + 9*pageSwiper.PanelIndex; i++) // 해당 페이지의 아이템 리스트로 접근!
            {
                Debug.Log(MyItemList[i].isAcquired);
                if (MyItemList[i].isAcquired == false)
                {
                    isntAcquiredList.Add(MyItemList[i]);
                }                
            }
            int acquiredIndex = Random.Range(1, isntAcquiredList.Count); // 해당페이지의 리스트만 가지고 있다. 최대 9개 
            int BuyNum =  int.Parse(isntAcquiredList[acquiredIndex].Num);
            isntAcquiredList.RemoveAt(acquiredIndex);            
            MyItemList[BuyNum].isAcquired = true;
            UsingImage[BuyNum].gameObject.SetActive(false);
            DataManager.instanceData.AcquiredNumList.Add(BuyNum);
            ShopAudio.PlayOneShot(clickClip);

            DataManager.instanceData.moneySum -= DataManager.instanceData.priceData;
            DataManager.instanceData.priceData += 500; // DataManager의 살 수 있는 가격을 저장
        }
        Save();
    }
    // 현재 선택한 인덱스가 구매한 상품인지 확인하고 가능하면 DataManager로 현재 착용한 모자의 인덱스를 넘긴다. 
    public void WearingHat()
    {
        // 1. 반복문돌려서 isUsing == true 인걸 찾아서 false로바꿈
        // 2. 지난 번에 착용한 모자의 인덱스를 기억하고 
        // 착용한 상품의 인덱스가 원래 착용하고 있던 상품의 인덱스와 다르다면 원래 착용하던 상품의 isUsing을 false로 바꿔야한다.

        if (DataManager.instanceData.curHatIndex != curSelectedNum)
            MyItemList[DataManager.instanceData.curHatIndex].isUsing = false;

        DataManager.instanceData.curHatIndex = curSelectedNum;
        MyItemList[curSelectedNum].isUsing = true;
        wearingButton.gameObject.SetActive(false);
        removingButton.gameObject.SetActive(true);
        ShopAudio.PlayOneShot(clickClip);
        Save();
    }
    public void RemovingHat()
    {
        DataManager.instanceData.curHatIndex = -1;
        MyItemList[curSelectedNum].isUsing = false;
        wearingButton.gameObject.SetActive(true);
        removingButton.gameObject.SetActive(false);
        ShopAudio.PlayOneShot(clickClip);
        Save();
    }
    public void PlayBtn()
    {
        ShopAudio.PlayOneShot(clickClip);
        SceneManager.LoadScene("GameScene");
    }
}