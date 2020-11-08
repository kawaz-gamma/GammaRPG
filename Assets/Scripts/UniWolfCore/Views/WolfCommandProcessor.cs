using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniWolfCore.Models;
using UniWolfCore.UseCases;
using WodiLib.Event.EventCommand;
using WodiLib.Map;

public class WolfCommandProcessor : MonoBehaviour
{
    [SerializeField]
    int mapNo;

    [SerializeField]
    Text messageText;

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
            ReadEvent();
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

    void ReadEvent()
    {
        var hoge = CoreData.Instance.commonEvents[63];
        var line = hoge.EventCommands[21];
        MapData mapData = CoreData.Instance.mapDataArray[mapNo];
        commandList = mapData.MapEvents[0].MapEventPageList[0].EventCommands;
        func = commandReader.ReadCommand(commandList[0]);
    }
}