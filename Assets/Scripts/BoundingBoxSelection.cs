using UnityEngine;
using TMPro;

public class BoundingBoxSelection : MonoBehaviour
    {
    // Reference to the bounding box collider
    private BoxCollider2D boundingBoxCollider;

    private void Awake()
        {
        boundingBoxCollider = GetComponent<BoxCollider2D>();
        }

    private void Update()
        {
        if (Input.GetMouseButtonDown(0)) // Change the input method as needed
            {
            // Cast a ray from each corner of the bounding box and collect all intersected objects
            Ray[] rays = new Ray[4];

            Vector2 center = boundingBoxCollider.bounds.center;
            Vector2 extents = boundingBoxCollider.bounds.extents;

            rays[0] = Camera.main.ScreenPointToRay(new Vector3(center.x + extents.x, center.y + extents.y, 0f));
            rays[1] = Camera.main.ScreenPointToRay(new Vector3(center.x - extents.x, center.y + extents.y, 0f));
            rays[2] = Camera.main.ScreenPointToRay(new Vector3(center.x - extents.x, center.y - extents.y, 0f));
            rays[3] = Camera.main.ScreenPointToRay(new Vector3(center.x + extents.x, center.y - extents.y, 0f));

            RaycastHit2D[] hits;
            foreach (var ray in rays)
                {
                hits = Physics2D.RaycastAll(ray.origin, ray.direction);

                foreach (var hit in hits)
                    {
                    GameObject selectedObject = hit.collider.gameObject;
                    TextMeshProUGUI textMeshProUIComponent = selectedObject.GetComponent<TextMeshProUGUI>();
                    if (textMeshProUIComponent != null)
                        {
                        string textValue = textMeshProUIComponent.text;
                        Debug.Log("Selected Object Text: " + textValue);
                        // Do something with the selectedObject or its textValue, such as highlighting it or processing its selection.
                        }
                    }
                }
            }
        }
    }
