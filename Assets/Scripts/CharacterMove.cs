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
    private Vector2 velocity;

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
    Vector2 nextPosition = rb.position + playMove.normalized * moveSpeed * Time.fixedDeltaTime;

    Vector3Int gridPositionWater = waterTilemap.WorldToCell(nextPosition);
    Vector3Int gridPositionGround = groundTilemap.WorldToCell(nextPosition);

    bool isGroundTile = groundTilemap.HasTile(gridPositionGround);
    bool isWaterTile = waterTilemap.HasTile(gridPositionWater);

    if (isGroundTile || (isGroundTile && isWaterTile))
    {
        rb.velocity = playMove.normalized * moveSpeed;
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
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Box"))
    {
        if(isPushing)
            return;
        isPushing = true;

        Rigidbody2D boxRb = collision.gameObject.GetComponent<Rigidbody2D>();

        Vector2 pushDirection = playMove.normalized;
        Vector2 newBoxPosition = (Vector2)collision.gameObject.transform.position + pushDirection;
        Debug.Log(newBoxPosition);

        // Di chuyển thùng theo grid (1 đơn vị mỗi lần)
        boxRb.MovePosition(newBoxPosition);
    }
}
private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Box"))
    {
        isPushing = false;
    }
}  
    
}