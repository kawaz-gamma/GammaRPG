using Assets.Scripts.UniWolfCore.UseCases;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniWolfCore.Models;
using WodiLib.Event.EventCommand;

namespace UniWolfCore.Views
{
    class WolfCommandReader
    {
        ValueCallHandler valueCallHandler;

        EventCommandProcessor processor;

        public WolfCommandReader(EventCommandProcessor processor) {
            this.processor = processor;

            valueCallHandler = new ValueCallHandler();
        }

        public Func<bool> ReadCommand(IEventCommand command)
        {
            if (command.EventCommandCode == EventCommandCode.Message)
            {
                var commandData = command as Message;
                return () =>
                {
                    processor.messageText.text = commandData.Text;
                    return Input.GetKeyDown(KeyCode.Z);
                };
            }
            if (command.EventCommandCode == EventCommandCode.SetVariable)
            {
                var commandData = command as SetVariableBase;
                return () =>
                {
                    int right1 = valueCallHandler.GetVal(processor.eventID, commandData.RightSide1);
                    int right2 = valueCallHandler.GetVal(processor.eventID, commandData.RightSide2);
                    int assignVal = 0;
                    if (commandData.CalculateOperator == CalculateOperator.Addition)
                    {
                        assignVal = right1 + right2;
                    }
                    if (commandData.CalculateOperator == CalculateOperator.Subtraction)
                    {
                        assignVal = right1 - right2;
                    }
                    if (commandData.CalculateOperator == CalculateOperator.Multiplication)
                    {
                        assignVal = right1 * right2;
                    }
                    if (commandData.CalculateOperator == CalculateOperator.Division)
                    {
                        assignVal = right1 / right2;
                    }
                    if (commandData.CalculateOperator == CalculateOperator.Modulo)
                    {
                        assignVal = right1 % right2;
                    }
                    if (commandData.CalculateOperator == CalculateOperator.BitAnd)
                    {
                        assignVal = (right1 & right2);
                    }
                    if (commandData.CalculateOperator == CalculateOperator.Between)
                    {
                        System.Random rand = new System.Random();
                        assignVal = rand.Next(Math.Min(right1, right2), Math.Max(right1, right2) + 1);
                    }

                    int left = valueCallHandler.GetVal(processor.eventID, commandData.LeftSide);
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Assign)
                    {

                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Addition)
                    {
                        assignVal += left;
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Subtraction)
                    {
                        assignVal = left - assignVal;
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Multiplication)
                    {
                        assignVal *= left;
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Division)
                    {
                        assignVal = left / assignVal;
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Modulo)
                    {
                        assignVal = left % assignVal;
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.LowerBound)
                    {
                        assignVal = Math.Max(left, assignVal);
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.UpperBound)
                    {
                        assignVal = Math.Min(left, assignVal);
                    }
                    if (commandData.AssignmentOperator == NumberAssignmentOperator.Absolute)
                    {
                        assignVal = Math.Abs(assignVal);
                    }

                    valueCallHandler.SetVal(processor.eventID, commandData.LeftSide, assignVal);
                    return true;
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
