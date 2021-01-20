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

    public void UpdatePos(List<Vector3> posList)
    {
        var t = Time.time;
        


        for (int i = 0; i < _instanceCount; i++)
        {
            if(i>=posList.Count) return;

            _lightObjs[i].transform.localPosition = new Vector3(posList[i].x,0,posList[i].z);
            _lightObjs[i].transform.LookAt(Vector3.zero);

            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetAreaLightSize(new Vector2(0.1f, 0.1f+posList[i].y));
            _lightObjs[i].GetComponent<HDAdditionalLightData>().SetIntensity(posList[i].y);



            var hue = Mathf.Sin(t*0.1f+i*0.2f) * 0.5f + 0.5f;
            var saturation = 0.1f + posList[i].y;
            if(saturation>1) saturation =1;
            _lightObjs[i].GetComponent<Light>().color = Color.HSVToRGB(hue,saturation, saturation);


        }
    }
}