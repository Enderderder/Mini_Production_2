using UnityEngine;
using System.Collections;

public class UVscroller : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.0f;
    public Renderer rend;

    private Vector2 defaultOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();

        defaultOffset = rend.material.GetTextureOffset(0);
    }
    void Update()
    {
        float offsetX = defaultOffset.x + (Time.time * scrollSpeedX);
        float offsetY = defaultOffset.y + (Time.time * scrollSpeedY);
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}