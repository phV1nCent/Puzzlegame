using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 playMove;
    private Rigidbody2D rb;
    private Animator playAnimator;
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private Tilemap groundTilemap;
    private bool isPushing = false;
    private Rigidbody2D boxRb;
    [SerializeField] private LayerMask boxLayer; // Layer dành cho box
    [SerializeField] private LayerMask obstacleLayer; // Layer dành cho vật cản


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
        if (isPushing)
        {
            rb.velocity = Vector2.zero;
            return; // Dừng mọi hành động khác khi đang đẩy
        }

        Vector2 nextPosition = rb.position + playMove.normalized * moveSpeed * Time.fixedDeltaTime;

        // Kiểm tra tile hợp lệ
        Vector3Int gridPositionWater = waterTilemap.WorldToCell(nextPosition);
        Vector3Int gridPositionGround = groundTilemap.WorldToCell(nextPosition);

        bool isGroundTile = groundTilemap.HasTile(gridPositionGround);
        bool isWaterTile = waterTilemap.HasTile(gridPositionWater);

        if (isGroundTile || (isGroundTile && isWaterTile))
        {
            // Kiểm tra box phía trước
            RaycastHit2D hit = Physics2D.Raycast(rb.position, playMove.normalized, 0.5f, boxLayer);
            if (hit.collider != null)
            {
                Rigidbody2D boxRb = hit.collider.GetComponent<Rigidbody2D>();
                if (boxRb != null)
                {
                    Vector2 boxNextPosition = boxRb.position + playMove.normalized * groundTilemap.cellSize.x;

                    // Kiểm tra xem vị trí tiếp theo của box có vật cản không
                    Collider2D obstacle = Physics2D.OverlapCircle(boxNextPosition, 0.1f, obstacleLayer);
                    if (obstacle == null)
                    {
                        StartCoroutine(PushBox(boxRb, boxNextPosition));
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
            }
            else
            {
                // Không va chạm với box, di chuyển bình thường
                rb.velocity = playMove.normalized * moveSpeed;
            }
        }
        else
        {
            // Nếu vị trí tiếp theo không hợp lệ, nhân vật dừng
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator PushBox(Rigidbody2D boxRb, Vector2 targetPosition)
    {
        isPushing = true;
        rb.velocity = Vector2.zero;

        boxRb.velocity = Vector2.zero;

        // Lấy vị trí ô hiện tại và ô tiếp theo của box
        Vector3Int targetGridPos = groundTilemap.WorldToCell(targetPosition);
        Debug.Log(" targetGrid" + targetGridPos);
        // Lấy vị trí thế giới chính giữa của ô đích
        Vector3 endWorldPos = groundTilemap.GetCellCenterWorld(targetGridPos);

        boxRb.velocity = Vector2.zero;
        // Di chuyển box đến đúng vị trí ô tiếp theo
        boxRb.position = endWorldPos;

        isPushing = false;
        yield return null;
    }
    void UpdateAnimation()
    {
        // Kiểm tra xem nhân vật có đang di chuyển không
        bool isMoving = playMove != Vector2.zero;
        Vector2 lastMove = Vector2.zero;
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