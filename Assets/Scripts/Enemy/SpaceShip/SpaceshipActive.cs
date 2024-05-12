using UnityEngine;

public class SpaceshipActive : MonoBehaviour
{
    public GameObject objectToCount; // Parent GameObject whose children's activity will be checked
    public GameObject objectToActivate; // GameObject to activate when all children of objectToCount are inactive
    public GameObject objectToActivate2; // Additional GameObject to activate when all children of objectToCount are inactive

    // Update is called once per frame
    void Update()
    {
        bool allChildrenInactive = true; // Flag to track if all children of objectToCount are inactive

        // Iterate through all children of objectToCount
        foreach (Transform child in objectToCount.transform)
        {
            // If any child is active, set the allChildrenInactive flag to false and break out of the loop
            if (child.gameObject.activeSelf)
            {
                allChildrenInactive = false;
                break;
            }
        }

        // Activate or deactivate GameObjects based on the state of the children
        if (allChildrenInactive)
        {
            // Activate the GameObjects when all children are inactive
            objectToActivate.SetActive(true);
            objectToActivate2.SetActive(true);
        }
        else
        {
            // Deactivate the GameObjects when any child is active
            objectToActivate.SetActive(false);
            objectToActivate2.SetActive(false);
        }
    }
}
