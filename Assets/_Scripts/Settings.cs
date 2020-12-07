using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "GameSettings")]
public class Settings : ScriptableObject
{
    [Header("Customers")]
    public CustomerSettings[] customerTypes;
    [Header("Game Loop")]
    [Label("Levels Settings")]
    public Levels[] levels;
}

[System.Serializable]
public class CustomerSettings
{
    public string name;
    [MinValue(1)] [AllowNesting]
    [Tooltip("How many beers customer needs to drink before leaving")]
    public int thirst;
    [Space]
    [MinValue(0)] [AllowNesting]
    public float drinkDuration; //czas trwania animacji
    [MinValue(0)] [AllowNesting]
    [Tooltip("How long customer will wait between moving")]
    public float waitDuration = 0.5f;
    [MinValue(0)] [AllowNesting]
    public float moveSpeed = 10f;
    [MinValue(0)] [AllowNesting]
    public float moveDistance = 1f;
}
[System.Serializable]
public class Levels
{
    public string name;
    public float timeBetweenWave;
    public Wave[] wave;
    [System.Serializable]
    public class Wave
    {
        public string name;
        [Tooltip("Does wave ends when enough numbers of customers is served?")]
        public bool loopWave;
        [MinValue(1)] [AllowNesting][Tooltip("Number of all customer in wave")]
        public int numberOfCustomers;
        [Range(0,100)][Tooltip("Minimal chance for spawning at bar")]
        public int minBarChance;
        [Range(0,100)][Tooltip("Maximal chance for spawning at bar")]
        public int maxBarChance;
        [Range(0,100)][Tooltip("Minimal chance for spawning at bar increase after not spawning")]
        public int chanceIncrease;
        [MinValue(0)][AllowNesting][Tooltip("How many customers will spawn before spawn break. If set to 0 there will be not spawn limit")]
        public int spawnLimit;
        [MinValue(0)] [AllowNesting][Tooltip("How long game spawn customers before spawn break")]
        public float spawnDuration;
        [MinValue(0)] [AllowNesting][Tooltip("How long is spawn break")]
        public float breakDuration;
        [Tooltip("Sum of all the chances should be 100 and lenght of the array should be number of customer types")]
        public CustomerTypes[] customersTypes;
        [System.Serializable]
        public class CustomerTypes
        {
            public string name;
            [Range(0,100)][Label("Chance for spawn (%)")][AllowNesting]
            public int chanceForSpawn;
            public bool overridePrefab;
            public CustomerSettings customerSettings;
        }
    }
}