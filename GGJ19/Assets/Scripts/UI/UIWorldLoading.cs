using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldLoading : MonoBehaviour
{
    [SerializeField]
    private FloatVariable _loadingProgress;
    [SerializeField]
    private Slider _loadingSlider;
    [SerializeField]
    private GameObject _loadingScreen;

    private void Update()
    {
        _loadingSlider.value = _loadingProgress.Value;
    }

    public void WorldReady()
    {
        _loadingScreen.SetActive(false);
    }
}
