using System;
using UnityEngine;
using WodiLib.Map;
using WodiLib.UnityUtil.IO;
using System.Threading.Tasks;
using WodiLib.Database;

namespace UniWolfCore.Models
{
    class WolfDataReader : ICoreDataReader
    {
        public WolfDataReader()
        {

        }

        public async Task ReadData(Action onDone)
        {
            string dataPath = Application.streamingAssetsPath + "/Project/Data/";
            CoreData.Instance.userDB = await ReadDB(dataPath, "DataBase");
            CoreData.Instance.changableDB = await ReadDB(dataPath, "CDataBase");
            CoreData.Instance.systemDB = await ReadDB(dataPath, "SysDataBase");
            CoreData.Instance.tileSetData = await ReadTileSetData(dataPath);
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
    }
}
