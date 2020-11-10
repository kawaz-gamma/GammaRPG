using System;
using System.Collections.Generic;
using UniWolfCore.Models;

namespace Assets.Scripts.UniWolfCore.UseCases
{
    class CommonEventHandler
    {
        public int GetSelfVariableInteger(int eventID,int variableID)
        {
            if (variableID >= 5 && variableID <= 9)
            {
                if (int.TryParse(CoreData.Instance.commonEventStrings[eventID][variableID - 5], out int i))
                {
                    return i;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (variableID > 9)
                {
                    variableID -= 5;
                }
                return CoreData.Instance.commonEventVariables[eventID][variableID];
            }
        }
    }
}
