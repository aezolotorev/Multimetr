using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UniRx;
using DG.Tweening;

public struct InteractMultimetrMessage
{
    public bool IsActive;
}

public struct MultimetrViewProtocol
{
    
    public ReactiveCommand <MultimetrMode> SetModeCommand;
    public ReactiveCommand <Vector3> SetPosCommand;
    public ReactiveCommand <string> SetInfoCommand;
}

public class MultiMetrView : MonoBehaviour, IDisposable
{
    [SerializeField] private Transform wheel;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private MeshRenderer  wheelMesh;
    [SerializeField] private List<ModeAngle> modeAngles;

    
    private bool isActive=false;
    private MultimetrViewProtocol _protocol;
    
    private CompositeDisposable _disposables = new CompositeDisposable();
    
    public void Init(MultimetrViewProtocol protocol)
    {
        _protocol = protocol;
       
        _protocol.SetPosCommand.Subscribe(SetPos).AddTo(_disposables);
        _protocol.SetModeCommand.Subscribe(SetWheel).AddTo(_disposables);
        _protocol.SetInfoCommand.Subscribe(SetDisplayInfo).AddTo(_disposables);
    }

    private void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private void SetDisplayInfo(string info)
    {
        display.text = info;
    }

    private void SetWheel(MultimetrMode mode)
    {
        foreach (var m in modeAngles)
        {
            if (m.mode == mode)
            {
                wheel.localRotation = Quaternion.Euler(0, -90, m.angle);
                break;
            }
        }
    }

    public void Interact(bool isInteract)
    {
        HideOutWheel(isInteract);
        isActive = isInteract;
        
        MessageBroker.Default.Publish(new InteractMultimetrMessage()
            {
                IsActive = isActive
            }
        );
    }

    private void HideOutWheel(bool isHide)
    {
        var material = wheelMesh.material;
        material.color = isHide ? Color.green : Color.white; ;
    }
}