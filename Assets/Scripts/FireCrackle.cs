using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCrackle : MonoBehaviour
{
    [SerializeField]
    private float _minIntensity;
    [SerializeField]
    private float _maxIntensity;
    [SerializeField]
    private float _delay;
    private float _lerpTo;
    private Light _light;
    private float _counter;

    void Start() {
        _light = GetComponent<Light>();
        _lerpTo = Random.Range(_minIntensity, _maxIntensity);        
    }

    void Update()
    {
        _counter += Time.deltaTime;

        if (_counter >= _delay) {
            _counter = 0f;
            _lerpTo = Random.Range(_minIntensity, _maxIntensity);
        }

        _light.intensity = Mathf.Lerp(_light.intensity, _lerpTo, Time.deltaTime);
    }
}
