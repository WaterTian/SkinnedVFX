
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using UnityEngine;

[VFXBinder("LinesMap")]
public class AudioCircleLineMapBinder : VFXBinderBase
{

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    public ExposedProperty positionMapProperty;
    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    public ExposedProperty velocityMapProperty;

    
    public AudioCircleLine target = null;

    public override bool IsValid(VisualEffect component)
    {
        return target != null && component.HasTexture(positionMapProperty)&& component.HasTexture(velocityMapProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetTexture(positionMapProperty, target.PositionMap);
        component.SetTexture(velocityMapProperty, target.VelocityMap);
    }

}
