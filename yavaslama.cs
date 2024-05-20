using UnityEngine;

public class yavaslama : MonoBehaviour
{
    public float slowTimeScale = 0.5f; // Yava�lat�lm�� zaman �l�e�i
    public float normalTimeScale = 1.0f; // Normal zaman �l�e�i
    private bool isTimeSlowed = false; // Zaman�n yava�lat�l�p yava�lat�lmad���n� kontrol eden de�i�ken

    void Update()
    {
        // LeftShift tu�una bas�ld���nda zaman� yava�lat veya eski haline d�nd�r
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isTimeSlowed)
            {
                Time.timeScale = normalTimeScale; // Zaman� normale d�nd�r
                isTimeSlowed = false;
            }
            else
            {
                Time.timeScale = slowTimeScale; // Zaman� yava�lat
                isTimeSlowed = true;
            }

            // Time.timeScale de�i�tirildi�inde, FixedUpdate metodunun da do�ru �al��mas� i�in timeScale'i s�f�rl�yoruz.
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
} 