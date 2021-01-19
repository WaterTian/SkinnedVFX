using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RectLights
{
    List<GameObject> lightObj = new List<GameObject>();
    List<HDAdditionalLightData> lightDatatList = new List<HDAdditionalLightData>();

    public RectLights(int lightNum,Transform _parent = null)
    {
        for (int i = 0; i < lightNum; i++)
        {
            var tempObj = new GameObject();
            tempObj.AddComponent<Light>();
            tempObj.GetComponent<Light>().type = LightType.Area;

            tempObj.AddComponent<HDAdditionalLightData>().areaLightShape = AreaLightShape.Rectangle;
            tempObj.GetComponent<HDAdditionalLightData>().SetIntensity(0.5f);
            tempObj.GetComponent<HDAdditionalLightData>().SetRange(2);
            
            lightObj.Add(tempObj);

            lightDatatList.Add(tempObj.GetComponent<HDAdditionalLightData>());

            if (_parent) tempObj.transform.SetParent(_parent);

        }
    }

    // 更新位置
    public void UpdatePos(List<Vector3> posList)
    {
        for (int i = 0; i < posList.Count-1; i++)
        {
            // lightObj[i].transform.position = (posList[i] + posList[i + 1]) / 2f;
            // lightObj[i].transform.LookAt(posList[i]);
            // lightObj[i].transform.rotation *= Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
            // // 第一个参数决定了 tube 的长度
            // lightDatatList[i].SetAreaLightSize(new Vector2((posList[i] - posList[i + 1]).magnitude, 10f)); 

            lightObj[i].transform.position = (posList[i] + posList[i + 1]) / 2f;
            lightObj[i].transform.LookAt(Vector3.zero);
            // lightObj[i].transform.rotation *= Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
            // // 第一个参数决定了 tube 的长度
            lightDatatList[i].SetAreaLightSize(new Vector2(0.1f, 1f));


        }
    }
}