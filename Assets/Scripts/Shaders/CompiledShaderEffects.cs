using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
/// <summary>
/// CODE FROM: https://github.com/GarrettGunnell/Post-Processing
/// Compiled to reduce drawcalls
/// </summary>
public class CompiledShaderEffects : MonoBehaviour
{
    [Header("Shaders")]
    public Shader ditherShader;
    public Shader colorCorrectionShader;

    [Header("Dither Settings")]
    [Range(0.0f, 1.0f)]
    public float spread = 0.5f;

    [Range(2, 16)]
    public int redColorCount = 2;
    [Range(2, 16)]
    public int greenColorCount = 2;
    [Range(2, 16)]
    public int blueColorCount = 2;

    [Range(0, 2)]
    public int bayerLevel = 0;

    [Range(0, 8)]
    public int downSamples = 0;

    public bool pointFilterDown = false;

    private Material ditherMat;

    [Header("Color Correction Settings")]
    public Vector3 exposure = new Vector3(1.0f, 1.0f, 1.0f);

    [Range(-100.0f, 100.0f)]
    public float temperature, tint;

    public Vector3 contrast = new Vector3(1.0f, 1.0f, 1.0f);

    public Vector3 linearMidPoint = new Vector3(0.5f, 0.5f, 0.5f);

    public Vector3 brightness = new Vector3(0.0f, 0.0f, 0.0f);

    [ColorUsageAttribute(false, true)]
    public Color colorFilter;

    public Vector3 saturation = new Vector3(1.0f, 1.0f, 1.0f);

    private Material colorCorrectMat;

    void OnEnable()
    {
        //Dither shader
        ditherMat = new Material(ditherShader);
        ditherMat.hideFlags = HideFlags.HideAndDontSave;

        //Color Correction shader
        colorCorrectMat = new Material(colorCorrectionShader);
        colorCorrectMat.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnDisable()
    {
        //Dither shader
        ditherMat = null;

        //Color Correction shader
        colorCorrectMat = null;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Dither
        ditherMat.SetFloat("_Spread", spread);
        ditherMat.SetInt("_RedColorCount", redColorCount);
        ditherMat.SetInt("_GreenColorCount", greenColorCount);
        ditherMat.SetInt("_BlueColorCount", blueColorCount);
        ditherMat.SetInt("_BayerLevel", bayerLevel);

        int width = source.width;
        int height = source.height;

        RenderTexture[] textures = new RenderTexture[8];

        RenderTexture currentSource = source;

        for (int i = 0; i < downSamples; ++i)
        {
            width /= 2;
            height /= 2;

            if (height < 2)
                break;

            RenderTexture currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, source.format);

            if (pointFilterDown)
                Graphics.Blit(currentSource, currentDestination, ditherMat, 1);
            else
                Graphics.Blit(currentSource, currentDestination);

            currentSource = currentDestination;
        }

        RenderTexture dither = RenderTexture.GetTemporary(width, height, 0, source.format);
        Graphics.Blit(currentSource, dither, ditherMat, 0);

        Graphics.Blit(dither, destination, ditherMat, 1);
        RenderTexture.ReleaseTemporary(dither);
        for (int i = 0; i < downSamples; ++i)
        {
            RenderTexture.ReleaseTemporary(textures[i]);
        }

        //Color Correction shader

        colorCorrectMat.SetVector("_Exposure", exposure);
        colorCorrectMat.SetVector("_Contrast", contrast);
        colorCorrectMat.SetVector("_MidPoint", linearMidPoint);
        colorCorrectMat.SetVector("_Brightness", brightness);
        colorCorrectMat.SetVector("_ColorFilter", colorFilter);
        colorCorrectMat.SetVector("_Saturation", saturation);
        colorCorrectMat.SetFloat("_Temperature", temperature / 100.0f);
        colorCorrectMat.SetFloat("_Tint", tint / 100.0f);

        Graphics.Blit(source, destination, colorCorrectMat);
    }
}
