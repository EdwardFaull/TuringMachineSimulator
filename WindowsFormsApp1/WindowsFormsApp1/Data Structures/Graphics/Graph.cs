using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TuringMachine
{
    public class Graph
    {
        public Dictionary<string, Dictionary<string, string>> graph { get; } = 
           new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, List<string>> neighbours { get; } = new Dictionary<string, List<string>>();

        public List<string> statesHalting { get; } = new List<string>();

        bool isReadOnly = false;

        public Graph(string text)
        {
            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> stateList = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] != '/' && lines[i][1] != '/')
                {
                    string[] fields = lines[i].Split(' ');

                    if(i == 0)
                    {
                        if(fields.Length == 5)
                        {
                            isReadOnly = false;
                        }
                        else
                        {
                            isReadOnly = true;
                        }
                    }

                    //Add new pair in graph dictionary
                    if (!graph.ContainsKey(fields[0]))
                    {
                        graph.Add(fields[0], new Dictionary<string, string>());
                    }
                    //Add state to state list
                    if (!stateList.Contains(fields[0]))
                    {
                        stateList.Add(fields[0]);
                    }
                    //Add new pair in neighbour dictionary
                    if (!neighbours.ContainsKey(fields[0]))
                    {
                        neighbours.Add(fields[0], new List<string>());
                    }
                    if (neighbours.ContainsKey(fields[0]))
                    {
                        if (!neighbours[fields[0]].Contains(fields[fields.Length - 1]))
                        {
                            neighbours[fields[0]].Add(fields[fields.Length - 1]);
                        }
                    }

                    //Create information for state transition
                    string state = fields[0];
                    string hstate = fields[fields.Length - 1];
                    string terms = "";

                    //For read-only machines
                    if (fields.Length == 4)
                    {
                        terms += fields[1] + " | ";
                        if (fields[2] == "r")
                        {
                            terms += ">";
                        }
                        else if (fields[2] == "l")
                        {
                            terms += "<";
                        }
                        else
                        {
                            terms += "-";
                        }
                    }
                    //For all other machines
                    else
                    {
                        terms += fields[1] + " | ";
                        terms += fields[2] + " ";
                        foreach (char c in fields[3])
                        {
                            if (c == 'r')
                            {
                                terms += ">";
                            }
                            else if (c == 'l')
                            {
                                terms += "<";
                            }
                            else
                            {
                                terms += "-";
                            }
                        }
                    }

                    try
                    {
                        graph[state].Add(fields[fields.Length - 1], terms);
                    }
                    //If two transitions from the same state lead to the same state
                    catch (Exception)
                    {
                        string newLine;
                        if (fields.Length == 5)
                        {
                            newLine = ReassembleLine(graph[state][fields[fields.Length - 1]], fields[1], fields[2], fields[3], false);
                        }
                        else
                        {
                            newLine = ReassembleLine(graph[state][fields[fields.Length - 1]], fields[1], fields[2], fields[2], true);
                        }
                        graph[state][fields[fields.Length - 1]] = newLine;
                    }
                }
            }

            //Find what states are halting
            List<string> haltingStateList = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] != '/' && lines[i][1] != '/')
                {
                    string[] fields = lines[i].Split(' ');
                    if (!stateList.Contains(fields[fields.Length - 1]))
                    {
                        haltingStateList.Add(fields[fields.Length - 1]);
                    }
                }
            }

            statesHalting = haltingStateList;

            Dictionary<string, Dictionary<string, string>> newGraph = new Dictionary<string, Dictionary<string, string>>();
            foreach (KeyValuePair<string, Dictionary<string, string>> connection in graph)
            {
                Dictionary<string, string> newConnection = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> edge in connection.Value)
                {
                    bool isSimplified = false;
                    //Check if all values the same
                    int i = 0;
                    string previous = "";

                    List<string> ReadKeys = GetReadKeys(edge.Value, ref i);
                    i++;
                    List<string> WriteKeys = new List<string>();
                    if (!isReadOnly)
                    {
                        WriteKeys = GetWriteKeys(edge.Value, ref i);
                        previous = WriteKeys[0];
                        bool simplifyWriteKeys = false;
                        for (int j = 1; j < WriteKeys.Count; j++)
                        {
                            if (WriteKeys[j] != previous)
                            {
                                simplifyWriteKeys = false;
                                break;
                            }
                            else
                            {
                                simplifyWriteKeys = true;
                            }
                        }
                        if (simplifyWriteKeys)
                        {
                            string writeKey = WriteKeys[0];
                            WriteKeys = new List<string>();
                            WriteKeys.Add(writeKey);
                            isSimplified = true;
                        }
                    }
                    List<string> MoveKeys = GetMoveKeys(edge.Value, ref i);

                    previous = MoveKeys[0];
                    bool simplifyMoveKeys = false;
                    for (i = 1; i < MoveKeys.Count; i++)
                    {
                        if (MoveKeys[i] != previous)
                        {
                            simplifyMoveKeys = false;
                            break;
                        }
                        else
                        {
                            simplifyMoveKeys = true;
                        }
                    }

                    if (simplifyMoveKeys)
                    {
                        string moveKey = MoveKeys[0];
                        MoveKeys = new List<string>();
                        MoveKeys.Add(moveKey);
                    }

                    //Check if all values stay the same
                    if (!isSimplified && !isReadOnly)
                    {
                        bool isMatch = false;
                        for (i = 0; i < ReadKeys.Count; i++)
                        {
                            if (ReadKeys[i] == WriteKeys[i])
                            {
                                isMatch = true;
                            }
                            else if (Regex.IsMatch(WriteKeys[i], @"(\*)+"))
                            {
                                isMatch = true;
                            }
                            else
                            {
                                isMatch = false;
                            }
                        }
                        if (isMatch)
                        {
                            WriteKeys = new List<string>();
                        }
                    }

                    newConnection.Add(edge.Key, AssembleLine(ReadKeys, WriteKeys, MoveKeys, isReadOnly));
                }
                newGraph.Add(connection.Key, newConnection);
            }
            graph = newGraph;
        }

        //Recreate line if multiple transitions go to the same state from the same state
        string ReassembleLine(string existingLine, string readKey, string writeKey, string moveKey, bool isReadOnly)
        {
            int i = 0;
            List<string> ReadKeys = GetReadKeys(existingLine, ref i);
            i++;
            List<string> WriteKeys = new List<string>();
            if (!isReadOnly)
            {
                WriteKeys = GetWriteKeys(existingLine, ref i);
            }
            List<string> MoveKeys = GetMoveKeys(existingLine, ref i);

            //Add new fields to lists
            ReadKeys.Add(readKey);
            WriteKeys.Add(writeKey);

            string convertedMoveKey = "";

            foreach (char c in moveKey)
            {
                if (c == 'r')
                {
                    convertedMoveKey += ">";
                }
                else if (c == 'l')
                {
                    convertedMoveKey += "<";
                }
                else
                {
                    convertedMoveKey += "-";
                }
            }

            MoveKeys.Add(convertedMoveKey);

            return AssembleLine(ReadKeys, WriteKeys, MoveKeys, isReadOnly);
        }

        List<string> GetReadKeys(string line, ref int i)
        {
            List<string> ReadKeys = new List<string>();
            string readKey = "";
            i = 0;
            while (line[i] != '|')
            {
                if (line[i] != ' ' && line[i] != ',')
                {
                    readKey += line[i];
                }
                else
                {
                    ReadKeys.Add(readKey);
                    readKey = "";
                }
                i++;
            }
            return ReadKeys;
        }

        List<string> GetWriteKeys(string line, ref int i)
        {
            List<string> WriteKeys = new List<string>();
            string writeKey = "";
            while (line[i] != '>' && line[i] != '<' && line[i] != '-')
            {
                if (line[i] != ' ' && line[i] != ',')
                {
                    writeKey += line[i];
                }
                else
                {
                    if (writeKey != "")
                    {
                        WriteKeys.Add(writeKey);
                    }
                    writeKey = "";
                }
                i++;
            }
            return WriteKeys;
        }

        List<string> GetMoveKeys(string line, ref int i)
        {
            List<string> MoveKeys = new List<string>();
            string moveKey = "";
            while (i <= line.Length)
            {
                if (i == line.Length)
                {
                    if (moveKey != "")
                    {
                        MoveKeys.Add(moveKey);
                    }
                    moveKey = "";
                }
                else if (line[i] != ' ' && line[i] != ',')
                {
                    moveKey += line[i];
                }
                else
                {
                    if (moveKey != "")
                    {
                        MoveKeys.Add(moveKey);
                    }
                    moveKey = "";
                }
                i++;
            }


            return MoveKeys;
        }

        string AssembleLine(List<string> ReadKeys, List<string> WriteKeys, List<string> MoveKeys, bool isReadOnly)
        {
            string newLine = "";

            for (int j = 0; j < ReadKeys.Count; j++)
            {
                newLine += ReadKeys[j];
                if (j != ReadKeys.Count - 1)
                {
                    newLine += ",";
                }
                else
                {
                    newLine += " | ";
                }
            }
            if (!isReadOnly)
            {
                for (int k = 0; k < WriteKeys.Count; k++)
                {
                    newLine += WriteKeys[k];
                    if (k != WriteKeys.Count - 1)
                    {
                        newLine += ",";
                    }
                    else
                    {
                        newLine += " ";
                    }
                }
            }
            for (int l = 0; l < MoveKeys.Count; l++)
            {
                newLine += MoveKeys[l];
                if (l != MoveKeys.Count - 1)
                {
                    newLine += ",";
                }
            }

            return newLine;
        }
    }
}