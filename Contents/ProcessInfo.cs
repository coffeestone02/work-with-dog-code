using UnityEngine;
using System.Diagnostics;

public class ProcessInfo : MonoBehaviour
{
    public Process process;

    private void OnDestroy()
    {
        process?.Dispose();
        process = null;
    }
}
