using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private Pose _placementPose;
    private Transform _placementTransform;
    private bool _placementPoseIsValid = false;
    private bool _isObjectPlaced = false;
    private TrackableId _placePlaneId = TrackableId.invalidId;

    private ARRaycastManager m_RaycastManager;
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public static event Action onPlacedObject; 
    
    private void Awake() => m_RaycastManager = GetComponent<ARRaycastManager>();

    private void Update()
    {
        if (!_isObjectPlaced)
        {
            UpdatePlacementPosition();
            UpdatePlacementIndicator();
            
            if(_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void UpdatePlacementPosition()
    {
        if (Camera.main != null)
        {
            var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            if(m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                _placementPoseIsValid = s_Hits.Count > 0;
                if (_placementPoseIsValid)
                {
                    _placementPose = s_Hits[0].pose;
                    _placePlaneId = s_Hits[0].trackableId;
                
                    var planeManager = GetComponent<ARPlaneManager>();
                    var arPlane = planeManager.GetPlane(_placePlaneId);
                    _placementTransform = arPlane.transform;
                
                }
            }
        }
    }
    
    private void UpdatePlacementIndicator()
    {
        if (_placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementTransform.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
    
    private void PlaceObject()
    {
        Instantiate(objectToPlace, _placementPose.position, _placementTransform.rotation);
        onPlacedObject?.Invoke();
        _isObjectPlaced = true;
        placementIndicator.SetActive(false);
    }
}

