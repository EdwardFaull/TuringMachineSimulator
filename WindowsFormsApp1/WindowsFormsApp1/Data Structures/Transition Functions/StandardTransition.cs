using System.Collections.Generic;

namespace TuringMachine
{
    public class StandardTransition : ReadOnlyTransition
    {
        protected char WriteKey;

        public StandardTransition()
        {
        }
        //Parse parameter to get attributes (validation already done)
        public StandardTransition(string Line)
        {
            string[] strings = Line.Split(' ');
            InitialState = strings[0];
            ReadKey = strings[1][0];
            WriteKey = strings[2][0];
            MoveKey = strings[3][0];
            FinalState = strings[4];
        }

        public char GetWriteKey()
        {
            return WriteKey;
        }
    }
}
