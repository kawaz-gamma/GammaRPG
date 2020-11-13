using UniWolfCore.UseCases;
using System;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    class SetVariableRunner:ICommandRunner
    {
        int eventID;
        SetVariableBase setVariableCommand;

        public SetVariableRunner(int eventID,SetVariableBase setVariableCommand)
        {
            this.eventID = eventID;
            this.setVariableCommand = setVariableCommand;
        }

        public bool Run()
        {
            ValueCallHandler handler = new ValueCallHandler();

            int right1 = handler.GetVal(eventID, setVariableCommand.RightSide1);
            int right2 = handler.GetVal(eventID, setVariableCommand.RightSide2);
            CalculateOperator calcOperator = setVariableCommand.CalculateOperator;
            NumberAssignmentOperator assignOperator = setVariableCommand.AssignmentOperator;

            int assignVal = 0;
            if (calcOperator == CalculateOperator.Addition)
            {
                assignVal = right1 + right2;
            }
            if (calcOperator == CalculateOperator.Subtraction)
            {
                assignVal = right1 - right2;
            }
            if (calcOperator == CalculateOperator.Multiplication)
            {
                assignVal = right1 * right2;
            }
            if (calcOperator == CalculateOperator.Division)
            {
                assignVal = right1 / right2;
            }
            if (calcOperator == CalculateOperator.Modulo)
            {
                assignVal = right1 % right2;
            }
            if (calcOperator == CalculateOperator.BitAnd)
            {
                assignVal = (right1 & right2);
            }
            if (calcOperator == CalculateOperator.Between)
            {
                System.Random rand = new System.Random();
                assignVal = rand.Next(Math.Min(right1, right2), Math.Max(right1, right2) + 1);
            }

            int left = handler.GetVal(eventID, setVariableCommand.LeftSide);
            if (assignOperator == NumberAssignmentOperator.Assign)
            {

            }
            if (assignOperator == NumberAssignmentOperator.Addition)
            {
                assignVal += left;
            }
            if (assignOperator == NumberAssignmentOperator.Subtraction)
            {
                assignVal = left - assignVal;
            }
            if (assignOperator == NumberAssignmentOperator.Multiplication)
            {
                assignVal *= left;
            }
            if (assignOperator == NumberAssignmentOperator.Division)
            {
                assignVal = left / assignVal;
            }
            if (assignOperator == NumberAssignmentOperator.Modulo)
            {
                assignVal = left % assignVal;
            }
            if (assignOperator == NumberAssignmentOperator.LowerBound)
            {
                assignVal = Math.Max(left, assignVal);
            }
            if (assignOperator == NumberAssignmentOperator.UpperBound)
            {
                assignVal = Math.Min(left, assignVal);
            }
            if (assignOperator == NumberAssignmentOperator.Absolute)
            {
                assignVal = Math.Abs(assignVal);
            }

           handler.SetVal(eventID, setVariableCommand.LeftSide, assignVal);
            return true;
        }
    }
}
