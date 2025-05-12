using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject sensitivitySliderGameObject;
    private Slider sensitivitySlider;
    [SerializeField] private GameObject sensitivityTextGameObject;
    private Text sensitivityText;

    private FpsCamera camera;


    void Start()
    {
        camera = FindObjectOfType<FpsCamera>();
        sensitivitySlider = sensitivitySliderGameObject.GetComponent<Slider>();
        sensitivityText = sensitivityTextGameObject.GetComponent<Text>();
        sensitivitySlider.value = camera.mouseSensitivity/100;
    }

    // Update is called once per frame
    void Update()
    {
        camera.mouseSensitivity = sensitivitySlider.value * 100;
        sensitivityText.text = System.Math.Round(sensitivitySlider.value, 2).ToString();
    }

    public void Back()
    {
        gameObject.SetActive(false);
        PauseMenu.SetActive(true);
    }
}
