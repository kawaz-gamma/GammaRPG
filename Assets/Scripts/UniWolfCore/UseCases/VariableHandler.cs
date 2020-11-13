using System;
using System.Collections.Generic;
using UniWolfCore.Models;

namespace UniWolfCore.UseCases
{
    class VariableHandler
    {
        public int GetSystemVariable(int variableID)
        {
            return CoreData.Instance.systemVariables[variableID];
        }

        public void SetSystemVariable(int variableID,int value)
        {
            CoreData.Instance.systemVariables[variableID] = value;
        }

        public int GetSystemStringInteger(int variableID)
        {
            return GetIntegerFromString(CoreData.Instance.systemStrings[variableID]);
        }

        public void SetSystemString(int variableID,int value)
        {
            CoreData.Instance.systemStrings[variableID] = value.ToString();
        }

        public int GetNormalVariable(int variableID)
        {
            return CoreData.Instance.normalVariables[variableID];
        }

        public void SetNormalVariable(int variableID,int value)
        {
            CoreData.Instance.normalVariables[variableID] = value;
        }

        public int GetSubVariable(int subID,int variableID)
        {
            return CoreData.Instance.subVariables[subID][variableID];
        }

        public void SetSubVariable(int subID, int variableID,int value)
        {
            CoreData.Instance.subVariables[subID][variableID] = value;
        }

        public int GetStringVariableInteger(int variableID)
        {
            return GetIntegerFromString(CoreData.Instance.stringVariables[variableID]);
        }

        public void SetStringVariable(int variableID,int value)
        {
            CoreData.Instance.stringVariables[variableID] = value.ToString();
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
