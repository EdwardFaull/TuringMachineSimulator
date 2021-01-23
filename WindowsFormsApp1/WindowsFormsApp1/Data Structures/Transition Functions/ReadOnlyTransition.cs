namespace TuringMachine
{
    public class ReadOnlyTransition
    {
        #region ATTRIBUTES
         protected string InitialState;
         protected char ReadKey;
         protected char MoveKey;
         protected string FinalState;
        #endregion
        #region CONSTRUCTORS
        public ReadOnlyTransition()
        {
        }
        //Parse parameter to get attributes (validation already done)
        public ReadOnlyTransition(string Line)
        {
            string[] strings = Line.Split(' ');
            InitialState = strings[0];
            ReadKey = strings[1][0];
            MoveKey = strings[2][0];
            FinalState = strings[3];
        }
        #endregion
        #region GET
        //Return private attributes
        public string GetInitialState()
        {
            return InitialState;
        }
        public char GetReadKey()
        {
            return ReadKey;
        }
        public char GetMoveKey()
        {
            return MoveKey;
        }
        public string GetFinalState()
        {
            return FinalState;
        }
        #endregion
    }
}
