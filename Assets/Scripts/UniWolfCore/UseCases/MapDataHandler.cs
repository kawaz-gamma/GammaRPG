using System;
using System.Collections.Generic;
using UniWolfCore.Models;
using WodiLib.Map;

namespace UniWolfCore.UseCases
{
    class MapDataHandler
    {
        public int GetCurrentMapEventSelfVariable(int eventID, int variableID)
        {
            int mapID = CoreData.Instance.currentMapID;
            return CoreData.Instance.mapVariables[mapID][eventID][variableID];
        }

        public void SetCurrentMapEventSelfVariable(int eventID, int variableID, int value)
        {
            int mapID = CoreData.Instance.currentMapID;
            CoreData.Instance.mapVariables[mapID][eventID][variableID] = value;
        }

        /// <summary>
        /// イベントの現在ページ設定処理は未実装
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int GetEventPositionVariable(int eventID, int val)
        {
            int mapID = CoreData.Instance.currentMapID;
            int pageID = CoreData.Instance.mapEventCurrentPages[mapID][eventID];
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
                    return mapEvent.MapEventPageList[pageID].ShadowGraphicId;
                case 6:
                    return mapEvent.MapEventPageList[pageID].GraphicInfo.InitDirection.Code;
            }
            return 0;
        }

        /// <summary>
        /// WodiLibのアクセシビリティや型を修正する必要あり
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="typeID"></param>
        /// <param name="value"></param>
        public void SetEventPositionVariable(int eventID, int typeID, int value)
        {
            int mapID = CoreData.Instance.currentMapID;
            int pageID = CoreData.Instance.mapEventCurrentPages[mapID][eventID];
            MapEvent mapEvent = CoreData.Instance.mapDataArray[mapID].MapEvents[eventID];
            switch (typeID)
            {
                case 0:
                    mapEvent.Position = new Position(value, mapEvent.Position.Y);
                    break;
                case 1:
                    mapEvent.Position = new Position(mapEvent.Position.X,value);
                    break;
                case 2:
                    mapEvent.Position = new Position(value/2, mapEvent.Position.Y);
                    break;
                case 3:
                    mapEvent.Position = new Position(mapEvent.Position.X, value/2);
                    break;
                case 4:
                    break;
                case 5:
                    mapEvent.MapEventPageList[pageID].ShadowGraphicId = (byte)value;
                    break;
                case 6:
                    mapEvent.MapEventPageList[pageID].GraphicInfo.InitDirection 
                        = CharaChipDirection.FromByte((byte)value);
                    break;
            }
        }
    }
}
