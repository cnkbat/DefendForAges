using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RenderType
{
    Opaque,
    Fade,
    Transparent,
}

public class ObjectFader : MonoBehaviour, IFadeable
{
    GameManager gameManager;
    private Renderer[] renderers;
    public List<Material> materials;

    [Header("Render Types")]
    [SerializeField] private RenderType fadedRenderType = RenderType.Fade;
    [SerializeField] private RenderType originalRenderType = RenderType.Opaque;
    bool isFade;

    private float maxTimer = 2;
    private float currentTimer = 0;

    void Start()
    {
        gameManager = GameManager.instance;

        renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                materials.Add(material);
            }
        }

    }

    void Update()
    {
        if (isFade)
        {
            FadeOut();

            currentTimer -= Time.deltaTime;

            if (currentTimer < 0)
            {
                isFade = false;
                ResetTimer();
            }
        }
        else
        {
            ResetFade();
            ResetTimer();
        }
    }


    public void FadeOut()
    {

        for (int i = 0; i < materials.Count; i++)
        {
            Material currentMat = materials[i];

            currentMat.SetFloat("_RenderingMode", (int)fadedRenderType);

            currentMat.SetColor("_BaseColor", new Color(currentMat.GetColor("_BaseColor").r,
                                currentMat.GetColor("_BaseColor").g, currentMat.GetColor("_BaseColor").b,
                                    Mathf.Lerp(currentMat.GetColor("_BaseColor").a, gameManager.fadedObjectAlphaValue,
                                        gameManager.fadingSpeed * Time.deltaTime)));
        }
    }

    public void ResetFade()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            Material currentMat = materials[i];

            currentMat.SetFloat("_RenderingMode", (int)originalRenderType);
            currentMat.SetColor("_BaseColor", new Color(currentMat.GetColor("_BaseColor").r,
                  currentMat.GetColor("_BaseColor").g, currentMat.GetColor("_BaseColor").b,
                  Mathf.Lerp(currentMat.GetColor("_BaseColor").a, 1, gameManager.fadingSpeed * Time.deltaTime)));

        }
    }

    private void ResetTimer()
    {
        currentTimer = maxTimer;
    }


    public void SetFade(bool value)
    {
        isFade = value;
    }
}