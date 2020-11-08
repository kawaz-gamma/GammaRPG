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
            CoreData.Instance.commonEvents = await ReadCommonEventList(dataPath);
            CoreData.Instance.tileSetData = await ReadTileSetData(dataPath);
            await InitializeMapVariables(dataPath);
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

        async Task<CommonEventList> ReadCommonEventList(string dataPath)
        {
            string datPath= $"{dataPath}BasicData/CommonEvent.dat";

            var reader = new CommonEventDatFileReader();
            CommonEventData data = await reader.ReadFileAsync(datPath);
            return data.CommonEventList;
        }

        async Task<TileSetData> ReadTileSetData(string dataPath)
        {
            string tileSetPath = dataPath + "BasicData/TileSetData.dat";
            var reader = new TileSetDataFileReader();
            return await reader.ReadFileAsync(tileSetPath);
        }

        async Task InitializeMapVariables(string dataPath)
        {
            DatabaseDataDescList mapSettingList = CoreData.Instance.systemDB.GetDataDescList(0);

            CoreData.Instance.mapDataArray = new MapData[mapSettingList.Count];
            CoreData.Instance.mapVariablesArray = new int[mapSettingList.Count][][];
            for (int i = 0; i < mapSettingList.Count; i++)
            {
                string mapPath = dataPath + mapSettingList[i].ItemValueList[0].StringValue.ToString();
                var mpsReader = new MpsFileReader();
                MapData mapData= await mpsReader.ReadFileAsync(mapPath);
                CoreData.Instance.mapDataArray[i] = mapData;
                CoreData.Instance.mapVariablesArray[i] = new int[mapData.MapEvents.Count][];
                for(int j = 0; j < mapData.MapEvents.Count; j++)
                {
                    CoreData.Instance.mapVariablesArray[i][j] = new int[10];
                }
            }
        }
    }
}
