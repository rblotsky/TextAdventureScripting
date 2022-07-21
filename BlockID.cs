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
        public int idLength { get { return _id.Count; } }

        // Constants
        public static readonly char STRING_SEP_CHAR = '-';
        public static readonly int MIN_ID_LEN = 1;
        public static readonly int MIN_INDEX_VAL = 0;
        public static readonly BlockID ZERO = new BlockID(0);


        // CONSTRUCTORS //
        public BlockID(params int[] values)
        {
            _id = new List<int>(values);
        }

        public BlockID(List<int> values)
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
        public BlockID AddToLastIndex(int amount)
        {
            // Caches the stringified version of the ID for error message
            string originalID = this.ToString();

            // Iterates over the amount to add, modifying
            int absoluteAmount = Math.Abs(amount);
            int multiplier = amount / absoluteAmount;
            for(int i = 0; i < absoluteAmount; i++)
            {
                this.id[this.idLength-1] += multiplier;

                // If the last index is now too low, removes it and goes to a lower index
                if(this.id[this.idLength-1] < MIN_INDEX_VAL)
                {
                    this.RemoveLastIndex();

                    // If the ID length is now too low, throws an error
                    if(this.idLength <= MIN_ID_LEN)
                    {
                        throw new Exception(string.Format("Could not add {0} to BlockID \"{1}\" because it would modify the first index!", amount, originalID));
                    }
                }
            }

            // Returns the newly modified ID
            return this;
        }

        public BlockID AddIndex()
        {
            // Returns a this ID struct with a new index added to the end of the ID
            id.Add(MIN_INDEX_VAL);
            return this;
        }

        public BlockID RemoveLastIndex()
        {
            // Removes last index unless its at the min ID length
            if(idLength != MIN_ID_LEN)
            {
                id.Remove(idLength - 1);
            }
            
            // Returns this ID, modified earlier.
            return this;
        }


        // Overrides
        public override bool Equals(object obj)
        {
            // Ensures objects are the same type have the same length ID
            if (obj is BlockID && ((BlockID)obj).id.Count == id.Count)
            {
                // Ensures the values in the ID list are equal
                for (int i = 0; i < id.Count; i++)
                {
                    if (id[i] != ((BlockID)obj).id[i])
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

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        // Operator Overrides
        public static bool operator ==(BlockID left, BlockID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockID left, BlockID right)
        {
            return !(left == right);
        }
    }
}
