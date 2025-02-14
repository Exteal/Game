using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BitalinoManager : MonoBehaviour
{
    private PluxDeviceManager pluxDevManager;
    private string devAdress;
    private int samplingRate = 100;

    public static BitalinoManager instance;
    public List<int> stressLogs;
    public bool connected = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnStart()
    {
        pluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone, AcquisitionStarted, OnDataReceived, OnEventDetected, OnExceptionRaised);
        pluxDevManager.WelcomeFunctionUnity();

        pluxDevManager.GetDetectableDevicesUnity(new List<string>() { "BTH", "BLE" });

    }
    void Start()
    {
        OnStart();
    }

    void OnApplicationQuit()
    {
        pluxDevManager.DisconnectPluxDev();
    }
    public void ScanResults(List<string> listDevices)
    {
        Debug.Log("Results : " + listDevices.Count);
        if (listDevices.Count > 0)
        {
            devAdress = listDevices[0];
            pluxDevManager.PluxDev(devAdress);
        }

        else
        {
            if(!connected)
            {
                //      OnStart();
            }
        }

        
    }


    public void ConnectionDone(bool connectionStatus)
    {
        
        if (connectionStatus)
        {
            pluxDevManager.StartAcquisitionUnity(samplingRate, new List<int>{1}, 16);
            connected = true;
        }

        else
        {
            if (!connected)
            {
                //    OnStart();
            }
            else
            {
                connected = false;
            }
        }
    }

    // Callback invoked once the data streaming between the PLUX device and the computer is started.
    // acquisitionStatus -> A boolean flag stating if the acquisition was started with success (true) or not (false).
    // exceptionRaised -> A boolean flag that identifies if an exception was raised and should be presented in the GUI (true) or not (false).
    public void AcquisitionStarted(bool acquisitionStatus, bool exceptionRaised = false, string exceptionMessage = "")
    {
        if (acquisitionStatus)
        {
            return;
        }
        else
        {
            if (!connected)
            {
              //  OnStart();
            }
        }
        
    }

    // Callback invoked every time an exception is raised in the PLUX API Plugin.
    // exceptionCode -> ID number of the exception to be raised.
    // exceptionDescription -> Descriptive message about the exception.
    public void OnExceptionRaised(int exceptionCode, string exceptionDescription)
    {
        if (pluxDevManager.IsAcquisitionInProgress())
        {
            Debug.Log("Exception during acq");
        }
    }

    // Callback that receives the data acquired from the PLUX devices that are streaming real-time data.
    // nSeq -> Number of sequence identifying the number of the current package of data.
    // data -> Package of data containing the RAW data samples collected from each active channel ([sample_first_active_channel, sample_second_active_channel,...]).
    public void OnDataReceived(int nSeq, int[] data)
    {
        // Show samples with a 1s interval.
        if (nSeq % samplingRate == 0)
        {
            // Show the current package of data.
            for (int j = 0; j < data.Length; j++)
            {
                string outputString = "Acquired Data: ";

                outputString += data[j] + "\t";

                stressLogs.Add(data[j]);
                Debug.Log(outputString);

            }
        }
    }

    public void OnEventDetected(PluxDeviceManager.PluxEvent pluxEvent)
    {
        if (pluxEvent is PluxDeviceManager.PluxDisconnectEvent)
        {

            Debug.Log("Disconnected");
        }
        else if (pluxEvent is PluxDeviceManager.PluxDigInUpdateEvent)
        {
            PluxDeviceManager.PluxDigInUpdateEvent digInEvent = (pluxEvent as PluxDeviceManager.PluxDigInUpdateEvent);
            Debug.Log("Digital Input Update Event Detected on channel " + digInEvent.channel + ". Current state: " + digInEvent.state);
        }
    }
}
