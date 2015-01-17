namespace PhoneBookSystem
{

using System;
using System.Data;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Collections.Generic;


using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Mime;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;
        class PhoneBook
        {
            private const string code = "+359";
            public const char plusSymbol = '+';
            private static IPhonebookRepository phoneBook = new PhoneBookRepository(); 
            private static StringBuilder output = new StringBuilder();
            static void Main()
            {
                ProcessAllCommands();
                PrintCollectedOutput();
            }
            private static void ProcessAllCommands()
            {
                while (true)
                {
                    string commandText = Console.ReadLine();
                    if (commandText == "End" || commandText == null)
                    {
                        break;
                    }
                    ExecuteCommand(commandText);
                }
            }
            private static void ExecuteCommand(string commandText)
            {
                int colonIndex = commandText.IndexOf('(');
                if (colonIndex == -1)
                {
                    throw new ArgumentException("Invalid command format: " + commandText);
                }
                string command = commandText.Substring(0, colonIndex);
                if (!commandText.EndsWith(")"))
                {
                    throw new ArgumentException("Invalid command format: " + commandText);
                }
                string argumentsStr = commandText.Substring(colonIndex + 1, commandText.Length - colonIndex - 2);
                string[] arguments = argumentsStr.Split(',');

                for (int j = 0; j < arguments.Length; j++)
                {
                    arguments[j] = arguments[j].Trim();
                }
                ExecuteCommand(command, arguments);
            }
            private static void ExecuteCommand(string command, string[] arguments)
            {
                if ((command.StartsWith("AddPhone")) && (arguments.Length >= 2))
                {
                    ProcessAddPhoneCommand(arguments);
                }
                else if ((command == "ChangePhone") && (arguments.Length == 2))
                {
                    ProcessChangePhoneCommand(arguments);
                }
                else if ((command == "List") && (arguments.Length == 2))
                {
                    ProcessListCommand(arguments);
                }
                else
                {
                    throw new ArgumentException("Invalid command: " + command);
                }
            }
            private static void ProcessAddPhoneCommand(string[] arguments)
            {
                string name = arguments[0];
                var phones = arguments.Skip(1).ToList();
                for (int i = 0; i < phones.Count; i++)
                {
                    phones[i] = convertToCanonicalForm(phones[i]);
                }
                bool isNewEntry = phoneBook.AddPhone(name, phones);
                if (isNewEntry)
                {
                    Print("Phone entry created");
                }
                else
                {
                    Print("Phone entry merged");
                }
            }
            private static void ProcessChangePhoneCommand(string[] arguments)
            {
                string oldPhoneNumber = convertToCanonicalForm(arguments[0]);
                string newPhoneNumber = convertToCanonicalForm(arguments[1]);
                int updatedCount = phoneBook.ChangePhone(oldPhoneNumber, newPhoneNumber);
                Print(updatedCount + " numbers changed");
            }

            private static void ProcessListCommand(string[] arguments)
            {
                int startIndex = int.Parse(arguments[0]);
                int count = int.Parse(arguments[1]);

                try
                {
                    IEnumerable<PhoneBookEntry> entries = phoneBook.ListEntries(startIndex, count);
                    foreach (var entry in entries)
                    {
                        Print(entry.ToString());
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    Print("Invalid range");
                }
            }

			
            public static string removeInvalidCharAndLeadingZeros(string phoneNumber)
            {
                StringBuilder result = new StringBuilder();
                foreach (var symbol in phoneNumber)
                {
                    if (char.IsDigit(symbol) || symbol == plusSymbol)
                    {
                        result.Append(symbol);
                    }
                }
                if (result.Length >= 2 && result[0] == '0' && result[1] == '0')
                {
                    result.Remove(0, 1);
                    result[0] = plusSymbol;
                }
                while (result.Length > 0 && result[0] == '0')
                {
                    result.Remove(0, 1);
                }
                if (result.Length > 0 && result[0] != '+')
                {
                    result.Insert(0, code);
                }
                return result.ToString();
            }
            public static string  convertToCanonicalForm(string phoneNumber)
            {
                StringBuilder result = new StringBuilder (removeInvalidCharAndLeadingZeros(phoneNumber));
                if (result[0] != plusSymbol)
                {
                    result.Insert(0, code);
                }
                return result.ToString();

            }
            
            private static void Print(string text) {
                output.AppendLine(text);
            }
            private static void PrintCollectedOutput()
            {
                Console.Write(output);
            }
        }
        
}
