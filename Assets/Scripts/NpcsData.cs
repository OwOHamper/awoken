using System;
using System.Collections.Generic;

// [Serializable]
// public class Npc
// {
//     public string id;
//     public string name;
//     public List<string> requirements;
//     public string notFullfilingRequirements;
//     public List<string> lines;
//     public List<string> npcLines;

// }

[Serializable]
public struct Npc
{
    public string id;
    public string name;
    public List<string> requirements;
    public string notFullfilingRequirements;
    public Line lines;
}

[Serializable]
public struct Line
{
    public Replica replica;
}

[Serializable]
public struct Replica
{
    public string type;
    public List<Choice> choices;
}


[Serializable]
public struct Choice
{
    public string line;
    public List<string> requirements;
    public Outcome outcome;
    public Next next;
}

[Serializable]
public struct Outcome
{
    public string id;
    public string value;
}

[Serializable]
public struct Next
{
    public Replica replica;
}

[Serializable]
public struct NpcsData
{
    public List<Npc> npcs;
}
