using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used by OnSave / OnLoad callbacks
/// </summary>
public delegate void SaveLoadCallback(Dictionary<string, byte[]> data);

/// <summary>
/// General no parameter delegate
/// </summary>
public delegate void Notification();

