using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject sensitivitySlider;
    [SerializeField] private GameObject sensitivityText;

    private FpsCamera camera;


    void Start()
    {
        camera = FindObjectOfType<FpsCamera>();
        sensitivitySlider.GetComponent<Slider>().value = camera.mouseSensitivity/100;
    }

    // Update is called once per frame
    void Update()
    {
        camera.mouseSensitivity = sensitivitySlider.GetComponent<Slider>().value * 100;
        sensitivityText.GetComponent<Text>().text = System.Math.Round(sensitivitySlider.GetComponent<Slider>().value, 2).ToString();
    }

    public void Back()
    {
        gameObject.SetActive(false);
        PauseMenu.SetActive(true);
    }
}
