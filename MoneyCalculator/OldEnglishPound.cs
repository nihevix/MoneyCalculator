using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MoneyCalculator
{
    internal class OldEnglishPound
    {
        private int pound;
        private int shillings;
        private int pennies;
        private int[] rest = { 0,0,0 };
        public override string ToString()
        {
                if (rest[0]>0)
                        return String.Format("{0}p {1}s {2}d ({3}p{4}s{5}d) ", pound, shillings, pennies, rest[0], rest[1], rest[2]);
                    else if (rest[1] > 0)
                        return String.Format("{0}p {1}s {2}d ({3}s {4}d) ", pound, shillings, pennies, rest[1], rest[2]);
                    else if (rest[2] > 0)
                        return String.Format("{0}p {1}s {2}d ({3}d) ", pound, shillings, pennies, rest[2]);
                    else return String.Format("{0}p {1}s {2}d ", pound, shillings, pennies);
        }

        /// <summary>
        /// Constructor of the class that set default parameter to 0
        /// </summary>
        /// <param name="pound"></param>
        /// <param name="shillings"></param>
        /// <param name="pennies"></param>
        public OldEnglishPound(int pound = 0, int shillings = 0, int pennies = 0)
        {
            this.pound = pound;
            this.shillings = shillings;
            this.pennies = pennies;

        }
        /// <summary>
        /// Constructor that initialize an OldEnglishPound with the parameter written in the input string
        /// </summary>
        /// <param name="command"></param>
        public OldEnglishPound(string command)
        {
            int poundId, shillingsId, penniesId;
            poundId = command.IndexOf("p");
            shillingsId = command.IndexOf("s");
            penniesId = command.IndexOf("d");

            this.pound = Convert.ToInt32(command.Substring(0, poundId));
            this.shillings = Convert.ToInt32(command.Substring(poundId + 1, shillingsId - poundId - 1));
            this.pennies = Convert.ToInt32(command.Substring(shillingsId + 1, penniesId - shillingsId - 1));
        }

        /// <summary>
        /// This function execute the written command and invokes the appropriate operation.
        /// it returns a String with the result of the operation.
        /// </summary>
        /// <returns></returns>
        public string Execute(string command)
        {
            Console.Write(command + " = ");
            //leaving whiteplaces
            command = Regex.Replace(command, @" ", "");
            int operatorIndex;
            //find operator
            try
            {
                int[] values, newArray;
                if (command.Contains('+'))
                {
                    operatorIndex = command.IndexOf("+");
                    char operation = command[operatorIndex];
                     values = GetValues(command, operatorIndex);
                     newArray = Add(values);
                    return String.Format("{0}p {1}s {2}d", newArray[0], newArray[1], newArray[2]);
                }
                else if (command.Contains('-'))
                {
                    operatorIndex = command.IndexOf("-");
                    char operation = command[operatorIndex];
                     values = GetValues(command, operatorIndex);
                     newArray = Sub(values);
                    if (newArray[0] < 0) return "Attention: you are trying to subtract more money than is available. ";
                    else return String.Format("{0}p {1}s {2}d", newArray[0], newArray[1], newArray[2]);
                }
                else if (command.Contains('*'))
                {
                    operatorIndex = command.IndexOf("*");
                    char operation = command[operatorIndex];
                     values = GetMoltiplicationValues(command, operatorIndex);
                     newArray = Mul(values);
                    return String.Format("{0}p {1}s {2}d", newArray[0], newArray[1], newArray[2]);
                }
                else if (command.Contains('/'))
                {
                    operatorIndex = command.IndexOf("/");
                     values = GetMoltiplicationValues(command, operatorIndex);
                     newArray= Div(values);
                    if (newArray[3] > 0)
                        return String.Format("{0}p {1}s {2}d ({3}p{4}s{5}d) ", newArray[0], newArray[1], newArray[2], newArray[3], newArray[4], newArray[5]);
                    else if (newArray[4] > 0)
                        return String.Format("{0}p {1}s {2}d ({3}s {4}d) ", newArray[0], newArray[1], newArray[2], newArray[4], newArray[5]);
                    else if (newArray[5] > 0)
                        return String.Format("{0}p {1}s {2}d ({3}d) ", newArray[0], newArray[1], newArray[2], newArray[5]);
                    else return String.Format("{0}p {1}s {2}d ", newArray[0], newArray[1], newArray[2]);
                }
                else
                {

                    return "sorry, Operator not finded in the input String. Try Again.";
                }
            }
            catch (Exception e)
            {
                return "Sorry, input was not in the correct format. Error Code: " + e.Message;
            }
        }
        /// <summary>
        /// this function add the input GetValues and return a string with the answer.
        /// <param name="Values"> 
        /// GetValues[0] = pound; GetValues[1] =Shillings; GetValues[2] = Pennies;
        /// GetValues[3] = pound; GetValues[4] =Shillings; GetValues[5] = Pennies;
        /// </param>
        /// <returns></returns>
        /// </summary>
        private int[] Add(int[] Values)
        {
            Values[0] = Values[0] + Values[3];
            Values[1] = Values[1] + Values[4];
            Values[2] = Values[2] + Values[5];
            int[] newArray = AdjustValues(Values.Take(3).ToArray<int>());
            return newArray;
        }
        /// <summary>
        /// this function subtract the input GetValues and return a string with the answer.
        /// <param name="Values"> 
        /// Values[0] = pound; Values[1] =Shillings; Values[2] = Pennies;
        /// Values[3] = pound; Values[4] =Shillings; Values[5] = Pennies;
        /// </summary>
        /// <param name="Values"></param>
        /// <returns></returns>
        private int[] Sub(int[] Values)
        {
            Values[0] = Values[0] - Values[3];
            Values[1] = Values[1] - Values[4];
            Values[2] = Values[2] - Values[5];
            int[] newArray = AdjustValues(Values.Take(3).ToArray<int>());
            return newArray;
        }
        /// <summary>
        /// this function multiply the input GetValues and return a string with the answer.
        /// Values[0] = pound; Values[1] =Shillings; Values[2] = Pennies; Values[3] = Multiplier
        /// </summary>
        /// <param name="Values"></param>
        /// <returns></returns>
        private int[] Mul(int[] Values)
        {
            int penny = ConvertToPenny(Values);
            penny = penny * Values[3];
            int[] newArray = AdjustValues(penny);
            return newArray;
        }
        /// <summary>
        /// this function divide the input GetValues and return a string with the answer.
        /// Values[0] = pound; Values[1] =Shillings; Values[2] = Pennies; Values[3] = Divided
        /// </summary>
        /// <param name="Values"></param>
        /// <returns></returns>
        private int[] Div(int[] Values)
        {
            int pennies = ConvertToPenny(Values);
            int restPennies = pennies % Values[3];
            pennies = pennies / Values[3];
            int[] newArray = AdjustValues(pennies);
            int[] ArrayRest = AdjustValues(restPennies);
            this.rest = ArrayRest;
            int[] fullArray = newArray.Concat(ArrayRest).ToArray();
            return fullArray;
        }
        /// <summary>
        /// this function return the conversation into pennies of the input values;
        /// </summary>
        /// <param name="Values"></param>
        /// <returns></returns>
        private int ConvertToPenny(int[] Values)
        {
            while (Values[0] > 0)
            {
                Values[0]--;
                Values[1] += 20;
            }
            while (Values[1] > 0)
            {
                Values[1]--;
                Values[2] += 12;
            }
            return Values[2];
        }
        /// <summary>
        /// This function translate the Values pounds, shillings and pennies using the conversion
        /// 12 pennies for 1 shillings, 20 shillings for 1 pounds. 
        /// </summary>
        /// <param name="Values">
        /// Values[0] = pound; Values[1] =Shillings; Values[2] = Pennies; 
        /// </param>
        /// <returns></returns>
        public int[] AdjustValues(int[] Values)
        {
            while (Values[2] <= 0)
            {
                Values[1]--;
                Values[2] += 12;

            }
            while (Values[1] <= 0)
            {
                Values[0]--;
                Values[1] += 20;

            }
            while (Values[2] >= 12)
            {
                Values[2] -= 12;
                Values[1]++;
            }
            while (Values[1] >= 20)
            {
                Values[1] -= 20;
                Values[0]++;
            }
            return Values;
        }
        /// <summary>
        /// This function translate the Pennies gived to Pounds, shillings and pennies using the conversion
        /// 12 pennies for 1 shillings, 20 shillings for 1 pounds. 
        /// </summary>
        /// <param name="Values">
        /// Values[0] = pound; Values[1] =Shillings; Values[2] = Pennies; 
        /// </param>
        /// <returns></returns>
        public int[] AdjustValues(int pennies)
        {
            int pound = 0;
            int shillings = 0;
            while (pennies >= 12)
            {
                pennies -= 12;
                shillings++;
            }
            while (shillings >= 20)
            {
                shillings -= 20;
                pound++;
            }
            return new int[] { pound, shillings, pennies };
        }
        /// <summary>
        /// This function return an int Array with the initial parameters of pound,shillings and pennies
        /// followed by the parameters yet to be accounted for
        /// </summary>
        /// <param name="command"></param>
        /// <param name="operatorIndex"></param>
        /// <returns></returns>
        private int[] GetValues(string command, int operatorIndex)
        {
      
            int[] array1 = GetValuesFromString(command.Substring(0, operatorIndex));
            int[] array2 = GetValuesFromString(command.Substring(operatorIndex + 1, command.Length - operatorIndex - 1));
            int[] values = { array1[0], array1[1], array1[2], array2[0], array2[1], array2[2] };
            return values;
        }
        /// <summary>
        /// This function return an int Array with the initial parameters of pound, shillings and pennies
        /// followed by the multiplier
        /// </summary>
        /// <param name="command"></param>
        /// <param name="operatorIndex"></param>
        /// <returns></returns>
        private int[] GetMoltiplicationValues(string command, int operatorIndex)
        {

            int multiplier = Convert.ToInt32(command.Substring(operatorIndex + 1, command.Length - operatorIndex - 1));
            int[] array = GetValuesFromString(command.Substring(0, operatorIndex));
            int[] values = { array[0], array[1], array[2], multiplier };
            return values;
        }
        private int[] GetValuesFromString(string command)
        {
            int poundId, shillingsId, penniesId;
            poundId = command.IndexOf("p");
            shillingsId = command.IndexOf("s");
            penniesId = command.IndexOf("d");
            int pound = Convert.ToInt32(command.Substring(0, poundId));
            int shillings = Convert.ToInt32(command.Substring(poundId + 1, shillingsId - poundId - 1));
            int pennies = Convert.ToInt32(command.Substring(shillingsId + 1, penniesId - shillingsId - 1));
            return new int[] { pound, shillings, pennies };
        }
        public OldEnglishPound Sum(OldEnglishPound wallet)
        {
            int[] array = { this.pound,this.shillings,this.pennies, wallet.pound,wallet.shillings,wallet.pennies };
            int[] newArray = Add(array);
            this.pound = newArray[0];
            this.shillings = newArray[1];
            this.pennies = newArray[2];
            return this;

        }
        public OldEnglishPound Sub(OldEnglishPound wallet)
        {
            int[] array = { this.pound, this.shillings, this.pennies, wallet.pound, wallet.shillings, wallet.pennies };
            int[] newArray = Sub(array);
            this.pound = newArray[0];
            this.shillings = newArray[1];
            this.pennies = newArray[2];
            return this;
        }
        public OldEnglishPound Multiply(int multipler)
        {
            int[] array = { this.pound, this.shillings, this.pennies,multipler };
            int[] newArray = Mul(array);
            this.pound = newArray[0];
            this.shillings = newArray[1];
            this.pennies = newArray[2];
            return this;
        }
        public OldEnglishPound Div(int dividend)
        {
            int[] array = { this.pound, this.shillings, this.pennies, dividend };
            int[] newArray = Div(array);
            this.pound = newArray[0];
            this.shillings = newArray[1];
            this.pennies = newArray[2];
            return this;
        }
    }
}
