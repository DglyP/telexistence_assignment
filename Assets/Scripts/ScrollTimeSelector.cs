using UnityEngine;
using TMPro;

public class ScrollTimeSelector : MonoBehaviour
    {
    [SerializeField] private TMP_Text tmpPrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private int numberOfObjects = 100;
    [SerializeField] private Vector2 spacing = new Vector2(0, -30);

    private void Start()
        {
        CreateTMPObjectsWithNumbers();
        }

    private void CreateTMPObjectsWithNumbers()
        {
        for (int i = 0; i < numberOfObjects; i++)
            {
            TMP_Text tmpObject = Instantiate(tmpPrefab, parentTransform);
            tmpObject.text = i.ToString();

            // Set the position of the TMP object based on its index and spacing
            tmpObject.rectTransform.anchoredPosition = new Vector2(spacing.x * i, spacing.y * i);
            tmpObject.gameObject.SetActive(true);
            }
        }
    }
