using Assets.Scripts.UniWolfCore.UseCases;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniWolfCore.Models;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    class WolfCommandReader
    {
        MapDataHandler mapDataHandler;
        CommonEventHandler commonEventHandler;
        VariableHandler variableHandler;
        DataBaseHandler dbHandler;

        EventCommandProcessor processor;

        public WolfCommandReader(EventCommandProcessor processor) {
            this.processor = processor;

            mapDataHandler = new MapDataHandler();
            commonEventHandler = new CommonEventHandler();
            variableHandler = new VariableHandler();
            dbHandler = new DataBaseHandler();
        }

        public Func<bool> ReadCommand(IEventCommand command)
        {
            if (command.EventCommandCode == EventCommandCode.Message)
            {
                var commandData = command as Message;
                return () =>
                {
                    processor.messageText.text = commandData.Text;
                    return Input.GetKeyDown(KeyCode.Z);
                };
            }
            if (command.EventCommandCode == EventCommandCode.SetVariable)
            {
                var commandData = command as SetVariableBase;
                int val = 0;
            }

            return () => true;
        }

        public Func<bool>[] ReadCommands()
        {
            return new Func<bool>[1] { () => false };
        }

        int GetVal(int val)
        {
            if (Math.Abs(val) <= 999999)
            {
                return val;
            }
            else if (val / 100000 == 10)
            {
                // マップイベントのセルフ変数
                int eventID = (val - 1000000) / 10;
                int variableID = val % 10;
                return mapDataHandler.GetCurrentMapEventSelfVariable(eventID, variableID);
            }
            else if (val / 100000 == 11)
            {
                // このマップイベントのセルフ変数
                int variableID = val % 10;
                return mapDataHandler.GetCurrentMapEventSelfVariable(processor.eventID, variableID);
            }
            else if (val / 1000000 == 15)
            {
                int eventID = (val - 15000000) / 100;
                int variableID = val % 100;
                return commonEventHandler.GetSelfVariableInteger(eventID, variableID);
            }
            else if (val / 100000 == 16)
            {
                int variableID = val % 100;
                return commonEventHandler.GetSelfVariableInteger(processor.eventID, variableID);
            }
            else if (val / 100000 == 20)
            {
                int variableID = val - 2000000;
                return variableHandler.GetNormalVariable(variableID);
            }
            else if (val / 1000000 == 2)
            {
                int subID = (val - 2000000) / 100000 - 1;
                int variableID = val % 100000;
                return variableHandler.GetSubVariable(subID, variableID);
            }
            else if (val / 1000000 == 3)
            {
                int variableID = val - 3000000;
                return variableHandler.GetStringVariableInteger(variableID);
            }
            else if (val / 1000000 == 8)
            {
                int max = val - 8000000;
                System.Random rand = new System.Random();
                return rand.Next(0, max + 1);
            }
            else if (val / 100000 == 90)
            {
                int variableID = val - 9000000;
                return variableHandler.GetSystemVariable(variableID);
            }
            else if (val / 10000 == 910)
            {
                // イベントの座標関連
                int eventID = val - 9100000 / 10;
            }
            else if (val / 10000 == 918)
            {
                // 主人公・仲間の座標関連
            }
            else if (val / 10000 == 918)
            {
                // このイベントの座標関連
            }
            else if (val / 100000 == 99)
            {
                int variableID = val - 9900000;
                return variableHandler.GetSystemStringInteger(variableID);
            }
            else if (val / 100000000 == 10)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                return dbHandler.GetSystemDBInteger(typeID, dataID, itemID);
            }
            else if (val / 100000000 == 11)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                return dbHandler.GetChangableDBInteger(typeID, dataID, itemID);
            }
            else if (val / 100000000 == 13)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                return dbHandler.GetUserDBInteger(typeID, dataID, itemID);
            }

            return 0;
        }
    }
}
