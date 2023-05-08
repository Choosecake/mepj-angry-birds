using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] private GameObject birdHolder;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float stiffness;
    [SerializeField] private int numTrajectoryPoints = 50;
    [HideInInspector] public bool canShootBird;
    [HideInInspector] public bool isShootingBird;
    [HideInInspector] public bool birdWasShot;
    private GameObject _bird;
    private BirdDrag _birdDrag;
    private Rigidbody _birdRigidbody;
    private LineRenderer _lineRenderer;
    private Vector3 _displacement;
    private Vector3 _springForce;
    private Vector3 _birdPosition;
    private Vector3 _trajectoryEndPosition;
    private Vector3[] _trajectoryPoints;
    private float _springForceMagnitude;

    private void Start()
    {
        _trajectoryPoints = new Vector3[numTrajectoryPoints];
        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            _trajectoryPoints[i] = Vector3.zero;
        }
    }

    private void Update()
    {
        if (!_bird || !_bird.activeSelf)
        {
            _bird = birdHolder.transform.GetChild(gameManager._currentBird).gameObject;
        }
        
        if (_bird)
        {
            _birdDrag = _bird.GetComponent<BirdDrag>();
            _birdRigidbody = _bird.GetComponent<Rigidbody>();
        }

        if (!_lineRenderer)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        
        _birdPosition = _bird.transform.position;
        
        if (_birdDrag.isDraggingBird) _lineRenderer.SetPositions(_trajectoryPoints);

        if (isShootingBird) return;
        
        UpdateTrajectory();

        _displacement = transform.position - _birdPosition;

        // hooke's law
        _springForceMagnitude = -stiffness * _displacement.magnitude;
        _springForce = _springForceMagnitude * -_displacement.normalized;
        
    }

    public IEnumerator ShootBird()
    {
        _birdRigidbody.useGravity = true;
        _birdRigidbody.AddForce(_springForce, ForceMode.Force);
        isShootingBird = true;
        birdWasShot = true;
        yield break;
    }

    private void UpdateTrajectory()
    {
        float timeStep = 0.1f;
        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = _birdPosition + -_displacement * _springForceMagnitude / stiffness * t + Physics.gravity * (t * t) / _birdDrag.lineDistance;
            _trajectoryPoints[i] = point;
        }

        _lineRenderer.positionCount = numTrajectoryPoints;
    }
}
