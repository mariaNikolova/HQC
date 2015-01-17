using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSystem
{
    class PhoneBookEntry : IComparable<PhoneBookEntry>
    {
        public string Name { get; private set; }
        public SortedSet<string> Phones { get; private set; }

        public PhoneBookEntry(string name)
        {
            this.Name = name;
            this.Phones = new SortedSet<string>();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append('[');
            sb.Append(this.Name);
            bool flag = true;
            foreach (var phone in this.Phones)
            {
                if (flag)
                {
                    sb.Append(": ");
                    flag = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(phone);
            }
            sb.Append(']');
            return sb.ToString();
        }
        public int CompareTo(PhoneBookEntry other)
        {
            return string.Compare(this.Name, other.Name, true);
        }
    }
}
