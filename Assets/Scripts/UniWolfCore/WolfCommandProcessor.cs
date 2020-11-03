using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniWolfCore;
using WodiLib.Event.EventCommand;
using WodiLib.Map;
using WodiLib.UnityUtil.IO;

public class WolfCommandProcessor : MonoBehaviour
{
    [SerializeField]
    int mapNo;

    [SerializeField]
    Text messageText;

    string dataPath = Application.streamingAssetsPath + "/Project/Data/";
    WolfCommandReader commandReader;
    bool rendered;

    EventCommandList commandList;
    int commandId = 0;
    Func<bool> func;

    // Start is called before the first frame update
    void Start()
    {
        rendered = false;
        commandReader = new WolfCommandReader(messageText);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CoreData.Instance.isReadCompleted)
        {
            return;
        }

        if (!rendered)
        {
            rendered = true;
            ReadMap();
        }

        if (commandList!=null&& commandId < commandList.Count)
        {
            if (func())
            {
                commandId++;
                if (commandId < commandList.Count)
                {
                    func = commandReader.ReadCommand(commandList[commandId]);
                }
            }
        }
    }

    async void ReadMap()
    {
        var mapDataList = CoreData.Instance.systemDB.GetDataDescList(0);
        if (mapNo >= mapDataList.Count)
        {
            return;
        }

        // 本当はCoreDataから読み込みたい
        string mapPath = dataPath + mapDataList[mapNo].ItemValueList[0].StringValue.ToString();
        var mpsReader = new MpsFileReader();
        MapData mapData = await mpsReader.ReadFileAsync(mapPath);
        commandList = mapData.MapEvents[0].MapEventPageList[0].EventCommands;
        func = commandReader.ReadCommand(commandList[0]);
    }
}