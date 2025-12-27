using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Player逻辑的核心。
/// 核心逻辑请转到PlayerManager.cs
/// 
/// 该代码为所有player准备。只要物体带有player.cs均可以实现player的逻辑。
/// 该代码自动向PlayerManager注册player信息，接受playermanager的控制。
/// </summary>
public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;

    [Header("Components")]
    public float speed;
    public float jumpForce;
    public bool canStop = true;
    public bool canJump = true; // 默认设置为true，如果需要禁用跳跃再通过其他逻辑控制
    public bool switchMove, switchHorizontal;

    [Header("Physics")]
    [HideInInspector] public bool isOnGround;
    public float footOffset;//0.195
    public float rayPositionY;//-0.502
    public float rayLength;//Default: 0.2
    bool isOnRotatingPlatform;

    Vector2 rayDir;
    public LayerMask ground;


    private void Start()
    {
        PlayerManager.Instance.RegisterPlayer(this);// 通过PlayerManager注册当前player
        
        anim = gameObject.GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        

        if (switchHorizontal)//这是player是否反转重力。请注意，player的动画idle第一帧有相关的数值更新！！
        {
            rb.gravityScale = -1;

            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            rayDir = Vector2.up;
            rayPositionY *= -1;
        }
        else
        {

            transform.rotation = Quaternion.identity;
            rayDir = Vector2.down;
        }
        
    }

    void Update()
    {
        anim.SetFloat("VelocityY", rb.velocity.y);
        anim.SetBool("OnGround", isOnGround);
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
    }

    public void Jump()
    {
        if (canJump && isOnGround ) 
        {
            AudioManager.PlayJumpAudio();
            int a = (rb.gravityScale < 0) ? -1 : 1;

            rb.velocity = new Vector2(rb.velocity.x, a * jumpForce); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            anim.SetTrigger("OnHit");
            GameManager.Instance.GameFail();
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("RotatingPlatform"))
    {
        
        // 将玩家设置为平台的子对象
        transform.SetParent(collision.transform);
        isOnRotatingPlatform = true;
    }
    else if (isOnGround && isOnRotatingPlatform)
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        isOnRotatingPlatform = false;
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("RotatingPlatform"))
    {
        transform.SetParent(null);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        isOnRotatingPlatform = false;
    }
}
    public void LeftMove() //由于有不同的玩家，不同player有不同的移动方式（有的是反转），因此这里的leftmove特指玩家输入，而不是player的真实表现。
    {
        anim.SetBool("IsWalking", true);
        float currentSpeed = switchMove ? speed : -speed;//在这里更改player的真正逻辑
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        gameObject.GetComponent<SpriteRenderer>().flipX = switchMove ? switchHorizontal : !switchHorizontal;
    }

    public void RightMove()
    {
        anim.SetBool("IsWalking", true);
        float currentSpeed = switchMove ? -speed : speed;
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        gameObject.GetComponent<SpriteRenderer>().flipX = switchMove ? !switchHorizontal : switchHorizontal;
    }

    public void StopMoving()
    {
        if (canStop)
        {
            
        anim.SetBool("IsWalking", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void PhysicsCheck()// 使用射线的方法
    {
        Vector2 originLeft = (Vector2)transform.position + new Vector2(footOffset, rayPositionY);
        Vector2 originRight = (Vector2)transform.position + new Vector2(-footOffset, rayPositionY);
        Vector2 originCenter = (Vector2)transform.position + new Vector2(0, rayPositionY);


        RaycastHit2D hit1 = Raycast(new Vector2(footOffset, rayPositionY), rayDir, rayLength, ground);
        RaycastHit2D hit2 = Raycast(new Vector2(-footOffset, rayPositionY), rayDir, rayLength, ground);
        RaycastHit2D hit3 = Raycast(new Vector2(0, rayPositionY), rayDir, rayLength, ground);

    
        bool grounded = hit1 || hit2 || hit3;
        isOnGround = grounded;

        Color color = grounded ? Color.red : Color.green;
        Debug.DrawRay(originLeft, rayDir * rayLength, color);
        Debug.DrawRay(originRight, rayDir * rayLength, color);
        Debug.DrawRay(originCenter, rayDir * rayLength, color);
    }
   private RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length, LayerMask layer)
{
    Vector2 origin = (Vector2)transform.position + offset;
    RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layer);

    Color color = hit ? Color.red : Color.green;
    Debug.DrawRay(origin, direction * length, color);

    return hit;
}
    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.UnregisterPlayer(this);
    }

    void DisableSelf()
    {
        Destroy(gameObject);
    }
}
 