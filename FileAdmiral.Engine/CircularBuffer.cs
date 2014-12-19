using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine
{
    public class CircularBuffer : IEnumerable<string>
    {
        private Queue<string> _queue;
        private int _capacity;
        public CircularBuffer(int capacity)
        {
            _queue = new Queue<string>(capacity);
            _capacity = capacity;
        }

        public void AddLines(IEnumerable<string> lines)
        {
            foreach(string line in lines)
            {
                AddLine(line);
            }
        }

        public void AddLine(string line)
        {
            if(_queue.Count == _capacity)
            {
                _queue.Dequeue();
            }
            _queue.Enqueue(line);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }
    }
}
