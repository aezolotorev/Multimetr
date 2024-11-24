using UnityEngine;


public class SceneObjectContainer : MonoBehaviour, ISceneObjectContainer
{
    [SerializeField] private Transform multimetrContainer;

    [SerializeField] private RectTransform canvasContainer;
    
    [SerializeField] private Camera mainCamera;

    public Transform MultimetrContainer => multimetrContainer;
    public RectTransform CanvasContainer => canvasContainer;
    
    public Camera MainCamera => mainCamera;
}