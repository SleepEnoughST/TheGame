using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Player_Controller PC;
    public Camera mainCamera;
    public LineRenderer _lineRenderer;
    public DistanceJoint2D _distanceJoint;
    public GameObject grapple;
    public StopPlayer SP;
    public GrabTrigger GT;
    [Header("General Settings:")]
    [SerializeField] private int percision = 80;
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    public bool isGrappling = true;
    public bool strightLine = true;
    public bool grab;
    // Start is called before the first frame update
    void Start()
    {
        SP = GetComponent<StopPlayer>();
        GT = GetComponent<GrabTrigger>();
        _lineRenderer.positionCount = percision;
        _distanceJoint.enabled = false;
        _lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        moveTime = 0;
        _lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        strightLine = false;
        isGrappling = false;
        _lineRenderer.enabled = true;
        //_lineRenderer.enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
        if (!_lineRenderer.enabled)
        {
            _distanceJoint.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            //Vector2 mousePos = mainCamera.ScreenToWorldPoint(transform.position);
            //_lineRenderer.SetPosition(0, garpple.transform.position);

            //_lineRenderer.SetPosition(1, transform.position);
            _distanceJoint.connectedAnchor = grapple.transform.position;


            SP.enabled = true;
            grab = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            //LinePointsToFirePoint();
            GT.cooldown = 1f;
            _distanceJoint.enabled = false;
            if (!_distanceJoint.enabled)
            {
                _lineRenderer.enabled = false;
            }

            SP.enabled = false;
            if (PC.facingright)
            {
                PC.rb.velocity = new Vector2(1, 10);
                PC.rb.AddForce(Vector2.right * PC.playerJump);
            }
            else
            {
                PC.rb.velocity = new Vector2(-1, 10);
                PC.rb.AddForce(Vector2.left * PC.playerJump);
            }
            if (_distanceJoint.enabled)
            {

                DrawRopeNoWaves();
            }
            _distanceJoint.autoConfigureDistance = true;
            //strightLine = false;
            grab = false;
        }
        //if (_distanceJoint.enabled)
        //{
        //    _lineRenderer.SetPosition(1, transform.position);
        //}

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _distanceJoint.autoConfigureDistance = false;
            _distanceJoint.distance -= Time.deltaTime * 3;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _distanceJoint.autoConfigureDistance = false;
            _distanceJoint.distance += Time.deltaTime * 3;
        }
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            _lineRenderer.SetPosition(i, transform.position);
        }
    }

    void DrawRopeWaves()
    {
        for (int i = 0; i < percision; i++)
        {
            float delta = i / (percision - 1f);
            Vector2 offset = Vector2.Perpendicular(grapple.transform.position * ropeAnimationCurve.Evaluate(delta) * waveSize);
            Vector2 targetPosition = Vector2.Lerp(transform.position, grapple.transform.position, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(transform.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            _lineRenderer.SetPosition(i, currentPosition);
        }
    }
    
    void DrawRope()
    {
        if (!strightLine && !isGrappling)
        {
            if (_lineRenderer.GetPosition(percision - 1).x == grapple.transform.position.x)
            {
                strightLine = true;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling && _lineRenderer.enabled == true)
            {
                isGrappling = true;
            }

            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (_lineRenderer.positionCount != 2) { _lineRenderer.positionCount = 2; }
                DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeNoWaves()
    {
        _lineRenderer.SetPosition(0, grapple.transform.position);
        _lineRenderer.SetPosition(1, transform.position);
        _distanceJoint.enabled = true;
    }
}
