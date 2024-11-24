using UniRx;
using UnityEngine;

public struct MultimetrInfoProtocol
{
    public ReactiveCommand <(MultimetrMode, string)> SetInfoCommand;
    public ReactiveCommand<bool> ShowCommand;
}