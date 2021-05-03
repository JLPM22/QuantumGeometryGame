using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeColorTMPRO : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color DefaultColor;
    [ColorUsage(true, true)]
    public Color HighlightColor;

    public TextMeshProUGUI Text;

    private bool Highlighted;
    private Material Material;

    private void Awake()
    {
        Material = Text.fontMaterial;
        Material.SetColor("_FaceColor", DefaultColor);
    }

    public void ChangeColor()
    {
        if (Highlighted)
        {
            Material.SetColor("_FaceColor", DefaultColor);
        }
        else
        {
            Material.SetColor("_FaceColor", HighlightColor);
        }
        Highlighted = !Highlighted;
    }

    private void OnDestroy()
    {
        if (Material != null) Destroy(Material);
    }
}
