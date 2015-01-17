using System;
namespace PhoneBookSystem
{
    interface IPhonebookRepository
    {
        bool AddPhone(string name, System.Collections.Generic.IEnumerable<string> phoneNumbers);
        int ChangePhone(string oldPhoneNumber, string newPhoneNumber);
        PhoneBookEntry[] ListEntries(int first, int num);
    }
}
