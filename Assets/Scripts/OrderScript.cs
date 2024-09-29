using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class OrderScript : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text orderText;
    public AudioManager audioManager;
    Color textColor;

    string displayText;

    // Food Type
    string tacoShell = "TacoShell";

    // Proteins
    string beef = "Beef";

    // Sauces
    string sourCream = "SourCream";

    // Topings
    string lettuce = "Lettuce";
    string cheese = "Cheese";
    string tomato = "Tomato";

    public List<GameObject> lights;
    public Light doorLight;

    List<string> proteins; 
    List<string> sauces; 
    List<string> order;
    List<List<string>> numberedOrders;

    List<string> regCrunchy; 
    List<string> supCrunchy; 

    void Awake()
    {
        textColor = orderText.color;

        proteins = new List<string> {beef};
        sauces = new List<string> {sourCream};

        regCrunchy = new List<string> {tacoShell, beef, lettuce, cheese};
        supCrunchy = new List<string> {tacoShell, beef, sourCream, lettuce, cheese, tomato};

        numberedOrders = new List<List<string>> {regCrunchy, supCrunchy};
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
        int orderNum = Random.Range(0, numberedOrders.Count);
        Debug.Log(orderNum);
        order = new List<string>(numberedOrders[orderNum]);
    }

    void UpdateOrderText()
    {
        orderText.color = textColor;
        displayText = "Order: \n";

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
            if (other.gameObject.tag == "WrappedTaco" && CheckWrappedContents(other.gameObject)) // or wrapped burrito
            {
                orderText.text = "It enjoys it!";
                Destroy(other.gameObject);
                StartCoroutine(WaitAndExecute(2f));
                return;
            }
            other.GetComponent<Rigidbody>().AddForce(-transform.right * 20, ForceMode.Impulse);
            orderText.color = Color.red;
            StartCoroutine(TemporarilyChangeText("no"));
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
