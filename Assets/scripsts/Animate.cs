using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]

public class Animate : MonoBehaviour
{
    public float duration = 2.5f; // Trajanje animacije u sekundama
    public float rotationX = 90f; // Ciljna rotacija po X osi
    public float rotationZ = 90f; // Ciljna rotacija po Z osi

    public AnimationCurve easingCurve; // Kriva za easing u Unity editoru

    private Vector3 startRotation; // Početna rotacija objekta
    private Vector3 targetRotation; // Ciljna rotacija objekta
    private float elapsedTime = 0f; // Proteklo vreme
    public bool started = false; // Indikator da li je animacija počela

    void Start()
    {
        // Sačuvaj početnu i ciljnu rotaciju
        startRotation = transform.eulerAngles;
        targetRotation = new Vector3(startRotation.x + rotationX, startRotation.y, startRotation.z + rotationZ);
    }

    public void Play()
    {
        started = true;
    }

    void Update()
    {
        if (started)
        {
            
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime; // Povećaj proteklo vreme

                // Interpolacija između početne i ciljne rotacije
                float t = elapsedTime / duration; // Normalizovano vreme (0 do 1)
                float easedT = easingCurve.Evaluate(t); // Primeni easing krivu

                // Izračunaj novu rotaciju
                float newRotX = Mathf.Lerp(startRotation.x, targetRotation.x, easedT);
                float newRotZ = Mathf.Lerp(startRotation.z, targetRotation.z, easedT);

                // Ažuriraj rotaciju objekta
                transform.eulerAngles = new Vector3(newRotX, startRotation.y, newRotZ);
            }
            else
            {
                // Završena animacija, sakrij objekat
                started = false;
                gameObject.SetActive(false);
            }
        }
    }
}
