using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame
{
    public struct BlockID
    {
        // DATA //
        // ID
        private List<int> _id;

        // Properties
        public List<int> id { get { return _id; } }
        public static readonly char STRING_SEP_CHAR = '-';


        // CONSTRUCTORS //
        public BlockID(params int[] values)
        {
            _id = new List<int>(values);
        }


        // FUNCTIONS //
        // Overrides
        public override string ToString()
        {
            // Iterates over each integer in the ID and adds a separating character
            string stringified = "";
            foreach(int value in id)
            {
                stringified += value.ToString();
                stringified += STRING_SEP_CHAR;
            }

            // Removes the last separator
            stringified = stringified.Remove(stringified.Length - 1);

            // Returns the generated string
            return stringified;
        }


        // Utility Functions
        public bool IsEqual(object obj)
        {
            // Ensures objects are the same type have the same length ID
            if(obj is BlockID && ((BlockID)obj).id.Count == id.Count)
            {
                // Ensures the values in the ID list are equal
                for(int i = 0; i < id.Count; i++)
                {
                    if(id[i] != ((BlockID)obj).id[i])
                    {
                        return false;
                    }
                }

                // If the values are confirmed to be equal (didn't return within the loop) considers the IDs to be equal
                return true;
            }

            // Returns false if the objects aren't the same type and don't ahve the same ID length
            return false;
        }
    }
}
