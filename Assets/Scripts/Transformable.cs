using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls transformable objects, in this case the primitive cubes.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class Transformable : MonoBehaviour, ISelectable
{
    public List<GimbalValue> gimbalValues = new List<GimbalValue>();

    private Vector3 mOffset;

    private void OnEnable()
    {
        if (InputController.instance.gimbalManager)
        {
            InputController.instance.gimbalManager.SaveGimbalValues(gimbalValues, InputController.instance.gimbalManager.defaultValues);
        }
    }

    /// <summary>
    /// Deselect the previously selected cube and select the current cube.
    /// </summary>
    public void Select()
    {
        InputController.instance.previousSelectable?.Deselect();

        InputController.instance.selectedObject = transform;
        ToggleSelectedObjectUI(true);
        mOffset = InputController.instance.GetMousePos() - transform.position;
    }

    /// <summary>
    /// Deselect the selected cube.
    /// </summary>
    public void Deselect()
    {
        ToggleSelectedObjectUI(false);
        InputController.instance.selectedObject = null;
    }

    /// <summary>
    /// Cube's position will be transformed by an amount relative to cursor displacement.
    /// </summary>
    public void Drag()
    {
        transform.position = InputController.instance.GetMousePos() - mOffset;
    }

    public void Drop()
    {
    }

    /// <summary>
    /// Toggle outline & gimbal for the selected cube.
    /// Save gimbal transform data if toggle off, and load gimbal transform data if toggle on.
    /// </summary>
    /// <param name="toggleOn">Should outline & gimbal be toggled on/off.</param>
    public void ToggleSelectedObjectUI(bool toggleOn)
    {
        var selectedObj = InputController.instance.selectedObject;
        if (selectedObj && selectedObj.GetComponent<Outline>())
        {
            selectedObj.GetComponent<Outline>().enabled = toggleOn;
        }
        InputController.instance.gimbal.SetActive(toggleOn);

        if (InputController.instance.gimbalManager)
        {
            if (toggleOn)
            {
                InputController.instance.gimbalManager.LoadGimbalValues(InputController.instance.gimbalManager.gimbals, gimbalValues);
            }
            else
            {
                InputController.instance.gimbalManager.SaveGimbalValues(gimbalValues, InputController.instance.gimbalManager.gimbals);
            }
        }
    }
}
