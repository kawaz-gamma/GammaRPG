using System.Linq;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    class WolfCommandReader
    {
        EventCommandProcessor processor;

        public WolfCommandReader(EventCommandProcessor processor) {
            this.processor = processor;
        }

        public ICommandRunner ReadCommand(IEventCommand command)
        {
            if (command.EventCommandCode == EventCommandCode.Message)
            {
                var commandData = command as Message;
                return new MessageRunner(processor.messageText, commandData.Text);
            }
            if (command.EventCommandCode == EventCommandCode.SetVariable)
            {
                var commandData = command as SetVariableBase;
                return new SetVariableRunner(processor.eventID, commandData);
            }
            if (command.EventCommandCode==EventCommandCode.ChoiceStart)
            {
                var commandData = command as ChoiceStart;

            }

            return new BlankRunner();
        }

        public ICommandRunner[] ReadCommands(EventCommandList list)
        {
            var runners = new ICommandRunner[list.Count];
            for(int i = 0; i < list.Count; i++)
            {
                runners[i] = ReadCommand(list[i]);
            }
            return runners;
        }
    }
}
