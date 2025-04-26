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
    private List<Sprite> styleSprites = new List<Sprite>();

    [SerializeField] private float styleAmount;
    private int currentStyle;
    
    public UnityEvent<float> AddStyleEvent;

    private int lastWeapon;
    private int sameWeapontAmount;

    private float multiplier = 10;

    private float percentageToNext;
    [SerializeField] private Sprite F;
    [SerializeField] private Sprite E;
    [SerializeField] private Sprite D;
    [SerializeField] private Sprite C;
    [SerializeField] private Sprite B;
    [SerializeField] private Sprite A;
    [SerializeField] private Sprite S;
    [SerializeField] private Sprite SS;
    [SerializeField] private Sprite SSS;

    [SerializeField] private GameObject meter;
    [SerializeField] private GameObject rank;
    [SerializeField] private GameObject rankImage;
    [SerializeField] private GameObject canvas;
    private Vector3 imageScale;
    
    [SerializeField] private GameObject pointsPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SetStyles();
        currentStyle = 0;
        AddStyleEvent.AddListener(AddStyle);
        imageScale = rankImage.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        StyleReduction();
    }

    void SetStyles()
    {
        styles.Add("F", 1);
        styleSprites.Add(F);
        styleNames.Add("F");
        stylesColor.Add("F", Color.grey);

        styles.Add("E", 100);
        styleSprites.Add(E);
        styleNames.Add("E");
        stylesColor.Add("E", Color.white);

        styles.Add("D", 300);
        styleSprites.Add(D);
        styleNames.Add("D");
        stylesColor.Add("D", Color.cyan);

        styles.Add("C", 500);
        styleSprites.Add(C);
        styleNames.Add("C");
        stylesColor.Add("C", Color.blue);

        styles.Add("B", 800);
        styleSprites.Add(B);
        styleNames.Add("B");
        stylesColor.Add("B", new Color32(252, 77, 27, 255));

        styles.Add("A", 1100);
        styleSprites.Add(A);
        styleNames.Add("A");
        stylesColor.Add("A", new Color32(252, 183, 27, 255));

        styles.Add("S", 1500);
        styleSprites.Add(S);
        styleNames.Add("S");
        stylesColor.Add("S", Color.yellow);

        styles.Add("SS", 2000);
        styleSprites.Add(SS);
        styleNames.Add("SS");
        stylesColor.Add("SS", Color.yellow);

        styles.Add("SSS", 3000);
        styleSprites.Add(SSS);
        styleNames.Add("SSS");
        stylesColor.Add("SSS", Color.yellow);
    }

    void CheckCurrentStyle()
    {
        if (currentStyle < styles.Count-1)
        {
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
            percentageToNext = (styleAmount - styles[styleNames[currentStyle]]) / (styles[styleNames[currentStyle + 1]] - styles[styleNames[currentStyle]]);
        }
        else
        {
            percentageToNext = (styleAmount - styles[styleNames[currentStyle]]) / (styles[styleNames[currentStyle]] * 3) ;
            if (styles[styleNames[currentStyle]] > styleAmount)
            {
                currentStyle--;
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
            finalAmount *= GetComponent<MovementComponent>().playerSpeed.magnitude/5;
        }
        
        //animation
        LeanTween.rotateZ(rankImage, 10, 0.1f).setLoopPingPong(1).setOnComplete(()=>rankImage.transform.rotation = Quaternion.Euler(0, 0, 0));
        LeanTween.scale(rankImage, new Vector3(1.5f, 1.5f, 1.5f), 0.1f).setLoopPingPong(1).setOnComplete(()=>rankImage.transform.localScale = imageScale);
        
        GameObject points = Instantiate(pointsPrefab, meter.transform.position + new Vector3(0, 5, 0), Quaternion.identity, canvas.transform);
        points.GetComponent<Text>().text = $"+{Mathf.Round(finalAmount)}";
        
        styleAmount += finalAmount;
        CheckCurrentStyle();
    }

    void SetUI()
    {
        // rank.GetComponent<Text>().text = styleNames[currentStyle];
        // rank.GetComponent<Text>().color = stylesColor[styleNames[currentStyle]];
        rankImage.GetComponent<Image>().sprite = styleSprites[currentStyle];
        meter.gameObject.GetComponent<Image>().fillAmount = percentageToNext;
        meter.gameObject.GetComponent<Image>().color = stylesColor[styleNames[currentStyle]];
    }

    void StyleReduction()
    {
        styleAmount -= 5 * currentStyle * Time.deltaTime;
        if (styleAmount < 0)
        {
            styleAmount = 0;
        }
        CheckCurrentStyle();
    }
}
