using UnityEngine;

/// <summary>
/// Controls the cube spawner.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class CubeSpawner : MonoBehaviour, ISelectable
{
    [SerializeField] private Color grabbedObjectOutlineColor = Color.yellow;

    public void Select()
    {
        SpawnCube(InputController.instance.GetMousePos());
    }
    
    public void Deselect()
    {
    }

    public void Drag()
    {
        
    }

    public void Drop()
    {

    }

    /// <summary>
    /// Spawns a cube primitive and selects it.
    /// </summary>
    /// <param name="spawnPosition">Position to spawn the cube at.</param>
    private void SpawnCube(Vector3 spawnPosition)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spawnPosition.z = 0;
        cube.transform.position = spawnPosition;

        // add outline script
        var outline = cube.AddComponent<Outline>();
        outline.enabled = false;
        outline.OutlineColor = grabbedObjectOutlineColor;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;

        // add Transformable script and select the newly spawned cube
        var cubeSelectable = cube.AddComponent<Transformable>();
        InputController.instance.previousSelectable?.Deselect();
        InputController.instance.selectedObject = cube.transform;
        InputController.instance.hitObject = cube.transform;
        cubeSelectable.ToggleSelectedObjectUI(true);
    }
}
