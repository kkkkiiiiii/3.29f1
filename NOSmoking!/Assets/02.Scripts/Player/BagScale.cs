using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScale : MonoBehaviour
{
    private float XBigScale = 0.1f;
    private float YBigScale = 0.1f;
    private float ZBigScale = 0.1f;
    
    public void GetSizeValue(int buttsNum)
    {
        Debug.Log("Size");
        switch (buttsNum)
        {
            case 3:
                XBigScale = 0.2f;
                YBigScale = 0.1f;
                ZBigScale = 0.1f;
                break;
            case 6:
                XBigScale = 0.2f;
                YBigScale = 0.2f;
                ZBigScale = 0.1f;
                break ;
            case 9:
                XBigScale = 0.2f;
                YBigScale = 0.2f;
                ZBigScale = 0.2f;
                break;
            case 12:
                XBigScale = 0.3f;
                YBigScale = 0.2f;
                ZBigScale = 0.2f;
                break;
            case 15:
                XBigScale = 0.3f;
                YBigScale = 0.3f;
                ZBigScale = 0.2f;
                break;
            case 18:
                XBigScale = 0.3f;
                YBigScale = 0.3f;
                ZBigScale = 0.3f;
                break;
            case 21:
                XBigScale = 0.4f;
                YBigScale = 0.3f;
                ZBigScale = 0.3f;
                break;
            case 24:
                XBigScale = 0.4f;
                YBigScale = 0.4f;
                ZBigScale = 0.3f;
                break;
            case 27:
                XBigScale = 0.4f;
                YBigScale = 0.4f;
                ZBigScale = 0.4f;
                break;
            default:
                break;
        }

        ScaleUp();
    }
    void ScaleUp()
    {        
        this.gameObject.transform.localScale = new Vector3(XBigScale, YBigScale, ZBigScale);
    }
}
