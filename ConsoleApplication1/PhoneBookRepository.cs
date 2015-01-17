namespace PhoneBookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Wintellect.PowerCollections;
    class PhoneBookRepository : IPhonebookRepository
    {
        private OrderedSet<PhoneBookEntry> entriesSorted = 
            new OrderedSet<PhoneBookEntry>();
        private Dictionary<string, PhoneBookEntry> entriesByName = 
            new Dictionary<string, PhoneBookEntry>();
        private MultiDictionary<string, PhoneBookEntry> entriesByPhone = 
            new MultiDictionary<string, PhoneBookEntry>(false);
        public bool AddPhone(string name, IEnumerable<string> phoneNumbers)
        {
            string personName = name.ToLowerInvariant();
            PhoneBookEntry entry;
            bool isNewEntry = !this.entriesByName.TryGetValue(personName, out entry);
            if (isNewEntry)
            {
                entry = new PhoneBookEntry(name);
                this.entriesByName.Add(personName, entry);
                this.entriesSorted.Add(entry);
            }
            foreach (var phoneNumber in phoneNumbers)
            {
                this.entriesByPhone.Add(phoneNumber, entry);
            }
            entry.Phones.UnionWith(phoneNumbers);
            return isNewEntry;
        }
        public int ChangePhone(string oldPhone, string newPhone)
        {
            var matchedEntries = this.entriesByPhone[oldPhone].ToList();
            foreach (var entry in matchedEntries)
            {
                entry.Phones.Remove(oldPhone);
                this.entriesByPhone.Remove(oldPhone, entry);
                entry.Phones.Add(newPhone);
                this.entriesByPhone.Add(newPhone, entry);
            }
            return matchedEntries.Count;
        }
        public PhoneBookEntry[] ListEntries(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex + count > this.entriesByName.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid strat index or count.");
            }
            else
            {
                PhoneBookEntry[] list = new PhoneBookEntry[count];
                for (int i = startIndex; i <= startIndex + count - 1; i++)
                {
                    PhoneBookEntry entry = this.entriesSorted[i];
                    list[i - startIndex] = entry;
                }
                return list;
            }
            
        }
    }
}
