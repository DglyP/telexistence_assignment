using UnityEngine;
using TMPro;

public class BoundingBoxSelection : MonoBehaviour
    {
    // Reference to the bounding box collider
    private BoxCollider2D boundingBoxCollider;

    private void Awake()
        {
        // Get the BoxCollider2D component attached to this GameObject
        boundingBoxCollider = GetComponent<BoxCollider2D>();
        }

    private void Update()
        {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click (Change the input method as needed)
            {
            // Cast a ray from each corner of the bounding box and collect all intersected objects
            Ray[] rays = new Ray[4];

            Vector2 center = boundingBoxCollider.bounds.center;
            Vector2 extents = boundingBoxCollider.bounds.extents;

            // Top-right corner ray
            rays[0] = Camera.main.ScreenPointToRay(new Vector3(center.x + extents.x, center.y + extents.y, 0f));

            // Top-left corner ray
            rays[1] = Camera.main.ScreenPointToRay(new Vector3(center.x - extents.x, center.y + extents.y, 0f));

            // Bottom-left corner ray
            rays[2] = Camera.main.ScreenPointToRay(new Vector3(center.x - extents.x, center.y - extents.y, 0f));

            // Bottom-right corner ray
            rays[3] = Camera.main.ScreenPointToRay(new Vector3(center.x + extents.x, center.y - extents.y, 0f));

            RaycastHit2D[] hits;
            foreach (var ray in rays)
                {
                // Perform a raycast and get all the hits
                hits = Physics2D.RaycastAll(ray.origin, ray.direction);

                foreach (var hit in hits)
                    {
                    // Get the selected object from the hit collider
                    GameObject selectedObject = hit.collider.gameObject;

                    // Check if the selected object has a TextMeshProUGUI component attached
                    TextMeshProUGUI textMeshProUIComponent = selectedObject.GetComponent<TextMeshProUGUI>();
                    if (textMeshProUIComponent != null)
                        {
                        // If the selected object has a TextMeshProUGUI component, get the text value from it
                        string textValue = textMeshProUIComponent.text;


                        // Do something with the selectedObject or its textValue,
                        // such as highlighting it or processing its selection.
                        }
                    }
                }
            }
        }
    }
