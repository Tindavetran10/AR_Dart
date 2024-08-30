using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public class DartController : MonoBehaviour
{
    public GameObject dartPrefab;
    public Transform dartThrowPoint;
    private XROrigin _xrOrigin;
    private GameObject ARCam;
    private GameObject dartTemp;
    private Rigidbody rb;

    private void Start()
    {
        _xrOrigin = GameObject.Find("XR Origin").GetComponent<XROrigin>();
        ARCam = _xrOrigin.transform.Find("Main Camera").gameObject;
    }

    private void OnEnable()
    {
        PlaceObjectOnPlane.onPlacedObject += DartsInit;
    }
    
    private void OnDisable()
    {
        PlaceObjectOnPlane.onPlacedObject -= DartsInit;
    }

    private void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(raycast, out var raycastHit))
            {
                if (raycastHit.collider.CompareTag("dart"))
                {
                    //Disable back touch Collider from dart
                    raycastHit.collider.enabled = false;

                    dartTemp.transform.parent = _xrOrigin.transform;
                }
            }
        }
    }

    private void DartsInit() => StartCoroutine(WaitAndSpawnDart());

    private IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(0.1f);
        dartTemp = Instantiate(dartPrefab, dartThrowPoint.position, ARCam.transform.localRotation);
        dartTemp.transform.parent = ARCam.transform;
        rb = dartTemp.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
    }
}
