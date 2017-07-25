using System;
using System.Collections.Generic;
using UnityEngine;


// Used by OnSave / OnLoad callbacks
public delegate void SaveLoadCallback(Dictionary<string, byte[]> data);

public delegate void Notification();

