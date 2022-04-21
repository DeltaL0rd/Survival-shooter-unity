using System;
using System.Collections;
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
    [SerializeField] private List<EnemyHealth> enemies;

    public void SaveGame()
    {
        saveData.enemies.Clear();
        saveData.score = ScoreManager.score;
        saveData.playerHealth = playerHealth.CurrentHealth;
        saveData.playerPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        saveData.playerRotation = player.transform.localRotation.y;
        
        var enemies = FindObjectsOfType<EnemyHealth>();
        for (int i = 0; i < enemies.Length; i++)
        {
            _enemyData = new EnemyData();
            _enemyData.enemyTypeIndex = enemies[i].enemyTypeIndex;
            _enemyData.enemyHealth = enemies[i].currentHealth;
            _enemyData.enemyPosition = enemies[i].transform.localPosition;
            _enemyData.enemyRotation = enemies[i].transform.localRotation.y;
            saveData.enemies.Add(_enemyData);
        }

        //string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(saveData);
        //Debug.Log(jsonText);
        #region PlayerPrefs 

        //If you want to save these data in PlayerPrefs
        /*PlayerPrefs.SetInt("LastScore",ScoreManager.score);
        //Player Health
        PlayerPrefs.SetInt("PlayerHealth",playerHealth.CurrentHealth);
        //Player Position-Rotation
        PlayerPrefs.SetFloat("PXP",player.transform.localPosition.x);
        PlayerPrefs.SetFloat("PYP",player.transform.localPosition.y);
        PlayerPrefs.SetFloat("PZP",player.transform.localPosition.z);
        PlayerPrefs.SetFloat("PYR",player.transform.localRotation.y);
        
        //Set EnemiesData
        var enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.enemyTypeIndex);
        }*/
        #endregion
    }

    public void LoadGame()
    {
        ScoreManager.score = saveData.score;
        playerHealth.CurrentHealth = saveData.playerHealth;
        playerHealth.healthSlider.value = playerHealth.CurrentHealth;
        player.transform.localPosition = saveData.playerPosition;
        player.transform.localRotation = new Quaternion(0f, saveData.playerRotation, 0f, 1f);

        for (int i = 0; i < saveData.enemies.Count; i++)
        {
            
           GameObject currentEnemy =  Instantiate (enemyprefabs[saveData.enemies[i].enemyTypeIndex], saveData.enemies[i].enemyPosition, new Quaternion(0f,saveData.enemies[i].enemyRotation,0f,1f));
           currentEnemy.GetComponent<EnemyHealth>().currentHealth = saveData.enemies[i].enemyHealth;
           currentEnemy.GetComponent<EnemyHealth>().enemyTypeIndex = saveData.enemies[i].enemyTypeIndex;
        }
        #region PlayerPrefs

        /*// Score
        ScoreManager.score = PlayerPrefs.GetInt("LastScore");
        //Player Health
        playerHealth.CurrentHealth = PlayerPrefs.GetInt("PlayerHealth");
        playerHealth.healthSlider.value = playerHealth.CurrentHealth;
        //Player Position-Rotation
        player.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("PXP"),PlayerPrefs.GetFloat("PYP"),PlayerPrefs.GetFloat("PZP"));
        player.transform.localRotation = new Quaternion(0f, PlayerPrefs.GetFloat("PYR"),0f,1f);*/

        #endregion
    }
}
