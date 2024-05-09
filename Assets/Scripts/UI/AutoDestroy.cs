using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	[SerializeField] private float secondsToDestroy;

    private void Start()
	{
		Destroy(gameObject, secondsToDestroy);
	}
}