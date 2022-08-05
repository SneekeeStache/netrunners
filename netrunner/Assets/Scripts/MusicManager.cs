using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
using System.Runtime.InteropServices;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField]
    private EventReference music ;
    public TimelineInfo timelineInfo = null;
    public  GCHandle timelineHandle;
    private FMOD.Studio.EventInstance musicInstance;

    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo{
        public int currentBeat =0;
        public float currentTempo = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    private void Awake() {
        instance = this;
            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();
    }

    void Start()
    {
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(beatEventCallback);
        timelineHandle = GCHandle.Alloc(timelineInfo,GCHandleType.Pinned);
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy() {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        timelineHandle.Free();
    }

    private void OnGUI() {
        
    }


    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT beatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type,IntPtr instancePtr, IntPtr parameterPtr){
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if(result != FMOD.RESULT.OK){
            Debug.LogError("Timeline Callback error: "+ result);
        }else if(timelineInfoPtr != IntPtr.Zero){
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;
            switch(type){
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr,typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    timelineInfo.currentBeat = parameter.beat;
                    timelineInfo.currentTempo = parameter.tempo;
                }
                break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr,typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.lastMarker = parameter.name;
                }
                break;
            }
        }
        return FMOD.RESULT.OK;
    }
    public void ChangeParameter(string id,float parameters){
        musicInstance.setParameterByName(id,parameters);
        Debug.Log(id +" - "+parameters);
    }
}
