using System.Collections.Generic;

namespace TuringMachine
{
    public class Tape
    {
        #region ATTRIBUTES
         List<char> Contents = new List<char>();
         int Pointer;
        #endregion
        #region CONSTRUCTORS
        //Add blank tape
        public Tape()
        {
            Contents = new List<char>();
        }
        //Tape with known contents
        public Tape(string Tape, int Pointer)
        {
            Contents = new List<char>();
            foreach (char key in Tape)
            {
                Contents.Add(key);
            }
            this.Pointer = Pointer;
        }
        #endregion
        #region GET
        //Get whole tape
        public string GetTape()
        {
            string tape = "";
            foreach (char c in Contents)
            {
                tape += c;
            }
            return tape;
        }
        //Get character from tape
        public char GetKey()
        {
            return Contents[Pointer];
        }
        public int GetLength()
        {
            return Contents.Count;
        }
        public int GetPointer()
        {
            return Pointer;
        }
        #endregion
        #region SET
        //Add at beginning of tape
        public void AddFront(char Input)
        {
            if (Contents.Count == 1)
            {
                Contents.Add(Contents[0]);
            }
            else if(Contents.Count == 0)
            {
                Contents.Add(Input);
            }
            else
            {
                Contents.Add(Contents[Contents.Count - 1]);
            }
            for (int i = Contents.Count - 1; i > 0; i--)
            {
                Contents[i] = Contents[i - 1];
            }
            Contents[0] = Input;
        }
        //Add at pointer value of tape
        public void AddMiddle(char Input)
        {
            Contents[Pointer] = Input;
        }
        //Add to end of tape
        public void AddBack(char Input)
        {
            Contents.Add(Input);
        }
        //Remove unnecessary spaces from ends of tape
        public void TrimEdges()
        {
            if (Contents.Count != 0)
            {
                int IndexOfLeftBound = 0;
                int IndexOfRightBound = 0;

                for (int i = 0; i < Contents.Count; i++)
                {
                    if (Contents[i] != '_')
                    {
                        //Assign first item that is not blank
                        IndexOfLeftBound = i;
                        break;
                    }
                }

                for (int i = Contents.Count - 1; i >= 0; i--)
                {
                    if (Contents[i] != '_')
                    {
                        //Assign first item that is not blank
                        IndexOfRightBound = i;
                        break;
                    }
                }

                //If pointer is after left bound
                //Delete all blank spaces to left of tape
                if (Pointer >= IndexOfLeftBound)
                {
                    Contents.RemoveRange(0, IndexOfLeftBound);
                    Pointer -= IndexOfLeftBound;
                }
                //If pointer is before right bound
                //Delete all blank spaces to right of tape
                if (Pointer <= IndexOfRightBound && Contents.Count - IndexOfRightBound - 1 > 0)
                {
                    Contents.RemoveRange(IndexOfRightBound + 1, Contents.Count - IndexOfRightBound - 1);
                }
            }
        }
        //Move pointer by +/- 1
        public void ChangePointer(int Value)
        {
            Pointer += Value;
        }
        #endregion
    }
}
