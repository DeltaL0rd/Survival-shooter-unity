using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadSystem : MonoBehaviour
{
    public SaveData saveData;
    [Serializable]
    public class SaveData
    {
        public int score;
        public int playerHealth;
        public Vector3 playerPosition;
        public float playerRotation;
        public List<EnemyData> enemies;
    }
    [Serializable]
    public class EnemyData
    {
        public int enemyTypeIndex;
        public int enemyHealth;
        public Vector3 enemyPosition;
        public float enemyRotation;
    }

    private EnemyData _enemyData;

    [SerializeField]private PlayerHealth playerHealth;
    [SerializeField]private GameObject player;
    [SerializeField] private GameObject[] enemyprefabs;

    public void SaveGame()
    {
        // Clear Any previous Data in the List
        saveData.enemies.Clear();
        // Save Score
        saveData.score = ScoreManager.score;
        //Save Player Health
        saveData.playerHealth = playerHealth.CurrentHealth;
        //Save Player Position
        saveData.playerPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        //Save Player Rotation
        saveData.playerRotation = player.transform.localRotation.y;
        
        // Getting the Current Enemies in the Scene
        var enemies = FindObjectsOfType<EnemyHealth>();
        for (int i = 0; i < enemies.Length; i++)
        {
            // Getting all the required Data For a particular Enemy
            _enemyData = new EnemyData();
            _enemyData.enemyTypeIndex = enemies[i].enemyTypeIndex;
            _enemyData.enemyHealth = enemies[i].CurrentHealth;
            _enemyData.enemyPosition = enemies[i].transform.localPosition;
            _enemyData.enemyRotation = enemies[i].transform.localRotation.y;
            // Save the Enemy Data
            saveData.enemies.Add(_enemyData);
        }

        // Serialize the Save Data Class In Json
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(saveData);
        //Save the Json File in the Playerprefs
        PlayerPrefs.SetString("SavedData",json);
    }

    public void LoadGame()
    {
        // First Off all Lets Clear the Current Enemies from the Scene
        var enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        
        // Deserialized the JSON File in the Save Data Class
        saveData = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveData>(PlayerPrefs.GetString("SavedData"));
        
        // Set the Score
        ScoreManager.score = saveData.score;
        // Set the Player Health
        playerHealth.CurrentHealth = saveData.playerHealth;
        // Set the Player Health Slider 
        playerHealth.healthSlider.value = playerHealth.CurrentHealth;
        // Set the Player Position
        player.transform.localPosition = saveData.playerPosition;
        // Set the Player Rotation
        player.transform.localRotation = new Quaternion(0f, saveData.playerRotation, 0f, 1f);

      
        for (int i = 0; i < saveData.enemies.Count; i++)
        {
            // Lets Spawn the Exect same Enemies that were in the Scene when game saved
            GameObject currentEnemy =  Instantiate (enemyprefabs[saveData.enemies[i].enemyTypeIndex], saveData.enemies[i].enemyPosition, new Quaternion(0f,saveData.enemies[i].enemyRotation,0f,1f));
            // setting up the Enemies Health
           currentEnemy.GetComponent<EnemyHealth>().CurrentHealth = saveData.enemies[i].enemyHealth;
           // Set the Enemies Type Index for future Save
           currentEnemy.GetComponent<EnemyHealth>().enemyTypeIndex = saveData.enemies[i].enemyTypeIndex;
        }
    }
}
