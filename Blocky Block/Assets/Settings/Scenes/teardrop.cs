using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class teardrop : MonoBehaviour {
 
    private ParticleSystem ps;
    public float sizeMultiplier = 1.0f;
    public float heightMultiplier = 1.0f;
    public float radiusMultiplier = 1.0f;
 
    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
 
        var main = ps.main;
        main.startSpeed = 0.0f;
        main.startSize = 0.05f;
 
        var emission = ps.emission;
        emission.enabled = false;
    }
   
    // Update is called once per frame
    void Update () {
 
        float theta = Random.Range(0.0f, 1.0f);
        float phi = Random.Range(0.0f, 1.0f);
 
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = Eval(theta * Mathf.PI * 2.0f, phi * Mathf.PI) * sizeMultiplier;
        // emitParams.startColor = new Color(0.0f, phi, 0.0f);
        ps.Emit(emitParams, 1);
    }
 
    private Vector3 Eval(float theta, float phi)
    {
        float sinT = Mathf.Sin(theta);
        float cosT = Mathf.Cos(theta);
 
        Vector3 p;
 
        p.x = 0.5f * (1f - cosT) * sinT * Mathf.Cos(phi) * radiusMultiplier;
        p.y = 0.5f * (1f - cosT) * sinT * Mathf.Sin(phi) * radiusMultiplier;
        p.z = cosT * heightMultiplier;
 
        return (p);
    }
}