using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodingAdvent
{
    public class Permutations
    {
        public int[] m_perms;
        public int m_options = 0;
        public int m_size = 0;

        public Permutations(int options, int size)
        {
            Debug.Assert(options > 1);
            m_options = options;
            m_size = size;
            m_perms = new int[size];
        }

        public long NumberOfSolutions 
        { 
            get => (long) Math.Pow(m_options, m_size);
        }

        private void InternalIncrease(int index)
        {
            m_perms[index]++;
            if (m_perms[index] >= m_options)
            {
                m_perms[index] = 0;
                if (index < m_size - 1)
                    InternalIncrease(index + 1);
            }
        }

        public void Tick()
        {
            InternalIncrease(0);
        }

        public int this[int index]
        {
            get => m_perms[index];
        }

        public int Get(int index)
        {
            return m_perms[index];
        }

    }
}
