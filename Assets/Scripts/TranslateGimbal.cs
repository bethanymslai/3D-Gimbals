using UnityEngine;

/// <summary>
/// Controls translate gimbals.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class TranslateGimbal : GimbalController
{
    public override void Select()
    {
        // account for offset between click pos & object center to avoid cube jumping to cursor pos
        mOffset = transform.parent.position - InputController.instance.selectedObject.transform.position;
        mPreviousMousePos = Vector3.Project(InputController.instance.GetMousePos(), mOffset.normalized);
    }

    /// <summary>
    /// Cube will be translated along an axis by an amount relative to cursor displacement.
    /// </summary>
    public override void Drag()
    {
        var direction = mOffset.normalized;
        var currentMousePos = Vector3.Project(InputController.instance.GetMousePos(), direction);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(currentMousePos - mPreviousMousePos);
        InputController.instance.selectedObject.Translate(localAmountCursorMoved * mSensitivity);
        InputController.instance.gimbal.transform.Translate(localAmountCursorMoved * mSensitivity);
        
        mPreviousMousePos = currentMousePos;
    }
}
