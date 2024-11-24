using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class MultimetrInfo : MonoBehaviour, IDisposable
{
    [SerializeField] private GameObject panel;
    [SerializeField] private List<TextMeshProUGUI> infos;
    
    MultimetrInfoProtocol _protocol;

    private CompositeDisposable _disposables = new CompositeDisposable();
    public void Init(MultimetrInfoProtocol protocol)
    {
        _protocol = protocol;
        _protocol.SetInfoCommand.Subscribe(SetInfo).AddTo(_disposables);
        _protocol.ShowCommand.Subscribe(Show).AddTo(_disposables);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Show(bool isShow)
    {
        panel.SetActive(isShow);
    }
    
    public void SetInfo((MultimetrMode mode, string value) info)
    {
        HideAll();
        if ((int)info.mode == (int)MultimetrMode.Neutral)
        {
            return;
        }
        
        infos[(int)info.mode].text = info.value;
    }

    private void HideAll()
    {
        foreach (var info in infos)
        {
            info.text = "0";
        }
    }
}