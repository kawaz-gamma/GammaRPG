using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusStarter : MonoBehaviour
{
    [SerializeField]
    Flowchart flowchart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            flowchart.SendFungusMessage("hoge");
        }
    }
}
