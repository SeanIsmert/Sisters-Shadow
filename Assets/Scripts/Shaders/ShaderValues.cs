using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ShaderValues : ScriptableObject
{
    [Header("Dither settings")]
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
    /*
    [Range(0, 8)]
    public int ditherDownSamples = 0;

    public bool pointFilterDown = false;
    */
    [Header("Color Correction settings")]
    public Vector3 exposure = new Vector3(1.0f, 1.0f, 1.0f);

    [Range(-100.0f, 100.0f)]
    public float temperature, tint;

    public Vector3 contrast = new Vector3(1.0f, 1.0f, 1.0f);

    public Vector3 linearMidPoint = new Vector3(0.5f, 0.5f, 0.5f);

    public Vector3 brightness = new Vector3(0.0f, 0.0f, 0.0f);

    [ColorUsageAttribute(false, true)]
    public Color colorFilter;

    public Vector3 saturation = new Vector3(1.0f, 1.0f, 1.0f);
    /*
    [Header("Chromatic Abberation settings")]
    public bool debugMask = false;

    public Vector2 focalOffset = new Vector2(0.0f, 0.0f);
    public Vector2 radius = new Vector2(1.0f, 1.0f);

    [Range(0.01f, 5.0f)]
    public float hardness = 1.0f;
    [Range(0.01f, 5.0f)]
    public float intensity = 1.0f;

    public Vector3 channelOffsets = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Bloom")]
    [Range(0.0f, 10.0f)]
    public float threshold = 1.0f;

    [Range(0.0f, 1.0f)]
    public float softThreshold = 0.5f;

    [Range(1, 16)]
    public int bloomDownSamples = 1;

    [Range(0.01f, 2.0f)]
    public float downSampleDelta = 1.0f;

    [Range(0.01f, 2.0f)]
    public float upSampleDelta = 0.5f;

    [Range(0.0f, 10.0f)]
    public float bloomIntensity = 1;
    */
}
