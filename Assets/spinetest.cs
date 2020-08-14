using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spinetest : MonoBehaviour
{
    public GameObject tp;
    public int tpNum = 0;
    public Text t;

    public void AddTP(int num)
    {
        tpNum += num;
        t.text = tpNum.ToString();

        for (int i = 0; i < num; i++)
        {
            Instantiate(tp, new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 0f), 0), Quaternion.identity);
        }
    }
}
