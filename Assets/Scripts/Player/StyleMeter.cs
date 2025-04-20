using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StyleMeter : MonoBehaviour
{
    private Dictionary<string, float> styles = new Dictionary<string, float>(); 
    private Dictionary<string, Color> stylesColor = new Dictionary<string, Color>(); 
    private List<string> styleNames = new List<string>();

    [SerializeField] private float styleAmount;
    private int currentStyle;
    
    public UnityEvent<float> AddStyleEvent;

    private int lastWeapon;
    private int sameWeapontAmount;

    private float multiplier = 10;

    private float percentageToNext;

    [SerializeField] private GameObject meter;
    [SerializeField] private GameObject rank;
    // Start is called before the first frame update
    void Start()
    {
        SetStyles();
        currentStyle = 0;
        AddStyleEvent.AddListener(AddStyle);
    }

    // Update is called once per frame
    void Update()
    {
        StyleReduction();
    }

    void SetStyles()
    {
        styles.Add("F", 1);
        styleNames.Add("F");
        stylesColor.Add("F", Color.grey);
        styles.Add("E", 100);
        styleNames.Add("E");
        stylesColor.Add("E", Color.white);
        styles.Add("D", 300);
        styleNames.Add("D");
        stylesColor.Add("D", Color.cyan);
        styles.Add("C", 500);
        styleNames.Add("C");
        stylesColor.Add("C", Color.blue);
        styles.Add("B", 800);
        styleNames.Add("B");
        stylesColor.Add("B", new Color32(252, 77, 27, 255));
        styles.Add("A", 1100);
        styleNames.Add("A");
        stylesColor.Add("A", new Color32(252, 183, 27, 255));
        styles.Add("S", 1500);
        styleNames.Add("S");
        stylesColor.Add("S", Color.yellow);
        styles.Add("SS", 2000);
        styleNames.Add("SS");
        stylesColor.Add("SS", Color.yellow);
        styles.Add("SSS", 3000);
        styleNames.Add("SSS");
        stylesColor.Add("SSS", Color.yellow);
    }

    void CheckCurrentStyle()
    {
        if (currentStyle < styles.Count)
        {
            percentageToNext = (styleAmount - styles[styleNames[currentStyle]]) / styles[styleNames[currentStyle + 1]];
            if (styles[styleNames[currentStyle + 1]] < styleAmount)
            {
                currentStyle++;
                percentageToNext = 0;
            }
            else if (currentStyle > 0)
            {
                if (styles[styleNames[currentStyle]] > styleAmount)
                {
                    currentStyle--;
                }
            }
        }
        SetUI();
    }

    void AddStyle(float amount)
    {
        float finalAmount = amount;
        if (lastWeapon == GetComponent<WeaponSelector>().swapIndex)
        {
            sameWeapontAmount = 0;
            finalAmount *= multiplier;
        }
        else
        {
            sameWeapontAmount++;
            lastWeapon = GetComponent<WeaponSelector>().swapIndex;
            finalAmount *= multiplier * Mathf.Sqrt(sameWeapontAmount);
        }

        if (GetComponent<MovementComponent>().playerSpeed.magnitude > 5)
        {
            finalAmount *= GetComponent<MovementComponent>().playerSpeed.magnitude/10;
        }
        
        styleAmount += finalAmount;
        CheckCurrentStyle();
    }

    void SetUI()
    {
        rank.GetComponent<Text>().text = styleNames[currentStyle];
        rank.GetComponent<Text>().color = stylesColor[styleNames[currentStyle]];
        meter.gameObject.GetComponent<Image>().fillAmount = percentageToNext;
        meter.gameObject.GetComponent<Image>().color = stylesColor[styleNames[currentStyle]];
    }

    void StyleReduction()
    {
        styleAmount -= 5 * Time.deltaTime;
        CheckCurrentStyle();
    }
}
