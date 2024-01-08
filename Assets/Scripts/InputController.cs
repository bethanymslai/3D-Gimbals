using UnityEngine;

/// <summary>
/// Manages the input data from the mouse, as well as world space mouse position data.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject gimbalPrefab;

    public Transform selectedObject
    {
        get => mSelectedObject;
        set => mSelectedObject = value;
    }
    public Transform hitObject 
    { 
        get => mHitObject;
        set => mHitObject = value;
    }
    public ISelectable previousSelectable => mPreviousSelectable;
    public GameObject gimbal => mGimbal;
    public GimbalManager gimbalManager => mGimbalManager;
    
    public Camera mainCamera => mMainCamera;

    private Transform mSelectedObject = null;
    private GameObject mGimbal = null;
    private Transform mHitObject;
    private ISelectable mPreviousSelectable = null;
    private GimbalManager mGimbalManager;
    private Camera mMainCamera;

    public static InputController instance;

    private void Awake()
    {
        instance = this;
        mMainCamera = Camera.main;
    }

    private void Start()
    {
        if (gimbalPrefab)
        {
            mGimbal = Instantiate(gimbalPrefab);
            mGimbal.SetActive(false);
            mGimbalManager = mGimbal.GetComponent<GimbalManager>();
        }
    }

    private void Update()
    {
        // selecting object
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            // save a reference to the currently selected Selectable
            if (mSelectedObject)
            {
                mPreviousSelectable = mSelectedObject.GetComponent<ISelectable>();
            }

            // If a Selectable has been hit, select it.
            // Else, deselect the currently selected Selectable.
            if (hit.collider)
            {
                var selectable = hit.transform.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    mHitObject = hit.collider.transform;
                    selectable.Select();
                }
            }
            else
            {
                if (mPreviousSelectable != null)
                {
                    mPreviousSelectable.Deselect();
                }
            }
        }

        // Drag the currently selected Selectable if holding down mouse button
        // Else, drop the currently selected Selectable.
        if (mSelectedObject)
        {
            var selectable = mHitObject.GetComponent<ISelectable>();
            if (selectable != null)
            {
                if (Input.GetMouseButton(0))
                {
                    selectable.Drag();
                }
                else
                {
                    selectable.Drop();
                }
            }
        }
    }

    /// <summary>
    /// Convert mouse position from screen space to world space.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMousePos()
    {
        if (mSelectedObject)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mMainCamera.WorldToScreenPoint(mSelectedObject.position).z);
            return mMainCamera.ScreenToWorldPoint(position);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Referenced from https://www.youtube.com/watch?v=uNCCS6DjebA&t=469s
    /// </summary>
    /// <returns></returns>
    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mMainCamera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mMainCamera.nearClipPlane);
        Vector3 worldMousePosFar = mMainCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mMainCamera.ScreenToWorldPoint(screenMousePosNear);
        
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }     
}
