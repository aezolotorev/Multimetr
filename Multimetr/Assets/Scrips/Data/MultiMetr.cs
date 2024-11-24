using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;
using Zenject;

public class MultiMetr: IDisposable
{
    
    private MultiMetrView _multimetrView;
    private MultimetrInfo _multimetrInfo;
    
    private IAssetService _assetService;
    private IInstantiator _instantiator ;
    private ISceneObjectContainer _sceneObjectContainer;
    private InputController _inputController;
    
    private MultimetrViewProtocol _protocolView;
    private MultimetrInfoProtocol _protocolInfo;
    
    private MultimetrMode _currentMode;
    private List<VoltageInfo> _voltageInfo;
    private VoltageInfo _currentInfo;

    private bool isActive;
    
    private IDisposable _subscriberScroll;
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    public MultiMetr(IAssetService assetService, IInstantiator instantiator, ISceneObjectContainer sceneObjectContainer, InputController inputController)
    {
        _assetService = assetService;
        _instantiator = instantiator;
        _sceneObjectContainer = sceneObjectContainer;
        _inputController = inputController;
    }

    public void SetVoltageInfo(List<VoltageInfo> info, ReactiveProperty<int> index)
    {
        _voltageInfo = info;
        index.Subscribe(ChangeVoltage).AddTo(_disposable);
    }

    public void ChangeVoltage(int index)
    {
        _currentInfo = _voltageInfo[index];
        if (isActive)
        {
            CalculateAndNotify(_currentMode, _currentInfo);
        }
    }

    public async void Init()
    {
        var data = await  _assetService.GetAssetAsync<MultimetrData>("MultimetrData");

        _protocolView = new MultimetrViewProtocol()
        {
            SetPosCommand = new ReactiveCommand<Vector3>(),
            SetModeCommand = new ReactiveCommand<MultimetrMode>(),
            SetInfoCommand = new ReactiveCommand<string>()
        };
        _multimetrView = _instantiator.InstantiatePrefabForComponent<MultiMetrView>(data.multimetrViewPrefab);
        _protocolView.SetPosCommand.Execute(_sceneObjectContainer.MultimetrContainer.position);
        _multimetrView.Init(_protocolView);
        
        _protocolInfo = new MultimetrInfoProtocol()
        {
            SetInfoCommand = new ReactiveCommand<(MultimetrMode, string)>(),
            ShowCommand = new ReactiveCommand<bool>()
        };
        _multimetrInfo = _instantiator.InstantiatePrefabForComponent<MultimetrInfo>(data.infoPrefab, _sceneObjectContainer.CanvasContainer);
        _multimetrInfo.Init(_protocolInfo);
        
        _currentMode = MultimetrMode.Neutral;
        _protocolView.SetModeCommand.Execute(_currentMode);
        
        MessageSubscriber();
    }

    private void MessageSubscriber()
    {
        MessageBroker.Default.Receive<InteractMultimetrMessage>().Subscribe(message =>
        {
            isActive = message.IsActive;
            
            if (isActive)
            {
                _subscriberScroll = _inputController.ScrollProperty.Subscribe(Scroll).AddTo(_disposable); }
            else
            {
                _subscriberScroll.Dispose();
            }

            _protocolInfo.ShowCommand.Execute(isActive);
        }).AddTo(_disposable);
    }
    
    private void Scroll(int value)
    {
        if(value==0) return;
        
        var newMode = ((int)_currentMode + value) % Enum.GetNames(typeof(MultimetrMode)).Length;
        
        if (newMode >= Enum.GetNames(typeof(MultimetrMode)).Length)
        {
            newMode = 0;
        }

        if (newMode < 0)
        {
            newMode = Enum.GetNames(typeof(MultimetrMode)).Length - 1;
        }
        
        _currentMode = (MultimetrMode)newMode;
        
        _protocolView.SetModeCommand.Execute(_currentMode);

        CalculateAndNotify(_currentMode, _currentInfo);
    }

    public void CalculateAndNotify(MultimetrMode mode, VoltageInfo info)
    {
        var dataInfo = CreateInfo(mode, info);
        _protocolInfo.SetInfoCommand.Execute((mode, InfoTranslater(dataInfo)));
        _protocolView.SetInfoCommand.Execute(InfoTranslater(dataInfo));
    }

    private float CreateInfo(MultimetrMode mode, VoltageInfo info)
    {
        switch (_currentMode)
        {
            case MultimetrMode.Neutral:
                return NeitralInfo(info);
            case MultimetrMode.AmperMetr:
                return AmperMetrInfo(info);
            case MultimetrMode.VoltMetr:
                return VoltMetrInfo(info);
            case MultimetrMode.OhmMetr:
                return OhmMetrInfo(info);
            case MultimetrMode.ACVoltMetr:
                return AcVoltMetrInfo(info);
            default:
                return 0;
        }
    }

    private string InfoTranslater(float infoData)
    {
        return infoData.ToString("F2");
    }
    
    private float NeitralInfo(VoltageInfo info)
    {
        return 0;
    }

    private float AmperMetrInfo(VoltageInfo info)
    {
        return info.power*info.power*info.resistance;
    }
    
    private float VoltMetrInfo(VoltageInfo info)
    {
        return Mathf.Sqrt(info.power * info.resistance);
    }
    
    private float AcVoltMetrInfo(VoltageInfo info)
    {
        return 0.01f;
    }
    
    private float OhmMetrInfo(VoltageInfo info)
    {
        return info.resistance;
    }

    public void Dispose()
    {
        _subscriberScroll.Dispose();
        _disposable.Dispose();
    }
}