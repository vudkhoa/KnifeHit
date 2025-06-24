using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject knife;
    public Text text_score, text_high_score;
    void Start()
    {
        CreateKnife();  
    }
    // Update is called once per frame
    void Update()
    {
        text_score.text = "Score: " + GameController.GetScore();
        text_high_score.text = "High Score: " + GameController.GetHighScore();

    }
    
    public void CreateKnife() {
        if (GameObject.Find("Trunk").GetComponent<Trunk>().health > 1) {
            // Instantiate: Tạo clone của Object
            // Quaternion.identity: không xoay
            Instantiate(knife, transform.position, Quaternion.identity);    
        }
    }
}
