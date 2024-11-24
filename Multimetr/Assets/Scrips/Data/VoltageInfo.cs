using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject.Internal;
[Serializable]
public struct VoltageInfo
{
    public float resistance;
    public float power;
}

[CreateAssetMenu(menuName = "Data/VoltageData")]
public class VoltageData: ScriptableObject
{
    public List<VoltageInfo> voltages = new List<VoltageInfo>();
}