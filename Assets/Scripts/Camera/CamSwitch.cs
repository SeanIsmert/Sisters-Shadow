using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    //public Transform player;
    
    [SerializeField] private CinemachineVirtualCamera vm;
    [SerializeField] private ShaderValues sv;
    private Camera mainCam;
    private Camera minimapCam;

    [Header("Minimap Settings")]
    [SerializeField] private float minimapSize =5f;
    [SerializeField] private Transform minimapCamPos;

    [Header("Debug Settings")]
    [SerializeField] private bool ConstUpdate = false;

    private void Start()
    {
        mainCam = Camera.main;
        minimapCam = GameObject.FindGameObjectWithTag("MinimapCam").GetComponent<Camera>();
        vm.Priority = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("player");
            vm.Priority = 10;
            minimapCam.gameObject.transform.position = minimapCamPos.position;
            minimapCam.orthographicSize = minimapSize;
            updateCamShaders();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vm.Priority = 0;
        }
    }

    private void updateCamShaders()
    {
        if (sv == null) return;
        var dither = mainCam.GetComponent<Ditherer>();
        var colorCorrect = mainCam.GetComponent<ColorCorrection>();
        var chromAb = mainCam.GetComponent<ChromaticAberration>();
        var bloom = mainCam.GetComponent<Bloom>();

        dither.spread = sv.spread;
        dither.redColorCount = sv.redColorCount;
        dither.greenColorCount = sv.greenColorCount;
        dither.blueColorCount = sv.blueColorCount;
        dither.bayerLevel = sv.bayerLevel;
        //dither.pointFilterDown = sv.pointFilterDown;

        colorCorrect.exposure = sv.exposure;
        colorCorrect.temperature = sv.temperature;
        colorCorrect.tint = sv.tint;
        colorCorrect.contrast = sv.contrast;
        colorCorrect.linearMidPoint = sv.linearMidPoint;
        colorCorrect.brightness = sv.brightness;
        colorCorrect.colorFilter = sv.colorFilter;
        colorCorrect.saturation = sv.saturation;
    }

    private void Update()
    {
        if(!ConstUpdate) return;
        if (vm.Priority != 10) return;
        updateCamShaders();
    }
}
