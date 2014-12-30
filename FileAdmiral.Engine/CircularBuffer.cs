using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine
{
    public class CircularBuffer : IEnumerable<string>
    {
        private readonly Queue<string> _queue;
        private readonly int _capacity;
        public CircularBuffer(int capacity)
        {
            _queue = new Queue<string>(capacity);
            _capacity = capacity;
        }

        public void AddLines(IEnumerable<string> lines)
        {
            foreach(string line in lines)
            {
                AddLine(line,false);
            }
            Changed.Raise(this, EventArgs.Empty);
        }

        public void AddLine(string line)
        {
            AddLine(line, true);
        }

        private void AddLine(string line, bool notify)
        {
            if (_queue.Count == _capacity)
            {
                _queue.Dequeue();
            }
            _queue.Enqueue(line);
            if (notify)
            {
                Changed.Raise(this, EventArgs.Empty);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        public event EventHandler Changed;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Join("\n", this);
        }
    }
}
