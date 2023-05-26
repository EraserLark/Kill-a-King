using UnityEngine;

public class Solid : MonoBehaviour
{
	private bool isBlocking;
	private Renderer renderer;
	private Color matColor;

	private void Awake()
	{
		isBlocking = false;
		renderer = GetComponent<Renderer>();
		matColor = renderer.material.color;
	}

	public void FadeIn()
	{
		matColor.a = 1f;
		renderer.material.color = matColor;
	}

	public void FadeOut()
	{
		matColor.a = 0.35f;
		renderer.material.color = matColor;
	}
}
