using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGui : MonoBehaviour {
    private PlayerDatas _playerDatas;
    private Text _healthText;
    
	void OnEnable () {
        _playerDatas = GetComponent<PlayerDatas>();
        _healthText = GameObject.Find("Canvas/Health").GetComponent<Text>();
	}
	
	public void OnHealthChange(int currentHealth, int maxHealth){
        _healthText.text = currentHealth + " / " + maxHealth;
        _healthText.color = new Color((((float)maxHealth - currentHealth) / maxHealth), (1.0f-((255-180)/255)) * (currentHealth / maxHealth), (-0.02f*(255*currentHealth/maxHealth)+4* (255 * currentHealth / maxHealth))/255);
    }
}