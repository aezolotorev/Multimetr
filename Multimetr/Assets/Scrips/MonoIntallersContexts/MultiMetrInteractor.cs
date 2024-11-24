using System;
using System.Collections.Generic;
using UniRx;

public class MultiMetrInteractor: IDisposable
{
    private MultiMetr _multimetr;
    private IAssetService _assetService;
    private VoltageData _voltageData;
    private ReactiveProperty <int> currIndexInfo = new ReactiveProperty<int>(0);
    
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    public MultiMetrInteractor(MultiMetr multimetr, IAssetService assetService, 
         InputController inputController)
    {
        _multimetr = multimetr;
        _assetService = assetService;
        inputController.ChangeVoltage.Subscribe(_ => NextInfo()).AddTo(_disposable);
        _multimetr.Init();
        SetVoltageInfo();
    }
    
    public async void SetVoltageInfo()
    {
        _voltageData = await _assetService.GetAssetAsync<VoltageData>("VoltageData");
        _multimetr.SetVoltageInfo(_voltageData.voltages, currIndexInfo);
    }

    public void NextInfo()
    {
        currIndexInfo.Value = (currIndexInfo.Value + 1) % _voltageData.voltages.Count;
    }
    public void Dispose()
    {
        _disposable.Dispose();
    }
}