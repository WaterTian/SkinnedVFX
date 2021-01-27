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

            float intensity = _posList[i].y;

            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetAreaLightSize(new Vector2(_size, _size +intensity));
            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetIntensity(intensity);

            _lightObjs[i].transform.GetChild(0).transform.localScale = new Vector3(_size, _size + intensity,1);



            var hue = Mathf.Sin(t*0.1f+i*0.2f) * 0.5f + 0.5f;
            var saturation = 0.1f + intensity;
            if(saturation>1) saturation =1;
            var _color = Color.HSVToRGB(hue, saturation, saturation);
            _lightObjs[i].GetComponent<Light>().color =_color;

            Material _mat = new Material(Shader.Find("HDRP/Lit"));

            _mat.SetColor("_EmissiveColor", _color);
            _mat.SetInt("_UseEmissiveIntensity",1);
            _mat.SetFloat("_EmissiveIntensity",intensity*0.01f);
            _lightObjs[i].transform.GetChild(0).GetComponent<Renderer>().material= _mat;

        }
    }
}