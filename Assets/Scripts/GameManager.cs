using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public CanvasGroup bombFlashCanvas;

    public Text statusText;
    public Text healthText;
    public Text destroyedText;
    public Text missesLeftText;

    public Color goodHealth;
    public Color warnHealth;
    public Color badHealth;

    public int missesBeforeLoss = 5;

    public int playerHealth = 100;
    public int destroyed = 0;
    public int missed = 0;

    private bool playerAlive = true;
    private bool bombFlash = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    void UpdateHealthText()
    {
        healthText.text = playerHealth.ToString();
        if (playerHealth >= 60)
            healthText.material.color = goodHealth;
        else if (playerHealth >= 26)
            healthText.material.color = warnHealth;
        else
            healthText.material.color = badHealth;
    }

    void UpdateDestroyedText()
    {
        destroyedText.text = destroyed.ToString();
    }

    void UpdateMissesLeftText()
    {
        missesLeftText.text = (missesBeforeLoss - missed).ToString();
    }

    void InitGame()
    {
        UpdateHealthText();
        UpdateDestroyedText();
        UpdateMissesLeftText();

        statusText.material.color = Color.white;
        statusText.text = "READY!";
        BlinkStatusText(1.8f, 0.3f, true);

        // Start our super awesome free bgm
        var fx = GetComponent<AudioSource>();
        if (fx != null)
        {
            if (!fx.isPlaying)
                fx.Play();
        }
    }

    public IEnumerator FlashText(Text flashingText, float duration, float interval, bool clearWhenDone = true)
    {
        var oldText = flashingText.text;
        float started = Time.realtimeSinceStartup;

        while (true)
        {
            flashingText.text = "";
            yield return new WaitForSeconds(interval);

            flashingText.text = oldText;
            yield return new WaitForSeconds(interval);

            if (Time.realtimeSinceStartup - started >= duration)
            {
                if (clearWhenDone)
                    flashingText.text = "";

                break;
            }
        }
    }

    public void BlinkStatusText(float duration, float interval, bool clearWhenDone)
    {
        StartCoroutine(FlashText(statusText, duration, interval, clearWhenDone));
    }

    public void DestroyRock()
    {
        destroyed++;
        UpdateDestroyedText();
    }

    public void KillPlayer()
    {
        playerAlive = false;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            KillPlayer();
        }

        UpdateHealthText();
        if (damage <= 25)
        {
            statusText.text = "WARNING!";
            statusText.material.color = badHealth;
            BlinkStatusText(1.5f, 0.2f, true);
        }
    }

    public void MissedRock()
    {
        missed++;
        UpdateMissesLeftText();
    }

    void Update()
    {
        if (bombFlash)
        {
            bombFlashCanvas.alpha = bombFlashCanvas.alpha - Time.deltaTime;
            if (bombFlashCanvas.alpha <= 0)
            {
                bombFlashCanvas.alpha = 0;
                bombFlash = false;
            }
        }
    }

    public void SlowMotion()
    {
        StartCoroutine(DoSlowMotion());
    }

    private IEnumerator DoSlowMotion()
    {
        Time.timeScale = 1f / 4f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / 4f;

        yield return new WaitForSeconds(0.25f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * 4f;
    }

    public void BombFlash()
    {
        bombFlash = true;
        bombFlashCanvas.alpha = 1;
    }
}
