using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UniWolfCore.UseCases
{
    class ValueCallHandler
    {
        MapDataHandler mapDataHandler;
        CommonEventHandler commonEventHandler;
        VariableHandler variableHandler;
        DataBaseHandler dbHandler;

        public ValueCallHandler()
        {
            mapDataHandler = new MapDataHandler();
            commonEventHandler = new CommonEventHandler();
            variableHandler = new VariableHandler();
            dbHandler = new DataBaseHandler();
        }

        public int GetVal(int currentID,int val)
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
                return mapDataHandler.GetCurrentMapEventSelfVariable(currentID, variableID);
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
                return commonEventHandler.GetSelfVariableInteger(currentID, variableID);
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
                int eventID = val - 9100000 / 10;
                return mapDataHandler.GetEventPositionVariable(eventID, val % 10);
            }
            else if (val / 10000 == 918)
            {
                // 主人公・仲間の座標関連
                // 未実装
            }
            else if (val / 10000 == 918)
            {
                return mapDataHandler.GetEventPositionVariable(currentID, val % 10);
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



        public void SetVal(int currentID, int val, int assignValue)
        {
            if (Math.Abs(val) <= 999999)
            {
                // 何もしない
                return;
            }
            else if (val / 100000 == 10)
            {
                // マップイベントのセルフ変数
                int eventID = (val - 1000000) / 10;
                int variableID = val % 10;
                mapDataHandler.SetCurrentMapEventSelfVariable(eventID, variableID, assignValue);
            }
            else if (val / 100000 == 11)
            {
                // このマップイベントのセルフ変数
                int variableID = val % 10;
                mapDataHandler.SetCurrentMapEventSelfVariable(currentID, variableID, assignValue);
            }
            else if (val / 1000000 == 15)
            {
                int eventID = (val - 15000000) / 100;
                int variableID = val % 100;
                commonEventHandler.SetSelfVariableInteger(eventID, variableID, assignValue);
            }
            else if (val / 100000 == 16)
            {
                int variableID = val % 100;
                commonEventHandler.SetSelfVariableInteger(currentID, variableID, assignValue);
            }
            else if (val / 100000 == 20)
            {
                int variableID = val - 2000000;
                variableHandler.SetNormalVariable(variableID, assignValue);
            }
            else if (val / 1000000 == 2)
            {
                int subID = (val - 2000000) / 100000 - 1;
                int variableID = val % 100000;
                variableHandler.SetSubVariable(subID, variableID, assignValue);
            }
            else if (val / 1000000 == 3)
            {
                int variableID = val - 3000000;
                variableHandler.SetStringVariable(variableID, assignValue);
            }
            else if (val / 1000000 == 8)
            {
                // 乱数，何もしない
                return;
            }
            else if (val / 100000 == 90)
            {
                int variableID = val - 9000000;
                variableHandler.SetSystemVariable(variableID, assignValue);
            }
            else if (val / 10000 == 910)
            {
                int eventID = val - 9100000 / 10;
                mapDataHandler.SetEventPositionVariable(eventID, val % 10, assignValue);
            }
            else if (val / 10000 == 918)
            {
                // 主人公・仲間の座標関連
                // 未実装
            }
            else if (val / 10000 == 918)
            {
                mapDataHandler.SetEventPositionVariable(currentID, val % 10, assignValue);
            }
            else if (val / 100000 == 99)
            {
                int variableID = val - 9900000;
                variableHandler.SetSystemString(variableID, assignValue);
            }
            else if (val / 100000000 == 10)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                dbHandler.SetSystemDBInteger(typeID, dataID, itemID, assignValue);
            }
            else if (val / 100000000 == 11)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                dbHandler.SetChangableDBInteger(typeID, dataID, itemID, assignValue);
            }
            else if (val / 100000000 == 13)
            {
                int typeID = val / 1000000 % 100;
                int dataID = val / 100 % 10000;
                int itemID = val % 100;
                dbHandler.SetUserDBInteger(typeID, dataID, itemID, assignValue);
            }
        }
    }
}
