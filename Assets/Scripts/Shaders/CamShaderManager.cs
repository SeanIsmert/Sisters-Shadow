using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShaderManager : MonoBehaviour
{
    public Ditherer dither;
    public ColorCorrection colorCor;
    public Tonemapper tonemapper;
    public ChromaticAberration chrom;
    public Bloom bloom;


    void Start()
    {
        dither = GetComponent<Ditherer>();
        colorCor = GetComponent<ColorCorrection>();
        tonemapper = GetComponent<Tonemapper>();
        chrom = GetComponent<ChromaticAberration>();
        bloom = GetComponent<Bloom>();
    }

    public void UpdateValues(ShaderValues values)
    {
        //Dither settings
    }
}
