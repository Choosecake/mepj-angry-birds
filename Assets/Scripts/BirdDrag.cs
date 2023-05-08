using UnityEngine;
using UnityEngine.Serialization;

public class BirdDrag : MonoBehaviour
{
    [SerializeField] private GameObject slingshotHolder;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private new Camera camera;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private float dragSpeed;
    [SerializeField] private float maxDistance;
    [HideInInspector] public bool isDraggingBird;
    public float lineDistance;
    private Slingshot _slingshot;
    private Material _originalMaterial;
    private MeshRenderer _meshRenderer;
    private Vector3 _originalPosition;
    private Ray _ray;
    private RaycastHit _hit;
    private bool _canDragBird;
    private bool _isReturning;

    private void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _originalMaterial = _meshRenderer.material;
        
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (!_slingshot || !_slingshot.gameObject.activeSelf)
        {
            _slingshot = slingshotHolder.transform.GetChild(gameManager._currentSlingshot).gameObject.GetComponent<Slingshot>();
        }
        
        if (_slingshot.canShootBird) return;

        _ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Vector3.Distance(transform.position, _originalPosition) < 0.2f)
        {
            _isReturning = false;
        }

        if (!CanDragBird())
        {
            _meshRenderer.material = _originalMaterial;
            isDraggingBird = false;
            ReturnToOriginalPosition();
            return;
        }
        
        _meshRenderer.material = selectedMaterial;
        
        // drag the bird while holding lmb
        if (Input.GetMouseButton(0))
        {
            DragBird();
        }
        else
        {
            isDraggingBird = false;
        }
    }

    private void DragBird()
    {
        Vector3 rayPoint = _ray.GetPoint(0);
        Vector3 targetPosition = new Vector3(rayPoint.x, rayPoint.y, 0);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed * Time.deltaTime);
        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, maxDistance);
        
        isDraggingBird = true;
    }

    private bool CanDragBird()
    {
        if (isDraggingBird)
        {
            while (Input.GetMouseButton(0))
            {
                return true;
            }

            _slingshot.canShootBird = true;
            StartCoroutine(_slingshot.ShootBird());
            return false;
        }
        
        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.CompareTag("Player"))
            {
                if (_isReturning)
                {
                    return false;
                }
                
                return true;
            }
        }

        return false;
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _originalPosition, (dragSpeed * 2) * Time.deltaTime);
        _isReturning = true;
    }
}