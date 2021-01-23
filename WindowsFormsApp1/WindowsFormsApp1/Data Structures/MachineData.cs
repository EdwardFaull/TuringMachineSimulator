using System.Collections.Generic;

namespace TuringMachine
{
    public class MachineData
    {
        List<int> Pointer;
        List<int> InitialPointer;
        List<string> Tape;
        List<string> InitialState;
        int Machine;
        int Tapes;

        //Create blank object
        public MachineData() { }

        //Make out of individual parameters
        public MachineData(int machine, int tapes, List<string> tape, List<int> pointer, List<string> initialState)
        {
            Pointer = new List<int>(pointer);
            InitialPointer = new List<int>(pointer);
            Tape = tape;
            InitialState = initialState;
            Machine = machine;
            Tapes = tapes;
        }

        //Create copy of existing object
        public MachineData(MachineData machineData)
        {
            Pointer = new List<int>(machineData.GetPointer());
            InitialPointer = new List<int>(machineData.GetPointer());
            Tape = new List<string>(machineData.GetTape());
            InitialState = new List<string>(machineData.GetInitialState());
            Machine = machineData.GetMachine();
            Tapes = machineData.GetTapes();
        }

        #region GET

        public int GetPointer(int index)
        {
            return Pointer[index];
        }

        public List<int> GetPointer()
        {
            return Pointer;
        }

        public string GetTape(int index)
        {
            return Tape[index];
        }

        public List<string> GetTape()
        {
            return Tape;
        }

        public string GetInitialState(int index)
        {
            if(Machine != 4)
            {
                return InitialState[0];
            }
            return InitialState[index];
        }

        public List<string> GetInitialState()
        {
            return InitialState;
        }

        public int GetMachine()
        {
            return Machine;
        }

        public int GetTapes()
        {
            return Tapes;
        }

        #endregion

        #region SET

        public void SetPointer(int value, int index)
        {
            Pointer[index] = value;
        }

        public void SetTape(string value, int index)
        {
            Tape[index] = value;
        }

        public void SetInitialState(string value, int index)
        {
            InitialState[index] = value;
        }

        public void SetMachine(int value)
        {
            Machine = value;
        }

        public void ResetPointers()
        {
            Pointer = new List<int>(InitialPointer);
        }

        public void SetPointers()
        {
            InitialPointer = new List<int>(Pointer);
        }

        //Add new default fields to each list
        public void AddTape()
        {
            Pointer.Add(0);
            InitialPointer.Add(0);
            Tape.Add("");
            InitialState.Add("");
            Tapes++;
        }

        public void SetTapes(int value)
        {
            Tapes = value;
        }

        //Remove information from each list at specified index
        public void CloseTape(int index)
        {
            Pointer.RemoveAt(index);
            InitialPointer.RemoveAt(index);
            Tape.RemoveAt(index);
            InitialState.RemoveAt(index);
            Tapes--;
        }
        #endregion
    }
}
