using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "GameSettings")]
public class Settings : ScriptableObject
{
    public CustomerSettings[] customerTypes;
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
