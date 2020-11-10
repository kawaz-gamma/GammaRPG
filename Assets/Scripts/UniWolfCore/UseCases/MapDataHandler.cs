using System;
using System.Collections.Generic;
using UniWolfCore.Models;
using WodiLib.Map;

namespace Assets.Scripts.UniWolfCore.UseCases
{
    class MapDataHandler
    {
        public int GetCurrentMapEventSelfVariable(int eventID,int variableID)
        {
            int mapID = CoreData.Instance.currentMapID;
            return CoreData.Instance.mapVariables[mapID][eventID][variableID];
        }

        public void SetCurrentMapEventSelfVariable(int eventID, int variableID,int value)
        {
            int mapID = CoreData.Instance.currentMapID;
            CoreData.Instance.mapVariables[mapID][eventID][variableID] = value;
        }

        /// <summary>
        /// 要修正
        /// イベントの現在ページ判定部を未実装のため，1ページ目の情報のみを使用している
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int GetEventPositionVariable(int eventID, int val)
        {
            int mapID = CoreData.Instance.currentMapID;
            MapEvent mapEvent = CoreData.Instance.mapDataArray[mapID].MapEvents[eventID];
            switch (val)
            {
                case 0:
                    return mapEvent.Position.X;
                case 1:
                    return mapEvent.Position.Y;
                case 2:
                    return mapEvent.Position.X * 2;
                case 3:
                    return mapEvent.Position.Y * 2;
                case 4:
                    return 0;
                case 5:
                    return mapEvent.MapEventPageList[0].ShadowGraphicId;
                case 6:
                    return mapEvent.MapEventPageList[0].GraphicInfo.InitDirection.Code;
            }
            return 0;
        }
    }
}
