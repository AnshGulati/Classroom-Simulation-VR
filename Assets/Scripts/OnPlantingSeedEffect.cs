using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OnPlantingSeedEffect : MonoBehaviour
{
    [SerializeField] private GameObject sapling;
    [SerializeField] private GameObject smallTree;
    [SerializeField] private GameObject mediumTree;
    [SerializeField] private GameObject largeTree;
    [SerializeField] private GameObject soilPot;
    public TMP_Text treeCount;
    public TMP_Text carFootCount;
    public TMP_Text nitrogenCount;
    public TMP_Text surfaceCount;
    public TMP_Text waterCount;
    public TMP_Text humidCount;
    public TMP_Text tempCount;
    public static int treeNum;
    public static float waterNum;
    public static float humidNum;
    public static float carFootnum;
    public static float nitrogenNum;
    public static float surfaceNum;
    public static float tempNum;
    public AudioSource treeFX;

    

    public void Start()
    {
        carFootnum = 1000;
        nitrogenNum = 100;
        surfaceNum = 100;
        tempNum = 100;
        waterNum = 68;
        humidNum = 30;
        treeCount.text = "0";
        carFootCount.text = "1000" + "lbs";
        nitrogenCount.text = "100" + "%";
        surfaceCount.text = "100" + "%";
        waterCount.text = "68" + "%";
        humidCount.text = "30" + "%";
        tempCount.text = "100" + "deg";
    }
    public void Update()
    {
        treeCount.text = treeNum.ToString();
        carFootCount.text = carFootnum.ToString() + "lbs";
        nitrogenCount.text = nitrogenNum.ToString() + "%";
        surfaceCount.text = surfaceNum.ToString() + "%";
        waterCount.text = waterNum.ToString() + "%";
        humidCount.text = humidNum.ToString() + "%";
        tempCount.text = tempNum.ToString() + "deg";

        if (treeNum == 3)
        {
            RenderSettings.fogDensity = 0.01f;
        }
        if (treeNum == 8)
        {
            RenderSettings.fogDensity = 0.008f;
        }
        if (treeNum == 12)
        {
            RenderSettings.fogDensity = 0.006f;
        }
        if (treeNum == 18)
        {
            RenderSettings.fogDensity = 0.004f;
        }
        if (treeNum == 25)
        {
            RenderSettings.fogDensity = 0.00f;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Seed"))
        {
            Debug.Log("Seed Planted");
        }

        if (other.gameObject.CompareTag("Seed"))
        {

            Destroy(other.gameObject);
            StartCoroutine(SaplingSpawner());
        }
    }
    IEnumerator SaplingSpawner()
    {
        yield return new WaitForSeconds(1f);
        sapling.gameObject.SetActive(true);
        treeFX.Play();

        yield return new WaitForSeconds(2f);
        sapling.gameObject.SetActive(false);
        smallTree.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        smallTree.gameObject.SetActive(false);
        soilPot.gameObject.SetActive(false);
        mediumTree.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        mediumTree.gameObject.SetActive(false);
        largeTree.gameObject.SetActive(true);
        treeNum++;
        carFootnum -= 48;
        nitrogenNum = nitrogenNum - ((nitrogenNum * 2) / 100);
        surfaceNum = surfaceNum - ((surfaceNum * 3) / 100);
        waterNum = waterNum + ((waterNum * 3) / 100);
        humidNum = humidNum + ((humidNum * 4) / 100);
        tempNum = tempNum - 1.23f;

    }

    
    
}
