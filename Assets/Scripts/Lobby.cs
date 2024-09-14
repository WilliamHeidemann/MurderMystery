using System.Collections.Generic;
using UnityEngine;

public class Lobby
{
    private readonly List<string> _playerNames = new();
    public string[] GetNames() => _playerNames.ToArray();
    public void AddPlayer(string name) => _playerNames.Add(name);
}