using Calculator.Business.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Business.Services
{
    public class CalculatorService
    {
        public double result { get; private set; }

        public void PressDigit(CalculatorState state, string digitString)
        {
            if (digitString == ".")
            {
                if (!state.HasDecimalPoint)
                {
                    state.HasDecimalPoint = true;
                }
                return;
            }

            int digit = int.Parse(digitString);

            if (state.IsClearNeeded)
            {
                state.RegisterX = 0;
                state.HasDecimalPoint = false;
                state.DecimalPlaces = 0;
                state.IsClearNeeded = false;
            }

            if (state.HasDecimalPoint)
            {
                state.DecimalPlaces++;
                state.RegisterX += digit / Math.Pow(10, state.DecimalPlaces);
            }
            else
            {
                state.RegisterX = state.RegisterX * 10 + digit;
            }
        }

        public void Clear(CalculatorState state)
        {
            state.RegisterX = 0;
        }

        public void MoveXToY(CalculatorState state)
        {
            state.RegisterY = state.RegisterX;
        }

        public void CompleteOperation(CalculatorState state, string operationCode)
        {
            switch (operationCode)
            {
                case "+":
                    result = state.RegisterX = state.RegisterY + state.RegisterX;
                    break;
                case "-":
                    result = state.RegisterX = state.RegisterY - state.RegisterX;
                    break;
                case "*":
                    result = state.RegisterX = state.RegisterY * state.RegisterX;
                    break;
                case "/":
                    result = state.RegisterX = state.RegisterY / state.RegisterX;
                    break;
                case "X2":
                    result = state.RegisterX *= state.RegisterX;
                    break;
                case "R":
                    result = state.RegisterX = Math.Sqrt(state.RegisterX);
                    break;
                case "x^y":
                    result = state.RegisterX = Math.Pow(state.RegisterY, state.RegisterX);
                    break;
            }

            state.History = $"{state.RegisterY} {operationCode} {state.RegisterX}";
            state.IsClearNeeded = true;
        }

        public void PressOperation(CalculatorState state, string operationCode)
        {
            if (!string.IsNullOrEmpty(state.OperationCode))
            {
                CompleteOperation(state, state.OperationCode);
            }

            MoveXToY(state);
            state.OperationCode = operationCode;
            state.IsClearNeeded = true;
        }

        public void PressEqual(CalculatorState state)
        {
            if (!string.IsNullOrEmpty(state.OperationCode))
            {
                CompleteOperation(state, state.OperationCode);
                state.OperationCode = null;
            }
            state.IsClearNeeded = true;
        }
    }
}