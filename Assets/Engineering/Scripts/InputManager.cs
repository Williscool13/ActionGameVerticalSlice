using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // save the timestamps of the players inputs for all the buttons
    private Dictionary<string, float> buttonDownTimes = new Dictionary<string, float>();
    private Dictionary<string, float> buttonUpTimes = new Dictionary<string, float>();
}
