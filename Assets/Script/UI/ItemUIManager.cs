using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{
    public GameObject[] Item;

    public GameObject BaseWeapon;
    public GameObject ItemWepaon;

    public GameObject[] SelectLine; // 0 base / 1 Item

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemUIChange(bool checkd, int array = 0) => Item[array].SetActive(checkd);

    public void UIWeaponChange(bool check1, bool check2)
    {
        BaseWeapon.SetActive(check1);
        ItemWepaon.SetActive(check2);
    }
    public void UISelectChange(bool check1, bool check2)
    {
        SelectLine[0].SetActive(check1);
        SelectLine[1].SetActive(check2);
    }
}
