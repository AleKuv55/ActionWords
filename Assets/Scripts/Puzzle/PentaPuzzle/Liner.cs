using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using SpriteParticleEmitter;

public class Liner : MonoBehaviour
{
    [SerializeField] private float      _z;
    [SerializeField] private int        _segmentsPerPair;
    [SerializeField] private float      _leversPercentage;
    [SerializeField] private float      _minLeverLength;
    [SerializeField] private Vector3    _rotationAxis;
    

    private LineRenderer _lineRenderer;

    private bool _nowSelecting;

    private int _lettersCount;
    private List<Vector3> _lettersPos;

    public ParticleSystem _lineEffect;
    private List<ParticleSystem> _lineEffArray;

    public ParticleSystem _new;
    //public UIParticleSystem partSystem;

    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lettersCount = 1;
        _lettersPos = new List<Vector3>
        {
            Vector3.zero
        };
    }

    void Start()
    {
        _lineEffArray = new List<ParticleSystem>();

        PentaLetter[] letters = FindObjectsOfType<PentaLetter>();
        foreach (PentaLetter letter in letters)
        {
            letter.AddDragCallback(OnLettersBeingHighlighted);
            letter.AddDragEndedCallback(Clear);
            letter.AddLetterSelectedCallback(AddLetter);
        }
    }

    private void FixedUpdate()
    {
        if (_nowSelecting)
            DrawCurve();
    }


    private void OnLettersBeingHighlighted(PointerEventData eventData)
    {
        _nowSelecting = true;

        Vector3 mousePos3D = eventData.pointerCurrentRaycast.worldPosition;
        mousePos3D.z = _z;
        _lettersPos[_lettersCount-1] = mousePos3D;

        //First
        var dur = new ParticleSystem();
        //dur = _lineEffect;
        Vector3 a = new Vector3();
        a = mousePos3D;
        a.z -= 5;
        dur = Instantiate(_new, a, Quaternion.identity); 
        dur.transform.parent = GameObject.Find("VisualGame").transform;

        //_lineEffArray.Add(dur);

        //Second
        //partSystem.Init(mousePos3D);
        //partSystem.Play();

    }

    private void AddLetter(PentaLetter letter)
    {
        Vector3         letterPos   = letter.transform.position;
                        letterPos.z = _z;
        _lettersPos[_lettersCount++ - 1] = letterPos;
        _lettersPos.Add (letterPos);

        DrawCurve();
        
    }

    public void Clear(PentaLetter lastLetter)
    {
        _nowSelecting = false;

        _lineRenderer.positionCount = 0;

        _lettersCount = 1;
        _lettersPos.Clear();
        _lettersPos.Add(Vector3.zero);


     /*   foreach (ParticleSystem obj in _lineEffArray)
        {
            obj.Stop();
           // Destroy(obj.gameObject);
            Debug.Log("HOHOHOOHPHPHPHPHP");
        }
        _lineEffArray.Clear();*/
    }





    private void DrawCurve()
    {
        _lineRenderer.positionCount = 0;

        if (_lettersCount == 1) {  return; }

        if (_lettersCount == 2)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _lettersPos[0]);
            _lineRenderer.SetPosition(1, _lettersPos[1]);
            return;
        }

        Vector3 prevDir = Vector3.zero;

        for (int segN = 1; segN < _lettersCount; segN++)
        {
            Vector3 curDir = Vector3.zero;

            Vector3 supportingPoint1 = Vector3.zero;
            Vector3 supportingPoint2 = Vector3.zero;

            Vector3 A = _lettersPos[segN-1];
            Vector3 B = _lettersPos[segN];
            Vector3 C = (segN != _lettersCount-1) ? _lettersPos[segN+1] : _lettersPos[segN];
            

            Vector3 BA = A - B;
            Vector3 BC = C - B;
            float angleBetween = Mathf.Deg2Rad * Vector3.SignedAngle(BC, BA, _rotationAxis);
            

            float rotationAngle  = angleBetween;
                  rotationAngle += (rotationAngle >= 0) ? Mathf.PI : -Mathf.PI;
                  rotationAngle  = rotationAngle / 2;
            VectorService.RotateVector(ref BC, rotationAngle);

            curDir = BC.normalized;

            float lever1Length = _leversPercentage * BA.magnitude;
            if   (lever1Length < _minLeverLength) lever1Length = _minLeverLength;

            float lever2Length = _leversPercentage * BC.magnitude;
            if   (lever2Length < _minLeverLength) lever2Length = _minLeverLength;

            supportingPoint1 = A + ((-1) * lever1Length) * prevDir;
            supportingPoint2 = B + (       lever2Length) * curDir;

            prevDir = curDir;

            /*
            // Drawing supporting zigzag
            _lineRenderer.positionCount += 3;
            _lineRenderer.SetPosition (3 * segN, B);
            _lineRenderer.SetPosition (3 * segN - 1,     supportingPoint2);
            _lineRenderer.SetPosition (3 * segN - 2, supportingPoint1); Debug.LogWarning(supportingPoint1);
            */

            
            _lineRenderer.positionCount += _segmentsPerPair;
            for (int i = 0; i < _segmentsPerPair; i++)
            {
                float t = (float)i / (float)(_segmentsPerPair - 1);
                Vector3 point = CalculateBezierPoint(t, A, supportingPoint1, supportingPoint2, B);
                _lineRenderer.SetPosition((segN-1)*_segmentsPerPair + i, point);
            }
            
        }

    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }

}
