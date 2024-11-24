using UnityEngine;


public interface ISceneObjectContainer
{
    Transform MultimetrContainer { get; }
    RectTransform CanvasContainer { get; }
    
    Camera MainCamera { get; }
    
}