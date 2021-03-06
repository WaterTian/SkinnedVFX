﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioQuads : MonoBehaviour
{
    [SerializeField] AudioSource thisAudioSource;
    [SerializeField] Vector2Int spectrumArea = new Vector2Int(0, 64);
    [Space]
    [SerializeField] GameObject _prefab = null;
    [Space]
    public Texture2D PositionMap;
    private Texture2D _prePositionMap;
    public Texture2D VelocityMap;

    public float R = 1.0f;

    private float[] _spectrumData = new float[8192];

    private Vector3[] _lightsPos;
    private QuadLights _quadLights;

    public float LightSize=0.1f;

    void Start()
    {
        int _pointsCont = (spectrumArea.y - spectrumArea.x)*10;
        PositionMap = new Texture2D(_pointsCont, 1, TextureFormat.RGBAFloat, false)
        {
            hideFlags = HideFlags.DontSave,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat,
        };

        _prePositionMap = new Texture2D(_pointsCont, 1, TextureFormat.RGBAFloat, false)
        {
            hideFlags = HideFlags.DontSave,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat,
        };

        VelocityMap = new Texture2D(_pointsCont, 1, TextureFormat.RGBAFloat, false)
        {
            hideFlags = HideFlags.DontSave,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat,
        };



        _lightsPos = new Vector3[6];
        _quadLights =  new QuadLights(_prefab, 6, this.transform);



    }
    void Update()
    {
        thisAudioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);

        var _positionPixels = PositionMap.GetPixels();

        var _startA = spectrumArea.x + (spectrumArea.y - spectrumArea.x) / 2;
        var _step = 0.1f;
        for (int i = spectrumArea.x; i < spectrumArea.y; ++i)
        {
            var _cs = _spectrumData[i] * 40;
            var _ns = _spectrumData[i + 1] * 40;
            Vector3 _c_p = new Vector3(Mathf.Sin((i - _startA) * _step) * R, _cs, Mathf.Cos((i - _startA) * _step) * R);
            Vector3 _n_p = new Vector3(Mathf.Sin((i + 1 - _startA) * _step) * R, _ns, Mathf.Cos((i - _startA + 1) * _step) * R);

            if (i % 10 == 0){
                _lightsPos[i / 10 - 1] = _c_p;
            }


            for (int j = 0; j < 10; ++j)
            {
                float _x = _c_p.x + (_n_p.x - _c_p.x) * j / 10;
                float _y = _c_p.y + (_n_p.y - _c_p.y) * j / 10;
                float _z = _c_p.z + (_n_p.z - _c_p.z) * j / 10;
                _positionPixels[(i - spectrumArea.x) * 10 + j] = new Color(_x, _y, _z, 1);
            }
        }




        PositionMap.SetPixels(_positionPixels);
        PositionMap.Apply();

        var _prePositionPixels = _prePositionMap.GetPixels();
        var _velocityPixels = VelocityMap.GetPixels();
        for (int i = 0; i < _positionPixels.Length; ++i)
        {
            float _vx = _positionPixels[i].r - _prePositionPixels[i].r;
            float _vy = _positionPixels[i].g - _prePositionPixels[i].g;

            float _dc = Vector2.Distance(new Vector2(_positionPixels[i].r, _positionPixels[i].g), Vector2.zero);
            float _dp = Vector2.Distance(new Vector2(_prePositionPixels[i].r, _prePositionPixels[i].g), Vector2.zero);
            if (_dc - _dp < 0) _vx = _vy = 0;

            _velocityPixels[i] = new Color(_vx, _vy, 0, 1);

        }


        VelocityMap.SetPixels(_velocityPixels);
        VelocityMap.Apply();

        _prePositionMap.SetPixels(_positionPixels);
        _prePositionMap.Apply();

    }

    void LateUpdate()
    {

        _quadLights.UpdatePos(_lightsPos,LightSize);
    }



    void OnDestroy()
    {

    }


    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;

    //     var _startA = spectrumArea.x + (spectrumArea.y - spectrumArea.x) / 2;
    //     var _step = 0.1f;
    //     for (int i = spectrumArea.x; i < spectrumArea.y; ++i)
    //     {
    //         var _cs = _spectrumData[i] * 30;
    //         var _ns = _spectrumData[i + 1] * 30;
    //         Vector3 _c_p = new Vector3(Mathf.Sin((i - _startA) * _step) * R, _cs, Mathf.Cos((i - _startA) * _step) * R);
    //         Vector3 _n_p = new Vector3(Mathf.Sin((i + 1 - _startA) * _step) * R, _ns, Mathf.Cos((i - _startA + 1) * _step) * R);
    //         Gizmos.DrawLine(gameObject.transform.TransformPoint(_c_p), gameObject.transform.TransformPoint(_n_p));
    //     }
    // }

}
