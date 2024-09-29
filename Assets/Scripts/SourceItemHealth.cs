using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceItemHP : MonoBehaviour
{
    public bool isLargePan;
    public float maxHealth; // Maximum health value
    public float currentHealth; // Current health value

    void Start()
    {
        if (isLargePan)
        {
            maxHealth = 20;
        }
        else
        {
            maxHealth = 10;
        }
        // Initialize current health to max health at the start
        currentHealth = maxHealth;
    }

    // Method to deal damage
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to shrink the source object
    public void ShrinkObject()
    {
        // Shrink the object by the shrink factor
        //Vector3 newScale = transform.localScale * shrinkFactor;
        //transform.localScale = newScale;
        TakeDamage(1f);
    }

    public void MoveDown()
    {
        transform.position += new Vector3(0, -0.5f/maxHealth, 0);
        TakeDamage(1f);
    }

    public void MoveUp()
    {
        transform.position += new Vector3(0, 0.05f, 0);
        Heal(1f);
    }

    // Method to heal the object
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Cap health at max health
        }
    }

    // Method called when health reaches zero
    private void Die()
    {
        // Additional logic for when the object dies can be added here
        Destroy(gameObject); // For example, destroy the object
    }
}