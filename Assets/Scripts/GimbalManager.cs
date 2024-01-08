using System.Collections.Generic;
using UnityEngine;

public struct GimbalValue
{
    public Vector3 gimbalPosition;
    public Quaternion gimbalRotation;
    public Vector3 gimbalScale;
}

/// <summary>
/// Manages the saving and loading of the gimbal's transform values per each cube.
/// This allows only one gimbal to be used for all cubes instead of having multiple gimbals per cube.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class GimbalManager : MonoBehaviour
{
    public Transform[] gimbals => mGimbals;
    public GimbalValue[] defaultValues => mDefaultValues;

    [SerializeField] private Transform[] mGimbals = new Transform[5];
    private GimbalValue[] mDefaultValues;

    private void Awake()
    {
        mDefaultValues = new GimbalValue[mGimbals.Length];
        SaveGimbalValues(mDefaultValues, mGimbals);
    }

    /// <summary>
    /// Load the transformable cubes's saved transform values into the gimbal transform.
    /// </summary>
    /// <param name="loadTo">List to load values to.</param>
    /// <param name="loadFrom">Array to load values from.</param>
    public void LoadGimbalValues(Transform[] loadTo, List<GimbalValue> loadFrom)
    {
        for (int i = 0; i < loadTo.Length; i++)
        {
            loadTo[i].localPosition = loadFrom[i].gimbalPosition;
            loadTo[i].localRotation = loadFrom[i].gimbalRotation;
            loadTo[i].localScale = loadFrom[i].gimbalScale;
        }
    }
    
    private void SaveGimbalValues(GimbalValue[] loadTo, Transform[] loadFrom)
    {
        for (int i = 0; i < loadTo.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = loadFrom[i].localPosition;
            values.gimbalRotation = loadFrom[i].localRotation;
            values.gimbalScale = loadFrom[i].localScale;
            loadTo[i] = values;
        }
    }

    public void SaveGimbalValues(List<GimbalValue> loadTo, GimbalValue[] loadFrom)
    {
        loadTo.Clear();
        for (int i = 0; i < loadFrom.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = loadFrom[i].gimbalPosition;
            values.gimbalRotation = loadFrom[i].gimbalRotation;
            values.gimbalScale = loadFrom[i].gimbalScale;
            loadTo.Add(values);
        }
    }

    public void SaveGimbalValues(List<GimbalValue> loadTo, Transform[] loadFrom)
    {
        loadTo.Clear();
        for (int i = 0; i < loadFrom.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = loadFrom[i].localPosition;
            values.gimbalRotation = loadFrom[i].localRotation;
            values.gimbalScale = loadFrom[i].localScale;
            loadTo.Add(values);
        }
    }
}
