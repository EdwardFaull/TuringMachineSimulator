using System.Collections.Generic;
using System.Linq;

namespace TuringMachine
{
    public class MultiTapeTransition : StandardTransition
    {
        #region ATTRIBUTES
        new List<char> ReadKey;
        new List<char> WriteKey;
        new List<char> MoveKey;
        #endregion
        #region CONSTRUCTORS
        public MultiTapeTransition()
        {
        }
        //Parse parameter to get attributes (validation already done)
        public MultiTapeTransition(string Line, int Tapes)
        {
            string[] strings = Line.Split(' ');
            InitialState = strings[0];
            ReadKey = strings[1].ToCharArray().ToList();
            WriteKey = strings[2].ToCharArray().ToList();
            MoveKey = strings[3].ToCharArray().ToList();
            FinalState = strings[strings.Length - 1];
        }
        #endregion
        #region GET
        public char GetReadKey(int Tape)
        {
            return ReadKey[Tape];
        }
        public List<char> GetReadKeys()
        {
            return ReadKey;
        }
        public char GetWriteKey(int Tape)
        {
            return WriteKey[Tape];
        }
        public List<char> GetWriteKeys()
        {
            return WriteKey;
        }
        public char GetMoveKey(int Tape)
        {
            return MoveKey[Tape];
        }
        public List<char> GetMoveKeys()
        {
            return MoveKey;
        }
        #endregion
    }
}