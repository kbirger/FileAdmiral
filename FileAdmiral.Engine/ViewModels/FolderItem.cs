using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.Engine.ViewModels
{
    public class FolderItem
    {
        public FolderItem(string displayName, string resourcePath, bool isLeaf)
        {
            DisplayName = displayName;
            ResourcePath = resourcePath;
            IsLeaf = isLeaf;
        }

        public string ResourcePath { get; private set; }

        public string DisplayName { get; private set; }

        public bool IsLeaf { get; private set; }

        // hack: this is temporary. this should be done with a formatter later
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            if (!IsLeaf) // directory
            {
                return "[" + DisplayName + "]";
            }
            else
            {
                return DisplayName;
            }
        }
    }
}
