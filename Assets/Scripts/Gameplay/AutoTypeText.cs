using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoTypeText : MonoBehaviour {
	
	// for GUI Text
	// Place the script on any GUI Text and remove following values from GUI Text properties:
	// Pixel offset, Pixel correct and Rich text. Make them all zero. And use transform.scale
	// values to scale text. This will fix GUI text to all screen resolutions.
	
	
	public float letterPause = 0.2f;
	public AudioClip sound;
	private string word;
	public bool music=true;
	
	// if want to do it n only start use Start()
	// public void Start () {
	public void OnEnable () {
		//		word = GetComponent<GUIText>().text;
		//		GetComponent<GUIText>().text = "";
		word = GetComponent<Text>().text;
		GetComponent<Text>().text = "";
		
		StartCoroutine( TypeText ());
	}
	
	public void OnDisable () {
		GetComponent<Text>().text = "";
	}
	
	IEnumerator TypeText () {
		if (music = true) {
			foreach (char letter in word.ToCharArray()) {
				GetComponent<Text> ().text += letter;
			
				if (PlayerPrefs.GetInt ("Music") == 0) {
					if (sound) {
						//					Debug.Log("Sound");
						GetComponent<AudioSource> ().PlayOneShot (sound);
					}
				}
				yield return new WaitForSeconds (letterPause);
			}		
		}
	}
	
}
