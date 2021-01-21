
using UnityEngine;
using Unity.Mathematics;

public class AudioLines : MonoBehaviour
{
    [SerializeField] AudioSource thisAudioSource;
    [SerializeField] Vector2Int spectrumArea = new Vector2Int(0, 64);

    public Texture2D PositionMap;
    private Texture2D _prePositionMap;
    public Texture2D VelocityMap;
    public float R = 1.0f;
    private float[] _spectrumData = new float[8192];

    Vector3[] _vertices;
    Color[] _colors;
    Mesh _mesh;

    [Space]
    [SerializeField] Material _lineMaterial = null;
    [Space]
    [SerializeField] Transform _lineStartTarget =null;
    [Space]
    [SerializeField] float3 _palette1 = (float3)0.1f;
    [SerializeField] float3 _palette2 = (float3)1;


    void Start()
    {
        int _pointsCont = (spectrumArea.y - spectrumArea.x)*30;
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



        /// line mesh
        var vcount = spectrumArea.y - spectrumArea.x +1;
        _vertices = new Vector3[vcount];
        _colors = new Color[vcount];
        // Initial vertex positions
        for (var i = 0; i < _vertices.Length; i++) _vertices[i] = (float3)0;
        
        // Index array
        var indices = new int[_vertices.Length];
        for (var i = 0; i < indices.Length; i++) indices[i] = i;

        _mesh = new Mesh();
        _mesh.vertices = _vertices;
        _mesh.colors = _colors;
        _mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);

    }
    void Update()
    {
        var t = Time.time;

        thisAudioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);

        var _positionPixels = PositionMap.GetPixels();

        var _startA = spectrumArea.x + (spectrumArea.y - spectrumArea.x) / 2;
        var _step = 0.1f;
        for (int i = spectrumArea.x; i < spectrumArea.y; ++i)
        {
            var _r = R - i * 0.003f;
            var _cs = _spectrumData[i] * 10;
            var _ns = _spectrumData[i + 1] * 10;
            Vector3 _c_p = new Vector3(Mathf.Sin((i - _startA) * _step) * _r, _cs, Mathf.Cos((i - _startA) * _step) * _r);
            Vector3 _n_p = new Vector3(Mathf.Sin((i + 1 - _startA) * _step) * _r, _ns, Mathf.Cos((i - _startA + 1) * _step) * _r);

            _vertices[i - spectrumArea.x] = _c_p;

            for (int j = 0; j < 30; ++j)
            {
                float _x = _c_p.x + (_n_p.x - _c_p.x) * j / 30;
                float _y = _c_p.y + (_n_p.y - _c_p.y) * j / 30;
                float _z = _c_p.z + (_n_p.z - _c_p.z) * j / 30;
                _positionPixels[(i - spectrumArea.x) * 30 + j] = new Color(_x, _y, _z, 1);
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


        for (var i = 0; i < _vertices.Length; i++)
        {
            var c = (math.sin(_palette1 * i + _palette2 * t) + 1) * 1.5f;
            _colors[i] = new Color(c.x, c.y, c.z);
        }
        // _vertices[_vertices.Length - 1] = _vertices[0];
        _vertices[0] = _vertices[_vertices.Length - 1] = _lineStartTarget.position;
        _mesh.vertices = _vertices;
        _mesh.colors = _colors;
    }

    void LateUpdate()
    {
        // Draw call for the light cord
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _lineMaterial, gameObject.layer);
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
    //         var _r = R-i*0.003f;
    //         var _cs = _spectrumData[i] * 10;
    //         var _ns = _spectrumData[i + 1] * 10;
    //         Vector3 _c_p = new Vector3(Mathf.Sin((i - _startA) * _step) * _r, _cs, Mathf.Cos((i - _startA) * _step) * _r);
    //         Vector3 _n_p = new Vector3(Mathf.Sin((i + 1 - _startA) * _step) * _r, _ns, Mathf.Cos((i - _startA + 1) * _step) * _r);

    //         Gizmos.DrawLine(gameObject.transform.TransformPoint(_c_p), gameObject.transform.TransformPoint(_n_p));
    //     }
    // }

}
