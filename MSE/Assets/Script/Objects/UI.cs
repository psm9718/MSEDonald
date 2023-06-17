using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject _dirty;
    public Slider _sliderPrefab;
    Slider slider;
    Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        slider = Instantiate(_sliderPrefab, _dirty.transform.position, Quaternion.identity);
        slider.transform.SetParent(this.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = _camera.WorldToScreenPoint(_dirty.transform.position + new Vector3(0f, 1.5f, 0f));
    }
}
