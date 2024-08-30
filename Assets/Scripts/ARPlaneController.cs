using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaneController : MonoBehaviour
{
    private ARPlaneManager m_ARPlaneManager;
    
    private void Awake() => m_ARPlaneManager = GetComponent<ARPlaneManager>();

    private void OnEnable()
    {
        PlaceObjectOnPlane.onPlacedObject += DisablePlaneDetection;
    }
    
    private void OnDisable()
    {
        PlaceObjectOnPlane.onPlacedObject -= DisablePlaneDetection;
    }
    
    private void DisablePlaneDetection()
    {
        SetAllPlanesActive(false);
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
    }
    
    private void SetAllPlanesActive(bool active)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }
}