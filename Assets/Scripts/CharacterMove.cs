using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Play : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 playMove;
    private Rigidbody2D rb;
    private Animator playAnimator;
    private Vector2 lastMove = Vector2.zero; // Hướng cuối cùng
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private Tilemap groundTilemap;
    private bool isPushing = false;
    private Rigidbody2D boxRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playAnimator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        playerMovement();
        UpdateAnimation();

    }
    void playerMovement()
    {
        playMove = Vector2.zero;
        playMove.x = Input.GetAxisRaw("Horizontal");
        playMove.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate()
    {

        Debug.Log("Cell Size: " + groundTilemap.cellSize);
        Vector2 nextPosition = rb.position + playMove.normalized * moveSpeed * Time.fixedDeltaTime;
        Vector3Int gridPositionWater = waterTilemap.WorldToCell(nextPosition);
        Vector3Int gridPositionGround = groundTilemap.WorldToCell(nextPosition);

        bool isGroundTile = groundTilemap.HasTile(gridPositionGround);
        bool isWaterTile = waterTilemap.HasTile(gridPositionWater);

        if (isGroundTile || (isGroundTile && isWaterTile))
        {
            // Kiểm tra box phía trước
            RaycastHit2D hit = Physics2D.Raycast(rb.position, playMove.normalized, moveSpeed * Time.fixedDeltaTime);
            
            if (hit.collider != null && hit.collider.CompareTag("Box"))
            {
                boxRb = hit.collider.GetComponent<Rigidbody2D>();
                if (boxRb != null)
                {
                    // Kiểm tra xem phía trước box có vật cản không     
                    Vector2 boxNextPosition = boxRb.position + playMove.normalized * groundTilemap.cellSize.x;               
                    RaycastHit2D boxHit = Physics2D.Raycast(boxRb.position,playMove.normalized ,groundTilemap.cellSize.x);
                    
                    if (boxHit.collider == null)
                    {
                        // Di chuyển box
                        boxRb.MovePosition(boxNextPosition);
                        // Di chuyển player
                        rb.velocity = playMove.normalized * moveSpeed;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
            }
            else
            {
                rb.velocity = playMove.normalized * moveSpeed;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void UpdateAnimation()
    {
        // Kiểm tra xem nhân vật có đang di chuyển không
        bool isMoving = playMove != Vector2.zero;

        // Nếu nhân vật đang di chuyển, lưu hướng cuối cùng
        if (isMoving)
        {
            lastMove = playMove;
        }
        playAnimator.SetFloat("Horizontal", lastMove.x);
        playAnimator.SetFloat("Vertical", lastMove.y);
        playAnimator.SetBool("isPushing", isPushing);
        playAnimator.SetBool("isMoving", isMoving);

    }

}