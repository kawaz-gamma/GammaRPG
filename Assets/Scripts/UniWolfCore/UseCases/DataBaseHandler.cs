using System;
using System.Collections.Generic;
using UniWolfCore.Models;
using WodiLib.Database;

namespace UniWolfCore.UseCases
{
    class DataBaseHandler
    {
        public int GetSystemDBInteger(int typeID,int dataID,int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.systemDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].IntValue;
        }

        public string GetSystemDBString(int typeID, int dataID, int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.systemDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].StringValue;
        }

        public void SetSystemDB(int typeID, int dataID, int itemID, int value)
        {
            DatabaseMergedData db = CoreData.Instance.systemDB;
            db.GetDataDescList(typeID)[dataID].ItemValueList[itemID] = new DBItemValue(value);
        }

        public int GetChangableDBInteger(int typeID, int dataID, int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.changableDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].IntValue;
        }

        public string GetChangableDBString(int typeID, int dataID, int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.changableDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].StringValue;
        }

        public void SetChangableDB(int typeID, int dataID, int itemID, int value)
        {
            DatabaseMergedData db = CoreData.Instance.changableDB;
            db.GetDataDescList(typeID)[dataID].ItemValueList[itemID] = new DBItemValue(value);
        }

        public int GetUserDBInteger(int typeID, int dataID, int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.userDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].IntValue;
        }

        public string GetUserDBString(int typeID, int dataID, int itemID)
        {
            DatabaseMergedData db = CoreData.Instance.userDB;
            return db.GetDataDescList(typeID)[dataID].ItemValueList[itemID].StringValue;
        }

        public void SetUserDB(int typeID, int dataID, int itemID, int value)
        {
            DatabaseMergedData db = CoreData.Instance.userDB;
            db.GetDataDescList(typeID)[dataID].ItemValueList[itemID] = new DBItemValue(value);
        }
    }
}
