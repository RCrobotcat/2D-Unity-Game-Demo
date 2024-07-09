using UnityEngine;
using UnityEngine.SceneManagement;

public class grabbableController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        loadingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            savingPosition();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            savingPosition();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void loadingPosition()
    {
        if (PlayerPrefs.HasKey("ItemX") && PlayerPrefs.HasKey("ItemY"))
        {
            float x = PlayerPrefs.GetFloat("ItemX");
            float y = PlayerPrefs.GetFloat("ItemY");
            transform.position = new Vector3(x, y, 0);
        }
    }

    void savingPosition()
    {
        PlayerPrefs.SetFloat("ItemX", transform.position.x);
        PlayerPrefs.SetFloat("ItemY", transform.position.y);
        PlayerPrefs.Save(); // Ensure data is saved to disk immediately
    }
    public void ResetGame()
    {
        PlayerPrefs.SetFloat("ItemX", -0.34f);
        PlayerPrefs.SetFloat("ItemY", -0.98f);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
