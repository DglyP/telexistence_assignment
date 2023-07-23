// This script is responsible for creating a scrollable list of TMP_Text objects with numbers.

using UnityEngine;
using TMPro;

public class ScrollTimeSelector : MonoBehaviour
    {
    [SerializeField] private TMP_Text tmpPrefab; // Reference to the TMP_Text prefab to be instantiated.
    [SerializeField] private Transform parentTransform; // Parent transform to hold the TMP_Text objects.
    [SerializeField] private int numberOfObjects = 100; // The number of TMP_Text objects to create.
    [SerializeField] private Vector2 spacing = new Vector2(0, -30); // The spacing between each TMP_Text object.

    private void Start()
        {
        CreateTMPObjectsWithNumbers();
        }

    // Creates TMP_Text objects with numbers and arranges them in a scrollable list.
    private void CreateTMPObjectsWithNumbers()
        {
        for (int i = 0; i < numberOfObjects; i++)
            {
            // Instantiate a new TMP_Text object from the prefab.
            TMP_Text tmpObject = Instantiate(tmpPrefab, parentTransform);

            // Set the text of the TMP_Text object to the current index (number).
            tmpObject.text = i.ToString();

            // Set the position of the TMP_Text object based on its index and spacing.
            tmpObject.rectTransform.anchoredPosition = new Vector2(spacing.x * i, spacing.y * i);

            // Activate the TMP_Text object to make it visible.
            tmpObject.gameObject.SetActive(true);
            }
        }
    }
