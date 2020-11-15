using UnityEngine;
using UnityEngine.UI;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    class ChoiceStartRunner : ICommandRunner
    {
        RectTransform choiceWindow;
        string str;

        public ChoiceStartRunner(RectTransform choiceWindow, ChoiceStart choiceStartCommand)
        {
            this.choiceWindow = choiceWindow;

            choiceWindow.gameObject.SetActive(true);

        }

        public bool Run()
        {
            return true;
        }
    }
}