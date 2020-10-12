using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
#pragma warning disable 0649

    [SerializeField] Vector3 _movementVector;
    [Range(0, 1)] [SerializeField] float _movementFactor;
    Vector3 _stratingVector;
    [SerializeField] float _period = 2f;
    const float tau = Mathf.PI * 2f;


    // Start is called before the first frame update
    void Start()
    {
        _stratingVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / _period;

        float rawSinWave = Mathf.Sin(cycles * tau);

        _movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = _movementFactor * _movementVector;
        transform.position = _stratingVector + offset;
    }
}
