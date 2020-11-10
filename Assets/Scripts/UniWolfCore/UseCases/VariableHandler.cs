using System;
using System.Collections.Generic;
using UniWolfCore.Models;

namespace Assets.Scripts.UniWolfCore.UseCases
{
    class VariableHandler
    {
        public int GetSystemVariable(int variableID)
        {
            return CoreData.Instance.systemVariables[variableID];
        }

        public int GetSystemStringInteger(int variableID)
        {
            return GetIntegerFromString(CoreData.Instance.systemStrings[variableID]);
        }

        public int GetNormalVariable(int variableID)
        {
            return CoreData.Instance.normalVariables[variableID];
        }

        public int GetSubVariable(int subID,int variableID)
        {
            return CoreData.Instance.subVariables[subID][variableID];
        }

        public int GetStringVariableInteger(int variableID)
        {
            return GetIntegerFromString(CoreData.Instance.stringVariables[variableID]);
        }

        int GetIntegerFromString(string str)
        {
            string numeric = "";
            foreach(char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    numeric += c;
                }
            }

            if(int.TryParse(numeric,out int i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
    }
}
