using UnityEngine;
using UniWolfCore.Models;
using WodiLib.Database;
using WodiLib.Map;

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
    public TileSetData tileSetData;
    public bool isReadCompleted;

    private static void InitializeInstance()
    {
        // なんらかの初期化処理
        instance = new CoreData();
        instance.isReadCompleted = false;
        var reader = new WolfDataReader();
        _ = reader.ReadData(() => { instance.isReadCompleted = true; });
    }
}
