using UnityEngine;

public class yavaslama : MonoBehaviour
{
    public float slowTimeScale = 0.5f; // Yavaþlatýlmýþ zaman ölçeði
    public float normalTimeScale = 1.0f; // Normal zaman ölçeði
    private bool isTimeSlowed = false; // Zamanýn yavaþlatýlýp yavaþlatýlmadýðýný kontrol eden deðiþken

    void Update()
    {
        // LeftShift tuþuna basýldýðýnda zamaný yavaþlat veya eski haline döndür
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isTimeSlowed)
            {
                Time.timeScale = normalTimeScale; // Zamaný normale döndür
                isTimeSlowed = false;
            }
            else
            {
                Time.timeScale = slowTimeScale; // Zamaný yavaþlat
                isTimeSlowed = true;
            }

            // Time.timeScale deðiþtirildiðinde, FixedUpdate metodunun da doðru çalýþmasý için timeScale'i sýfýrlýyoruz.
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
} 