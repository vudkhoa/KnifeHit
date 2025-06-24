using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 20f;
    public AudioClip hitSound; 
    public AudioClip fail;
    Rigidbody2D knifeRigid;
    bool moving = false;
    GameObject spawn;
    public bool scriptEnabled = true;

    public Vector2 vt = new Vector2(0, 1);

    void Start()
    {
        knifeRigid = GetComponent<Rigidbody2D>();
        spawn = GameObject.Find("Spawn");       
    }

    void FixedUpdate () { 
        // S = v * t, knifeRigid.position: vị trí hiện tại, Vector2.up = (0, 1) đi lên.
        if (moving) { 
            knifeRigid.MovePosition(knifeRigid.position + vt * speed * Time.deltaTime);
        }

        if (Input.GetMouseButton(0)) { 
            moving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(scriptEnabled)
        {
            if (collider.name == "Trunk") {
                moving = false;
                // Chuyển cha dao thành thằng vừa đụng (Trunk)
                transform.parent = collider.transform;
                // Tắt PolygonCollider2D
                GetComponent<PolygonCollider2D>().enabled = false;
                // Bật Kinematic (không còn vật lý)
                knifeRigid.bodyType = RigidbodyType2D.Kinematic;
                // Tắt PolygonCollider2D của KnifeCollider
                transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;
                if (speed == 20) {
                    // Tạo knife mới bằng Spawn.cs
                    spawn.GetComponent<Spawn>().CreateKnife();
                    // Trừ dame trong trunk
                    collider.GetComponent<Trunk>().Damged(1);
                    GameController.SetScore(10);
                }
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                // Set điểm cho GameController
            }
            else if (collider.name != null) {
                moving = false;
                GameController.SetScore(-10);
                knifeRigid.bodyType = RigidbodyType2D.Dynamic;
                GetComponent<AudioSource>().PlayOneShot(fail);
                GameObject.Find("TxtMessage").GetComponent<Text>().text = "GAME OVER";
                GameController.SaveHighScore();
                GameController.ResetScore();
                // StartCoroutine: "Tạm dừng" 2s rồi chuyển sang Level1
                StartCoroutine(GoToLevel1());
            }

            scriptEnabled = false;
            GetComponent<Knife>().enabled = false;
        }
    }
    IEnumerator GoToLevel1()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Level1");
    }
}
