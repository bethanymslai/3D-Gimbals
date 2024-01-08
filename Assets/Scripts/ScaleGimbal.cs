using UnityEngine;

/// <summary>
/// Controls scale gimbals.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class ScaleGimbal : GimbalController
{
    [SerializeField] protected Transform rotationGimbals;

    public override void Select()
    {
        // account for offset between click pos & object center to avoid cube jumping to cursor pos
        mOffset = transform.parent.position - InputController.instance.selectedObject.transform.position;
        mPreviousMousePos = Vector3.Project(InputController.instance.GetMousePos(), mOffset.normalized);
    }

    /// <summary>
    /// Cube will be scaled along an axis by an amount relative to cursor displacement.
    /// </summary>
    public override void Drag()
    {
        var direction = mOffset.normalized;
        var currentMousePos = Vector3.Project(InputController.instance.GetMousePos(), direction);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(currentMousePos - mPreviousMousePos);
        var newScale = InputController.instance.selectedObject.localScale + localAmountCursorMoved * mSensitivity;
        
        // Translate gimbal handles so they stay a constant distance offset from the cube.
        if (newScale.x > 0 && newScale.y > 0 && newScale.z > 0)
        {
            transform.parent.Translate(localAmountCursorMoved);
            rotationGimbals.localScale += localAmountCursorMoved;
            InputController.instance.selectedObject.localScale = newScale;
        }
        
        mPreviousMousePos = currentMousePos;
    }
}
