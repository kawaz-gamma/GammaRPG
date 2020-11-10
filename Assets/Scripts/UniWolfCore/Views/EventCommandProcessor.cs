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
    public class EventCommandProcessor
    {
        public Text messageText;
        public int eventID;

        EventCommandList commandList;

        WolfCommandReader commandReader;

        int commandId = 0;
        Func<bool> func;

        public EventCommandProcessor(Text messageText,int eventID,EventCommandList commandList)
        {
            this.messageText = messageText;
            this.eventID = eventID;
            this.commandList = commandList;
            commandReader = new WolfCommandReader(this);
            func = commandReader.ReadCommand(commandList[0]);
        }

        public void Run()
        {
            if (commandId < commandList.Count)
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
    }
}