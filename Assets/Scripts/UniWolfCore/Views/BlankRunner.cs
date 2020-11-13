using UnityEngine;
using UnityEngine.UI;

namespace UniWolfCore.Views
{
    class BlankRunner : ICommandRunner
    {

        public bool Run()
        {
            return true;
        }
    }
}