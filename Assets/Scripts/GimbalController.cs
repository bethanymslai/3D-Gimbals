using UnityEngine;

/// <summary>
/// Base class for position, rotation, and scale gimbal handles.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class GimbalController : MonoBehaviour, ISelectable
{
    [SerializeField] protected float mSensitivity = 1f;
    protected Vector3 mOffset;
    protected Vector3 mPreviousMousePos;

    private void Update()
    {
        // Update gimbal position if a Selectable is selected.
        if (InputController.instance.selectedObject && InputController.instance.gimbal.transform)
        {
            InputController.instance.gimbal.transform.position = InputController.instance.selectedObject.transform.position;
        }
    }

    public virtual void Select()
    {
    }

    public virtual void Deselect()
    {
    }

    public virtual void Drag()
    {
    }

    public virtual void Drop()
    {
    }
}
