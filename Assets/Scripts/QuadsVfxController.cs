using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadsVfxController : MonoBehaviour
{
    [SerializeField] AudioQuads audioQuads;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _mouse = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
        var vfx = GetComponent<UnityEngine.VFX.VisualEffect>();
        // vfx.SetVector3("MousePostion", _mouse);

        vfx.SetTexture("PositionMap",audioQuads.PositionMap);
        // vfx.SetTexture("VelocityMap", audioQuads.VelocityMap);



    }
}
