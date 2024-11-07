using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobFrequency = 10.0f;
    [Range(0.001f, 0.1f)]
    public float bobAmount = 0.05f; // Tinggi goyangan
    [Range(1f, 30f)]
    public float smooth = 10f; // Kecepatan kembali ke posisi awal

    private Vector3 originalCameraPosition;
    private float bobTimer = 0f;

    void Start()
    {
        originalCameraPosition = transform.localPosition; // Simpan posisi awal kamera
    }

    void Update()
    {
        CheckForHeadbobTrigger();
    }

    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob(inputMagnitude);
        }
        else
        {
            // Kembali ke posisi asli jika tidak bergerak
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalCameraPosition, Time.deltaTime * smooth);
        }
    }

    private void StartHeadBob(float inputMagnitude)
    {
        bobTimer += Time.deltaTime * bobFrequency * inputMagnitude; // Mengupdate timer bob berdasarkan kecepatan gerakan

        // Hitung posisi goyang
        Vector3 newCameraPosition = originalCameraPosition;
        newCameraPosition.y += Mathf.Sin(bobTimer) * bobAmount; // Goyangan vertikal
        newCameraPosition.x += Mathf.Cos(bobTimer * 0.5f) * bobAmount; // Goyangan horizontal

        transform.localPosition = newCameraPosition; // Terapkan posisi baru
    }
}
