using UnityEngine;

/// <summary>
/// Renders the rotation gimbal visual.
/// Last edited: Bethany Lai 01/09/2024
/// </summary>
public class CircleLineRenderer : MonoBehaviour
{
    private LineRenderer mCircleRenderer;
    private MeshCollider mMeshCollider;

    private void Awake()
    {
        mCircleRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Draw the rotation gimbal line renderer.
        DrawCircle(100, 1);
        Mesh mesh = new Mesh();
        mMeshCollider = gameObject.AddComponent<MeshCollider>();
        mCircleRenderer.BakeMesh(mesh, false);
        mMeshCollider.sharedMesh = mesh;
        mMeshCollider.convex = true;
    }

    /// <summary>
    /// Draws the rotation gimbal using Line Renderer. Referenced from https://www.youtube.com/watch?v=DdAfwHYNFOE
    /// </summary>
    /// <param name="steps">The smoothness of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    void DrawCircle(int steps, float radius)
    {
        mCircleRenderer.positionCount = steps + 1;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            mCircleRenderer.SetPosition(currentStep, currentPosition);
        }
        mCircleRenderer.SetPosition(steps, mCircleRenderer.GetPosition(0));
    }
}
