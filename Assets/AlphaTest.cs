using UnityEngine;

public class AlphaTest : MonoBehaviour
{
    Material mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Material>();
        mat.SetFloat("_AlphaSpeed", 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
