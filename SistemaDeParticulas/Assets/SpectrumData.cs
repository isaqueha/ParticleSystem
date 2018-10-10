﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpectrumData : MonoBehaviour
{
    Vector3[] originalVertices;
    Mesh gameObjectMesh;
    public int deformedVertices = 4;

    private void Start() {
        gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        originalVertices = gameObjectMesh.vertices;        
    }

    void Update() {
        var spectrum = getSpectrumFromAudio();
        // var newVertices = deformOriginalVertices(spectrum);
        // gameObjectMesh.vertices = newVertices;
        MeshDeform deform = gameObject.GetComponent<MeshDeform>();
        if (deform) {
            for (int i = 0; i < deformedVertices; i++) {
                int rand = (int) (Random.value * originalVertices.Length);
                Vector3 point = originalVertices[rand];
                deform.DeformPoint(point, spectrum[2]);
            }
        }
    }

    float[] getSpectrumFromAudio() {
        var spectrum = new float[64];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        return spectrum;
    }

    Vector3[] deformOriginalVertices(float[] spectrum) {
        var deformedVertices = new Vector3[originalVertices.Length];
        var counter = 0;
        for (int i = 0; i < originalVertices.Length; i++) {
            counter++;
            float x = originalVertices[i].x;
            float y = originalVertices[i].y;
            float z = originalVertices[i].z;
            if (counter == 20) {
                x = x * spectrum[0] * 10;
                counter = 0;
            }
            deformedVertices[i] = new Vector3(x, y, z);
        }
        return deformedVertices;
    }
}