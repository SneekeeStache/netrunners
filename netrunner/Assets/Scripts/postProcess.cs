using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class postProcess : MonoBehaviour
{
    [Header("Lens Distortion")]
    
    [SerializeField] float lensDistortionIntensityDefaultValue = 0;
    [SerializeField] float lensDistrotionIntensityRunning = -40;
    [SerializeField] float lensDistrotionIntensityDash = -80;
    [SerializeField] float DistortionChangeSpeed=0.1f;
    [SerializeField] float DistortionChangeSpeedDash=2;
    float currentSpeedDistortion;
    float currentDistortionIntensity;

    [Header("Depth Of Field")]
    [SerializeField] float depthOfFieldApertureRunning = 3.2f;
    [SerializeField] float depthOfFieldApertureDash = 0.1f;
    [SerializeField] float depthOfFieldApertureDefaultValue = 15;
    [SerializeField] float apertureChangeSpeed=0.1f;
    [SerializeField] float apertureChangeSpeedDash=2;
    float currentSpeedAperture;
    float currentAperture;
        [Header("Focus")]
    [SerializeField] float focusDistanceDefault=1;
    [SerializeField] float focusDistanceDash=1;
    [SerializeField] float focusDistanceRunning=1;
    [SerializeField] float focusChangeSpeed=0.1f;
    [SerializeField] float focusChangeSpeedDash=2;
    float currentFocus;
    float CurrentFocusSpeed;
        [Header("Focal lenght")]
    [SerializeField] float FocalLengthDefault=1;
    [SerializeField] float FocalLengthDash=1;
    [SerializeField] float FocalLengthRunning=1;
    [SerializeField] float FocalLengthChangeSpeed=0.1f;
    [SerializeField] float FocalLengthChangeSpeedDash=2;
    
    float currentFocalLength;
    float CurrentFocalLengthSpeed;

    [SerializeField] PostProcessProfile postProcessGet;
    DepthOfField DOF;
    LensDistortion lensDistrotion;

    [SerializeField] metronome metronomeScript;
    // Start is called before the first frame update
    void Start()
    {
        if (postProcessGet.TryGetSettings<DepthOfField>(out DOF))
        {
            DOF.aperture.value = depthOfFieldApertureDefaultValue;
            DOF.focalLength.value=currentFocalLength;
            DOF.focusDistance.value=currentFocus;

            currentAperture = depthOfFieldApertureDefaultValue;
            currentFocalLength=FocalLengthDefault;
            currentFocus=focusDistanceDefault;
            
        }
        else
        {
            Debug.Log("can't get Depth Of Field");
        }

        if (postProcessGet.TryGetSettings<LensDistortion>(out lensDistrotion))
        {
            lensDistrotion.intensity.value = lensDistortionIntensityDefaultValue;
            currentDistortionIntensity = lensDistortionIntensityDefaultValue;
        }
        else
        {
            Debug.Log("can't get Lens Distortion");
        }
    }

    // Update is called once per frame
    void Update()
    {
        lensDistrotion.intensity.value = Mathf.Lerp(lensDistrotion.intensity.value,currentDistortionIntensity,currentSpeedDistortion);
        DOF.aperture.value = Mathf.Lerp(DOF.aperture.value,currentAperture,currentSpeedAperture);
        DOF.focalLength.value= Mathf.Lerp(DOF.focalLength.value,currentFocalLength,CurrentFocalLengthSpeed);
        DOF.focusDistance.value= Mathf.Lerp(DOF.focusDistance.value,currentFocus,CurrentFocusSpeed);


        if (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Vertical") > 0)
        {
            currentAperture=depthOfFieldApertureRunning;
            currentDistortionIntensity= lensDistrotionIntensityRunning;
            currentFocalLength=FocalLengthRunning;
            currentFocus=focusDistanceRunning;
            
        }else{
            currentAperture=depthOfFieldApertureDefaultValue;
            currentDistortionIntensity= lensDistortionIntensityDefaultValue;
            currentFocalLength=FocalLengthDefault;
            currentFocus=focusDistanceDefault;
        }

        if(Input.GetButtonDown("Dash")&& metronomeScript.onTempo){
            currentSpeedDistortion=DistortionChangeSpeedDash;
            currentDistortionIntensity = lensDistrotionIntensityDash;

            currentAperture=depthOfFieldApertureDash;
            currentSpeedAperture= apertureChangeSpeedDash;

            currentFocalLength=FocalLengthDash;
            CurrentFocalLengthSpeed=FocalLengthChangeSpeedDash;

            currentFocus=focusDistanceDash;
            CurrentFocusSpeed=focusChangeSpeedDash;

        }else{
            currentSpeedDistortion=DistortionChangeSpeed;
            currentSpeedAperture= apertureChangeSpeed;

            CurrentFocalLengthSpeed=FocalLengthChangeSpeed;
            CurrentFocusSpeed= focusChangeSpeed;
        }
    }
}
