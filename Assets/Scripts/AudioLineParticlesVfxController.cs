using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLineParticlesVfxController : MonoBehaviour
{
    [SerializeField] AudioLines audioLines;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _mouse = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
        var vfx = GetComponent<UnityEngine.VFX.VisualEffect>();
        // vfx.SetVector3("MousePostion", _mouse);

        vfx.SetTexture("PositionMap",audioLines.PositionMap);
        vfx.SetTexture("VelocityMap", audioLines.VelocityMap);



    }
}
