using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniWolfCore.Models;
using UniWolfCore.UseCases;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    public class MainEventProcessor : MonoBehaviour
    {
        [SerializeField]
        Text text;

        EventCommandProcessor processor;
        bool readDone = false;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!readDone&&CoreData.Instance.isReadCompleted)
            {
                readDone = true;
                MapDataHandler handler = new MapDataHandler();
                int mapID = CoreData.Instance.currentMapID;
                Debug.Log(mapID);
                EventCommandList events
                    = CoreData.Instance.mapDataArray[mapID].GetMapEvent(0).MapEventPageList[0].EventCommands;
                processor = new EventCommandProcessor(text,0, events);
                return;
            }

            if (readDone)
            {
                processor.Run();
            }
        }
    }
}