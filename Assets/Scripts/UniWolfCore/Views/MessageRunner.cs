using UnityEngine;
using UnityEngine.UI;

namespace UniWolfCore.Views
{
    class MessageRunner : ICommandRunner
    {
        Text text;
        string str;

        public MessageRunner(Text text, string str)
        {
            this.text = text;
            this.str = str;

            text.enabled = true;
            text.text = "";
        }

        public bool Run()
        {
            if (!text.enabled)
            {
                return true;
            }

            text.text = str;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Press");
                text.enabled = false;
            }
            return false;
        }
    }
}