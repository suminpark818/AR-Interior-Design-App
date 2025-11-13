using UnityEngine;
using System.Collections.Generic;

public class ColorApplicator : MonoBehaviour
{
    [SerializeField] private GameObject target; // 색을 바꿀 가구 루트
    private readonly List<Renderer> renderers = new();
    private MaterialPropertyBlock mpb;

    void Awake()
    {
        mpb = new MaterialPropertyBlock();
        CacheRenderers();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        CacheRenderers();
    }

    private void CacheRenderers()
    {
        renderers.Clear();
        if (!target) return;
        renderers.AddRange(target.GetComponentsInChildren<Renderer>(true));
    }

    public void ApplyColor(Color c)
    {
        if (renderers.Count == 0) return;

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(mpb);

            // URP/HDRP 우선, 그다음 내장 Standard
            if (HasProp(r, "_BaseColor")) mpb.SetColor("_BaseColor", c);
            else if (HasProp(r, "_Color")) mpb.SetColor("_Color", c);

            r.SetPropertyBlock(mpb);
        }
    }

    private bool HasProp(Renderer r, string name)
    {
        var mat = r.sharedMaterial;
        return mat && mat.HasProperty(name);
    }
}

