using System;
using UnityEngine;
using WodiLib.Map;
using WodiLib.UnityUtil.IO;
using System.Threading.Tasks;
using WodiLib.Database;
using System.Collections.Generic;
using WodiLib.Common;

namespace UniWolfCore.Models
{
    class WolfDataReader : ICoreDataReader
    {
        public WolfDataReader() { }

        public async Task ReadData(Action onDone)
        {
            string dataPath = Application.streamingAssetsPath + "/Project/Data/";
            CoreData.Instance.userDB = await ReadDB(dataPath, "DataBase");
            CoreData.Instance.changableDB = await ReadDB(dataPath, "CDataBase");
            CoreData.Instance.systemDB = await ReadDB(dataPath, "SysDataBase");
            CoreData.Instance.tileSetData = await ReadTileSetData(dataPath);
            await InitializeCommonEventList(dataPath);
            await InitializeMapData(dataPath);
            InitializeVariables();
            InitializeCurrentMapID();
            onDone();
        }

        async Task<DatabaseMergedData> ReadDB(string dataPath, string dbName)
        {
            string datPath = $"{dataPath}BasicData/{dbName}.dat";
            string projectPath = $"{dataPath}BasicData/{dbName}.project";

            var dbReader = new DatabaseMergedDataReader();
            DatabaseMergedData data = await dbReader.ReadFilesAsync(datPath, projectPath);
            return data;
        }

        async Task<TileSetData> ReadTileSetData(string dataPath)
        {
            string tileSetPath = dataPath + "BasicData/TileSetData.dat";
            var reader = new TileSetDataFileReader();
            return await reader.ReadFileAsync(tileSetPath);
        }

        async Task InitializeCommonEventList(string dataPath)
        {
            string datPath = $"{dataPath}BasicData/CommonEvent.dat";

            var reader = new CommonEventDatFileReader();
            CommonEventData data = await reader.ReadFileAsync(datPath);
            CoreData.Instance.commonEvents = data.CommonEventList;

            int eventCount = data.CommonEventList.Count;
            CoreData.Instance.commonEventVariables = new int[eventCount][];
            CoreData.Instance.commonEventStrings = new string[eventCount][];
            for (int i = 0; i < eventCount; i++)
            {
                CoreData.Instance.commonEventVariables[i] = new int[95];
                CoreData.Instance.commonEventStrings[i] = new string[5];
            }
        }

        async Task InitializeMapData(string dataPath)
        {
            DatabaseDataDescList mapSettingList = CoreData.Instance.systemDB.GetDataDescList(0);

            CoreData.Instance.mapDataArray = new MapData[mapSettingList.Count];
            CoreData.Instance.mapVariables = new int[mapSettingList.Count][][];
            for (int i = 0; i < mapSettingList.Count; i++)
            {
                string mapPath = dataPath + mapSettingList[i].ItemValueList[0].StringValue.ToString();
                var mpsReader = new MpsFileReader();
                MapData mapData= await mpsReader.ReadFileAsync(mapPath);
                CoreData.Instance.mapDataArray[i] = mapData;
                CoreData.Instance.mapVariables[i] = new int[mapData.MapEvents.Count][];
                for(int j = 0; j < mapData.MapEvents.Count; j++)
                {
                    CoreData.Instance.mapVariables[i][j] = new int[10];
                }
            }
        }

        void InitializeVariables()
        {
            int systemVariableCnt = CoreData.Instance.systemDB.GetDataDescList(6).Count;
            CoreData.Instance.systemVariables = new int[systemVariableCnt];

            int systemStringCnt = CoreData.Instance.systemDB.GetDataDescList(5).Count;
            CoreData.Instance.systemStrings = new string[systemStringCnt];

            int normalVariableCnt = CoreData.Instance.systemDB.GetDataDescList(14).Count;
            CoreData.Instance.normalVariables = new int[normalVariableCnt];

            CoreData.Instance.subVariables = new int[9][];
            for(int i = 0; i < 9; i++)
            {
                int variableCnt = CoreData.Instance.systemDB.GetDataDescList(15 + i).Count;
                CoreData.Instance.subVariables[i] = new int[variableCnt];
            }

            int stringVariableCnt= CoreData.Instance.systemDB.GetDataDescList(4).Count;
            CoreData.Instance.stringVariables = new string[stringVariableCnt];
        }

        void InitializeCurrentMapID()
        {
            DatabaseDataDescList posSettingList = CoreData.Instance.systemDB.GetDataDescList(7);
            CoreData.Instance.currentMapID = posSettingList[0].ItemValueList[0].IntValue;
        }
    }
}
