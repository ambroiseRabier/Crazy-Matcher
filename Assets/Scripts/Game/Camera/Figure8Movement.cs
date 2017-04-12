using UnityEngine;
using System.Collections;

public class Figure8Movement : MonoBehaviour {
    public float speed = 10;
    public float scaleX = 0.01f;
    public float scaleZ = 0.01f;
    public float offsetX = 0;
    public float offsetZ = 0;

    public bool isLinkOffsetScalePositiveX = false;
    public bool isLinkOffsetScaleNegativeX = false;
    public bool isLinkOffsetScalePositiveZ = false;
    public bool isLinkOffsetScaleNegativeZ = false;
    public bool isFigure8 = true;

    private float phase;
    private float m_2PI = Mathf.PI * 2;
    private Vector3 originalPosition;
    private Vector3 pivot;
    private Vector3 pivotOffset;
    private bool isInverted = false;
    private bool isRunning = false;


    void Start() {
        pivot = transform.position;
        originalPosition = transform.position;
        isRunning = true;

        if (isLinkOffsetScalePositiveX)
            phase = 3.14f / 2f + 3.14f;
        else if (isLinkOffsetScaleNegativeX)
            phase = 3.14f / 2f;
        else if (isLinkOffsetScalePositiveZ)
            phase = 3.14f;
        else
            phase = 0;
    }

    void Update() {
        pivotOffset = Vector3.up * 2 * scaleZ;

        phase += speed * Time.deltaTime;

        if (isFigure8) {
            if (phase > m_2PI) {
                Debug.Log("phase " + phase + " over 2pi: " + isInverted);
                isInverted = !isInverted;
                phase -= m_2PI;
            }
            if (phase < 0) {
                Debug.Log("phase " + phase + " under 0");
                phase += m_2PI;
            }
        }

        Vector3 nextPosition = pivot + (isInverted ? pivotOffset : Vector3.zero);
        transform.position = new Vector3(
            nextPosition.x + Mathf.Sin(phase) * scaleX + offsetX,
            nextPosition.y + Mathf.Cos(phase) * (isInverted ? -1 : 1) * scaleZ + offsetZ,
            nextPosition.z
        );
    }

    void OnDrawGizmos() {
        if (isLinkOffsetScalePositiveX)
            offsetX = scaleX;
        else if (isLinkOffsetScaleNegativeX)
            offsetX = scaleX * -1;
        else
            offsetX = 0;

        if (isLinkOffsetScalePositiveZ)
            offsetZ = scaleZ;
        else if (isLinkOffsetScaleNegativeZ)
            offsetZ = scaleZ * -1;
        else
            offsetZ = 0;

        if (isRunning) {
            Gizmos.DrawLine(new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + scaleZ + offsetZ), new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + offsetZ));
            Gizmos.DrawLine(new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z - scaleZ + offsetZ), new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + offsetZ));
            Gizmos.DrawLine(new Vector3(originalPosition.x + scaleX + offsetX, originalPosition.z + offsetZ, originalPosition.y), new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + offsetZ));
            Gizmos.DrawLine(new Vector3(originalPosition.x - scaleX + offsetX, originalPosition.z + offsetZ, originalPosition.y), new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + offsetZ));
        }
        else {
            Gizmos.DrawLine(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + scaleZ + offsetZ), new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ));
            Gizmos.DrawLine(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z - scaleZ + offsetZ), new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ));
            Gizmos.DrawLine(new Vector3(transform.position.x + scaleX + offsetX, transform.position.y, transform.position.z + offsetZ), new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ));
            Gizmos.DrawLine(new Vector3(transform.position.x - scaleX + offsetX, transform.position.y, transform.position.z + offsetZ), new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ));
        }
    }
}