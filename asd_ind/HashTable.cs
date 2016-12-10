using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asd_ind
{
    class Record
    {
        public string key;
        public string value;
        public Record(string key = null, string value = null)
        {
            this.key = key;
            this.value = value;
        }
    }

    class PrimeNumbers
    {
        private static bool IsPrime(int x)
        {
            if (x < 1) return false;
            for (int i = 2; i * i <= x; i++)
                if ((x % i) == 0) return false;
            return true;
        }
        public static int GetMaxPrime(int n)
        {
            while (!IsPrime(n))
                n--;
            return n;
        }
    }
    
    class HashTable
    {
        int capacity, m, count = 0;
        List<Record>[] vector;

        public delegate void addToStr(string str);

        public int hash(string key)
        {
            int hash = 0;
            for (int i = 0; i < key.Length; i += 2)
                hash += (int)(key[i] - '0');
            hash = (hash * key.Length) % m;
            return hash;
        }

        public HashTable(int capacity = 3)
        {
            this.capacity = capacity;
            m = PrimeNumbers.GetMaxPrime(capacity);
            vector = new List<Record>[capacity];
            for (int i = 0; i < vector.Length; i++)
                vector[i] = new List<Record>();
        }



        private void rebuildTable(int newLength)
        {
            List<Record>[] newTable = new List<Record>[newLength];
            m = PrimeNumbers.GetMaxPrime(newLength);

            for (int i = 0; i < newTable.Length; i++)
                newTable[i] = new List<Record>();

            foreach (List<Record> chain in vector)
            {
                foreach(Record r in chain)
                {
                    newTable[hash(r.key)].Add(r);
                }
            }

            vector = newTable;
        }

        public void insert(Record r)
        {
            if ((double)count / (double)vector.Length >= 0.8)
            {
                rebuildTable(vector.Length * 2);
            }
            List<Record> chain = vector[hash(r.key)];

            foreach(Record rec in chain)
            {
                if (rec.key == r.key)
                    throw new ArgumentException();
            }

            chain.Add(r);
            ++count;
                
        }
        public void insertCallback(Record r, addToStr callback)
        {
            insert(r);
            int hashCode = hash(r.key);
            List<Record> chain = vector[hashCode];
            foreach (Record t in chain)
                callback(t.key.ToString());            
        }

        public Record find(string key)
        {
            List<Record> chain = vector[hash(key)];
            if (!chain.Any())
                throw new IndexOutOfRangeException();
            foreach (Record t in chain)
            {
                if (t.key == key)
                    return t;
            }
            throw new IndexOutOfRangeException();
            return null;
        }
        public Record findCallback(string key, addToStr callback)
        {
            List<Record> chain = vector[hash(key)];
            if (!chain.Any())
                throw new IndexOutOfRangeException();
            Record r = null;
            foreach (Record t in chain)
            {
                callback(t.key);
                if (t.key == key)
                {
                    r = t;
                }
            }
            if (r == null)
                throw new IndexOutOfRangeException();
            return r;
        }
        public void remove(string key)
        {
            List<Record> chain = vector[hash(key)];
            if (!chain.Any())
                throw new IndexOutOfRangeException();
            foreach (Record t in chain)
            {
                if (t.key == key)
                {
                    chain.Remove(t);
                    --count;
                    return;
                }
            }
            throw new IndexOutOfRangeException();
        }

        public void clearTable()
        {
            for (int i = 0; i < capacity; i++)
                vector[i].Clear();
            Array.Resize(ref vector, capacity);
            count = 0;
            m = PrimeNumbers.GetMaxPrime(capacity);
        }
    }
}
