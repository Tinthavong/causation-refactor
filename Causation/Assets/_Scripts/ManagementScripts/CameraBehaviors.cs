using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviors : MonoBehaviour
{
    public GameObject followObject;
    public Vector2 followOffset;
    public float speed = 3f;
    private Vector2 threshold;
    private Rigidbody2D rb;
    public float verticalOffset;
    private float defaultVert;

    // Start is called before the first frame update
    void Start()
    {
        defaultVert = verticalOffset;
        followObject = FindObjectOfType<PlayerStateManager>().gameObject;
        threshold = CalculateThreshold();
        rb = followObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {//Vector2 follow = Vector2(followObject.transform.position.x, followObject.transform.position.y + verticalOffset);
        float followX = followObject.transform.position.x;
        float followY = followObject.transform.position.y + verticalOffset;
        //Vector2 follow = followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * followX);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * followY);

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x)
        {
            newPosition.x = followX;
        }

        if (Mathf.Abs(yDifference) >= threshold.y)
        {
            newPosition.y = followY;
        }
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

        if (followObject.GetComponent<PlayerStateManager>().isControlling) {
            if (Input.GetKeyDown(KeyCode.S))
            {
                verticalOffset -= 2f;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                verticalOffset = defaultVert;
            }
        }
    }

    private Vector3 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
