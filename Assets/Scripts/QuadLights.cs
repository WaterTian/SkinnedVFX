using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

using XXHash = Klak.Math.XXHash;

public class QuadLights
{
    GameObject[] _lightObjs;
    int _instanceCount = 0;

    public QuadLights(GameObject _prefab ,int _count,Transform _parent = null)
    {
        _instanceCount = _count;


        _lightObjs = new GameObject[_instanceCount];


        for (int i = 0; i < _instanceCount; i++)
        {
            var go = GameObject.Instantiate(_prefab, _parent);
            _lightObjs[i] = go;
        }
    }

    public void UpdatePos(Vector3[] _posList, float _size = 0.1f)
    {
        var t = Time.time;
        


        for (int i = 0; i < _instanceCount; i++)
        {
            
            _lightObjs[i].transform.localPosition = new Vector3(_posList[i].x,0,_posList[i].z);
            _lightObjs[i].transform.LookAt(Vector3.zero);

            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetAreaLightSize(new Vector2(_size, _size +_posList[i].y));
            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetIntensity(_posList[i].y);



            var hue = Mathf.Sin(t*0.1f+i*0.2f) * 0.5f + 0.5f;
            var saturation = 0.1f + _posList[i].y;
            if(saturation>1) saturation =1;
            _lightObjs[i].GetComponent<Light>().color = Color.HSVToRGB(hue,saturation, saturation);


        }
    }
}