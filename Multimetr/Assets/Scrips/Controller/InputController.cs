using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class InputController: IDisposable
{
    public ReactiveProperty<int> ScrollProperty = new ReactiveProperty<int>();
    public ReactiveCommand ChangeVoltage = new ReactiveCommand();
    
    private Camera _camera;
    private MultiMetrView _multimetr;
    
    private CompositeDisposable _disposables = new CompositeDisposable();
    
    public InputController (ISceneObjectContainer sceneObjectContainer)
    {
        _camera = sceneObjectContainer.MainCamera;
        Init();
    }

    public void Init()
    {
        _camera = Camera.main;
        Observable.EveryUpdate().Subscribe(_ => Tick()).AddTo(_disposables );
    }
    

    public void Tick()
    {
        // Get the mouse position
        Vector3 mousePosition = Input.mousePosition;
       
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag(("Multimetr")))
            {
                if (_multimetr == null)
                {
                    _multimetr = hit.transform.GetComponent<MultiMetrView>();
                    _multimetr?.Interact(true);
                }
            }
            else
            {
                _multimetr?.Interact(false);
                _multimetr = null;
            }
        }
        else
        {
            _multimetr?.Interact(false);
            _multimetr = null;
        }

        var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
       
        if (mouseWheel > 0.1)
        {
            ScrollProperty.Value = 1;
        }
        else if (mouseWheel< -0.1)
        {
            ScrollProperty.Value = -1;
        }
        else
        {
            ScrollProperty.Value = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeVoltage.Execute();
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
