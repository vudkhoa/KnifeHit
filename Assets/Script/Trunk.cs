using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trunk : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 1.5f;
    public int health = 5;
    public AudioClip startClip; 
    public int maxLevel = 3;
    void Start() {
        GetComponent<AudioSource>().PlayOneShot(startClip);        
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(0, 0, speed));
    }

    public void Damged(int damge) { 
        health -= damge;
        if (health == 0) { 
            GetComponent<CircleCollider2D>().enabled = false;
            
            // Trả lại tác động vật lý
            transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            // Tác động 1 lực theo hướng Vector
            transform.GetChild(0).GetComponent<Rigidbody>().AddForce(400, 800, 0);
            // Tác động 1 lực xoay theo hướng Vector
            transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(100, 100, 100);
            // Xóa khỏi cụm
            transform.GetChild(0).parent = null;

            // Làm tương tự vs hai mảnh gỗ còn lại
            transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-400, 800, 0);
            transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(-100, 100, 100);
            transform.GetChild(0).parent = null;
            transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(0).GetComponent<Rigidbody>().AddForce(0, 800, 0);
            transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(200, 100, -   100);
            transform.GetChild(0).parent = null;

            // Dao
            while (transform.childCount > 0) { 
                transform.GetChild(0).GetComponent<Rigidbody2D>().isKinematic = false;
                // Random Vector
                transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-400f, 400f), Random.Range(400f, 800f)));
                // Random Vector
                transform.GetChild(0).GetComponent<Rigidbody2D>().AddTorque(Random.Range(-400, 400));
                transform.GetChild(0).parent = null;
            }
            if (SceneManager.GetActiveScene().buildIndex == maxLevel) {
                GameObject.Find("TxtMessage").GetComponent<Text>().text = "MAX LEVEL!";
            }
            else {
                GameObject.Find("TxtMessage").GetComponent<Text>().text = "YOU WIN!";
            }
            // StartCoroutine: "Tạm dừng" 2s, để NextLevel
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel() {
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene().buildIndex == maxLevel) { 
            GameController.ResetScore();
            SceneManager.LoadScene(0);
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
