using UnityEngine;

/// <summary>
/// Controls rotation gimbals.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class RotateGimbal : GimbalController
{
    [SerializeField] private bool isX = false, isY = false, isZ = false;
    private Vector3 mPerpendicularDirection;

    public override void Select()
    {
        // account for offset between click pos & object center to avoid cube jumping to cursor pos
        mOffset = transform.position - InputController.instance.selectedObject.transform.position;

        if (isX)
            mPerpendicularDirection = InputController.instance.selectedObject.up;
        else if (isY)
            mPerpendicularDirection = InputController.instance.selectedObject.right;
        else if (isZ)
            mPerpendicularDirection = InputController.instance.selectedObject.forward;

        mPreviousMousePos = GetMousePosWithDepth();
    }

    /// <summary>
    /// Cube will be rotated along an axis by an amount relative to cursor displacement.
    /// The rotation angle is calculated by the cursor displacement relative to the selected cube's position.
    /// </summary>
    public override void Drag()
    {
        var currentMousePos = GetMousePosWithDepth();
        var angle = Vector3.SignedAngle(mPreviousMousePos - InputController.instance.selectedObject.position,
                                        currentMousePos - InputController.instance.selectedObject.position,
                                        mPerpendicularDirection);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(angle * mPerpendicularDirection);
        InputController.instance.selectedObject.Rotate(localAmountCursorMoved * mSensitivity);
        InputController.instance.gimbal.transform.Rotate(localAmountCursorMoved * mSensitivity);
        mPreviousMousePos = currentMousePos;
    }

    /// <summary>
    /// Project the 2D mouse position onto a 3D plane defined by the axis of rotation.
    /// Referenced from https://www.habrador.com/tutorials/math/4-plane-ray-intersection/
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMousePosWithDepth()
    {
        Vector3 selectedObjectPosition = InputController.instance.selectedObject.position;
        Vector3 perpendicularDirection = mPerpendicularDirection;

        var ray = InputController.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 rayOrigin = ray.origin;
        Vector3 rayDirection = ray.direction;

        float denominator = Vector3.Dot(rayDirection, perpendicularDirection);
        
        // If the ray intersects with the plane, project it onto the plane to get the new mouse pos
        Vector3 mousePosWithDepth;
        if (!Mathf.Approximately(denominator, 0))
        {
            float t = Vector3.Dot(selectedObjectPosition - rayOrigin, perpendicularDirection) / denominator;
            mousePosWithDepth = rayOrigin + rayDirection * t;
            return mousePosWithDepth;
        }
        
        mousePosWithDepth = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), mPerpendicularDirection) 
                            + Vector3.Project(transform.position, mPerpendicularDirection);
        return mousePosWithDepth;
    }
}
