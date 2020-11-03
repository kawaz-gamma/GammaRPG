using System;
using UnityEngine;
using UnityEngine.UI;
using WodiLib.Event.EventCommand;

namespace UniWolfCore
{
    class WolfCommandReader
    {
        Text messageText;

        public WolfCommandReader(Text messageText)
        {
            this.messageText = messageText;
        }

        public Func<bool> ReadCommand(IEventCommand command)
        {
            if (command.EventCommandCode == EventCommandCode.Message)
            {
                var commandData = command as Message;
                return () =>
                {
                    messageText.text = commandData.Text;
                    return Input.GetKeyDown(KeyCode.Z);
                };
            }

            return () => true;
        }

        public Func<bool>[] ReadCommands()
        {
            return new Func<bool>[1] { () => false };
        }
    }
}
