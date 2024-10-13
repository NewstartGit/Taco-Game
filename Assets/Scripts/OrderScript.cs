using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class OrderScript : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text orderText;
    public AudioManager audioManager;
    Color textColor;

    string displayText;

    // Food Type
    string tacoShell = "TacoShell";
    string tortilla = "Tortilla";

    // Hot stuff
    string bean = "Bean";

    // Proteins
    string beef = "Beef";

    // Sauces
    string redSauce = "RedSauce";
    string sourCream = "SourCream";

    // Topings
    string lettuce = "Lettuce";
    string cheese = "Cheese";
    string tomato = "Tomato";
    string onion = "Onion";

    public List<GameObject> lights;
    public Light doorLight;

    List<string> proteins; 
    List<string> sauces; 
    List<string> order;
    List<List<string>> numberedOrders;

    List<string> regCrunchy; 
    List<string> supCrunchy; 
    List<string> supBurrito;
    List<string> beanBurrito;

    int orderNum;

    void Awake()
    {
        textColor = orderText.color;

        proteins = new List<string> {beef};
        sauces = new List<string> {sourCream};

        regCrunchy = new List<string> {tacoShell, beef, lettuce, cheese};
        supCrunchy = new List<string> {tacoShell, beef, sourCream, lettuce, cheese, tomato};
        supBurrito = new List<string> {tortilla, beef, bean, redSauce, sourCream, onion, lettuce, cheese, tomato};
        beanBurrito = new List<string> {tortilla, bean, redSauce, onion, cheese};

        numberedOrders = new List<List<string>> {regCrunchy, supCrunchy, supBurrito, beanBurrito};
    }

    void Start()
    {
        //RunNextCycle();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.O))
        {
            RunNextCycle();
        }
        
    }

    void RunNextCycle()
    {
        StopAllCoroutines();
        StartCoroutine(TimeLimit());
        PickNumberedOrder();
        UpdateOrderText();
    }

    void PickNumberedOrder()
    {
        orderNum = Random.Range(0, numberedOrders.Count);
        Debug.Log(orderNum);
        order = new List<string>(numberedOrders[orderNum]);
    }

    void UpdateOrderText()
    {
        String orderName;
        switch(orderNum)
        {
            case 0:
                orderName = "Crunchy Taco";
                break;
            case 1:
                orderName = "Crunchy Taco Supreme";
                break;
            case 2:
                orderName = "Supreme Burrito";
                break;
            case 3:
                orderName = "Bean Burrito";
                break;
            default:
                orderName = "Custom";
                break;
        }

        orderText.color = textColor;
        displayText = "Order: " + orderName + "\n";

        foreach (string ingredient in order)
        {
            displayText += ingredient + "\n";
        }
        
        orderText.text = displayText;
    }

    void CreateOrder()
    {
        // Protein
        string protein = proteins[Random.Range(0, proteins.Count-1)];
        order.Add(protein);

        //Sauces
    }

    bool CheckWrappedContents(GameObject wrappedItem)
    {
        WrapperContent script = wrappedItem.GetComponent<WrapperContent>();
        List<string> wrappedIngredients = new List<string> {script.foodType};

        foreach (string ingredient in script.ingredients)
        {
            wrappedIngredients.Add(ingredient);
        }
        
        if (order.SequenceEqual(wrappedIngredients))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.transform.parent == null && other.gameObject.layer == 6)
        {
            if ((other.gameObject.tag == "WrappedTaco" || other.gameObject.tag == "WrappedBurrito") && CheckWrappedContents(other.gameObject)) // or wrapped burrito
            {
                orderText.text = "It enjoys it!";
                audioManager.Order(true);
                Destroy(other.gameObject);
                StartCoroutine(WaitAndExecute(2f));
                return;
            }
            other.GetComponent<Rigidbody>().AddForce(-transform.right * 20, ForceMode.Impulse);
            orderText.color = Color.red;
            StartCoroutine(TemporarilyChangeText("no"));
            audioManager.Order(false);
        }
    }

    public void WarnPlayer()
    {
        orderText.color = Color.red;
        StartCoroutine(TemporarilyChangeText("hurry"));
    }

    public void GameOver()
    {
        orderText.color = Color.red;
        orderText.text = "goodbye";
        foreach (GameObject light in lights)
        {
            Destroy(light);
        }
        doorLight.color = new Color(0.3f, 0f, 0f); // Dark red
        audioManager.StopAllMusic();
        StartCoroutine(CloseGame(10f));
    }

    IEnumerator WaitAndExecute(float delay)
    {
        yield return new WaitForSeconds(delay);
        RunNextCycle();
    }
    IEnumerator CloseGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioManager.Scream();
        yield return new WaitForSeconds(2f);
        EditorApplication.isPlaying = false;
    }

    IEnumerator TimeLimit()
    {
        yield return new WaitForSeconds(timeLimit - 10);
        WarnPlayer();
        yield return new WaitForSeconds(timeLimit);
        GameOver();
    }

    IEnumerator TemporarilyChangeText(String text)
    {
        orderText.text = text;
        yield return new WaitForSeconds(2f);
        orderText.color = textColor;
        orderText.text = displayText;
    }
}
