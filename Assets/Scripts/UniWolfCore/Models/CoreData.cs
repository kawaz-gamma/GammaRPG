using System.Collections.Generic;
using UnityEngine;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Map;

namespace UniWolfCore.Models
{

    /// <summary>
    /// コアデータとして常に居座る
    /// シングルトンとしてどこからでもアクセス可能
    /// Wolf関連ファイルの読み込みを行う
    /// </summary>
    public class CoreData
    {
        private static CoreData instance;

        public static CoreData Instance
        {
            get
            {
                if (instance == null)
                {
                    InitializeInstance();
                }
                return instance;
            }
        }

        private CoreData() { }

        public DatabaseMergedData userDB, changableDB, systemDB;

        public int[] systemVariables;
        public string[] systemStrings;
        public int[] normalVariables;
        public int[][] subVariables;
        public string[] stringVariables;

        public CommonEventList commonEvents;
        public int[][] commonEventVariables;
        public string[][] commonEventStrings;

        public TileSetData tileSetData;
        public MapData[] mapDataArray;
        public int[][] mapEventCurrentPages;
        public int[][][] mapVariables;
        public int currentMapID;

        public bool isReadCompleted;

        private static void InitializeInstance()
        {
            // なんらかの初期化処理
            instance = new CoreData();
            instance.isReadCompleted = false;
            var reader = new WolfDataReader();
            reader.ReadData(() => { instance.isReadCompleted = true; });
        }
    }

}