using System;
using System.Collections.Generic;

namespace TuringMachine
{
    public class MultiTape
    {
        #region ATTRIBUTES
        List<List<char>> Contents;
        readonly List<int> Pointers;
        #endregion
        #region CONSTRUCTORS
        //Add blank tape
        public MultiTape()
        {
            Contents = new List<List<char>>();
        }
        //Add tape with known values
        public MultiTape(List<string> Tape, List<int> Pointers)
        {
            if (Tape.Count != Pointers.Count)
            {
                return;
            }
            Contents = new List<List<char>>();
            for (int i = 0; i < Tape.Count; i++)
            {
                Contents.Add(new List<char>());
                foreach (char key in Tape[i])
                {
                    Contents[i].Add(key);
                }
            }
            this.Pointers = new List<int>();
            for (int i = 0; i < Tape.Count; i++)
            {
                this.Pointers.Add(Pointers[i]);
            }
        }
        #endregion
        #region GET
        //Get whole tape
        public string GetTape(int Tape)
        {
            string tape = "";
            foreach (char c in Contents[Tape])
            {
                tape += c;
            }
            return tape;
        }
        //Get character from tape
        public char GetKey(int Tape)
        {
            try
            {
                return Contents[Tape][Pointers[Tape]];
            }
            catch(Exception)
            {
                return '_';
            }
        }
        public int GetTapes()
        {
            return Contents.Count;
        }
        public int GetLength(int Tape)
        {
            return Contents[Tape].Count;
        }
        public int GetPointer(int Tape)
        {
            return Pointers[Tape];
        }
        public List<int> GetPointers()
        {
            return Pointers;
        }
        #endregion
        #region SET
        //Add to beginning of specified tape
        public void AddFront(char Input, int Tape)
        {
            if (Contents[Tape].Count == 1)
            {
                Contents[Tape].Add(Contents[Tape][0]);
            }
            else if(Contents[Tape].Count == 0)
            {
                Contents[Tape].Add(Input);
            }
            else
            { 
                Contents[Tape].Add(Contents[Tape][Contents[Tape].Count - 1]);
            }
            for (int i = Contents[Tape].Count - 1; i > 0; i--)
            {
                Contents[Tape][i] = Contents[Tape][i - 1];
            }
            Contents[Tape][0] = Input;
        }
        //Add at current position on tape
        public void AddMiddle(char Input, int Tape)
        {
            Contents[Tape][Pointers[Tape]] = Input;
        }
        //Add to end of specified tape
        public void AddBack(char Input, int Tape)
        {
            Contents[Tape].Add(Input);
        }
        //Removes unnecessary spaces from tape
        public void TrimEdges(int Tape)
        {
            if (Contents[Tape].Count != 0)
            {
                int IndexOfLeftBound = 0;
                int IndexOfRightBound = 0;

                for (int i = 0; i < Contents[Tape].Count; i++)
                {
                    if (Contents[Tape][i] != '_')
                    {
                        //Assign first item that is not blank
                        IndexOfLeftBound = i;
                        break;
                    }
                }

                for (int i = Contents[Tape].Count - 1; i >= 0; i--)
                {
                    if (Contents[Tape][i] != '_')
                    {
                        //Assign first item that is not blank
                        IndexOfRightBound = i;
                        break;
                    }
                }

                //If pointer is after left bound
                //Delete all blank spaces to left of tape
                if (Pointers[Tape] >= IndexOfLeftBound)
                {
                    Contents[Tape].RemoveRange(0, IndexOfLeftBound);
                    Pointers[Tape] -= IndexOfLeftBound;
                }
                //If pointer is before right bound
                //Delete all blank spaces to right of tape
                if (Pointers[Tape] <= IndexOfRightBound && Contents[Tape].Count - IndexOfRightBound - 1 > 0)
                {
                    Contents[Tape].RemoveRange(IndexOfRightBound + 1, Contents[Tape].Count - IndexOfRightBound - 1);
                }
            }
        }
        //Changes pointer by +/- 1
        public void ChangePointer(int Value, int Tape)
        {
            Pointers[Tape] += Value;
        }
        #endregion
    }
}