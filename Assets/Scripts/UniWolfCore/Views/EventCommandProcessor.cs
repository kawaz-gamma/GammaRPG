using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniWolfCore.Models;
using WodiLib.Event.EventCommand;
using WodiLib.Map;

namespace UniWolfCore.Views
{
    /// <summary>
    /// 最終的にRXを用いてイベント管理する予定
    /// </summary>
    public class EventCommandProcessor
    {
        public Text messageText;
        public int eventID;

        EventCommandList commandList;

        WolfCommandReader commandReader;

        int commandId = 0;
        ICommandRunner runner;

        public EventCommandProcessor(Text messageText,int eventID,EventCommandList commandList)
        {
            this.messageText = messageText;
            this.eventID = eventID;
            this.commandList = commandList;
            commandReader = new WolfCommandReader(this);
            runner = commandReader.ReadCommand(commandList[0]);
        }

        public void Run()
        {
            if (commandId < commandList.Count)
            {
                if ( runner.Run())
                {
                    commandId++;
                    if (commandId < commandList.Count)
                    {
                        runner = commandReader.ReadCommand(commandList[commandId]);
                    }
                }
            }
        }
    }
}