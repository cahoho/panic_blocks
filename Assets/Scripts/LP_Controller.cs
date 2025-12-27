using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 左侧玩家移动手感优化。改代码控制了玩家的移动逻辑
/// 于玩家数量多，所以玩家主要控制还是在PlayerManager.cs文件
/// 
/// 下文的AllPlayerxxx()函数均实现自PlayerManager.cs。详情可转到。
/// </summary>
public class LP_Controller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum State { LEFT, RIGHT }
    public State state;

    [Header("按钮颜色")]
    public Color color = Color.white;
    public Color changeColor = Color.gray;

    private Image image;
    private bool isHolding = false;

    private static LP_Controller leftButton;
    private static LP_Controller rightButton;
    private static LP_Controller currentActiveButton = null;
//为方便调试：接收键盘输入
    private static bool leftKeyPressed = false;
    private static bool rightKeyPressed = false;
    private void Start()
    {
        ResetStaticState(); 

        image = GetComponent<Image>();
        if (image != null) image.color = color;

        if (state == State.LEFT) leftButton = this;
        else if (state == State.RIGHT) rightButton = this;
    }

    private void OnDisable()
    {
        ResetStaticState();
    }
    
    private static void ResetStaticState()
    {
        leftKeyPressed = false;
        rightKeyPressed = false;
        currentActiveButton = null;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        ClearActiveAndStop();
    }

    private void Update()
    {
        HandleKeyboardInput();

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseUI();
#else
        HandleTouchUI();
#endif

        if (leftKeyPressed)
            Move(State.LEFT);
        else if (rightKeyPressed)
            Move(State.RIGHT);
    }

// 为方便调试，读取键盘输入
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftKeyPressed = true;
            SetActiveButton(leftButton);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            rightKeyPressed = true;
            SetActiveButton(rightButton);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            leftKeyPressed = false;

            if (rightKeyPressed)
                SetActiveButton(rightButton);
            else
                ClearActiveAndStop();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            rightKeyPressed = false;

            if (leftKeyPressed)
                SetActiveButton(leftButton);
            else
                ClearActiveAndStop();
        }
    }


    private void HandleMouseUI()
    {
        if (!isHolding) return;
        if (!Input.GetMouseButton(0)) return;

        Vector2 pos = Input.mousePosition;
        LP_Controller hovered = null;

        if (leftButton != null && RectTransformUtility.RectangleContainsScreenPoint(leftButton.GetComponent<RectTransform>(), pos))
            hovered = leftButton;
        else if (rightButton != null && RectTransformUtility.RectangleContainsScreenPoint(rightButton.GetComponent<RectTransform>(), pos))
            hovered = rightButton;

        if (hovered != currentActiveButton)
            SetActiveButton(hovered);

        if (currentActiveButton != null)
            Move(currentActiveButton.state);
    }


    private void HandleTouchUI()
    {
        if (Input.touchCount == 0)
        {
            ClearActiveAndStop();
            return;
        }

        LP_Controller hovered = null;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector2 pos = Input.GetTouch(i).position;

            if (leftButton != null && RectTransformUtility.RectangleContainsScreenPoint(leftButton.GetComponent<RectTransform>(), pos))
            {
                hovered = leftButton;
                break;
            }
            if (rightButton != null && RectTransformUtility.RectangleContainsScreenPoint(rightButton.GetComponent<RectTransform>(), pos))
            {
                hovered = rightButton;
                break;
            }
        }

        if (hovered != currentActiveButton)
            SetActiveButton(hovered);

        if (currentActiveButton != null)
            Move(currentActiveButton.state);
    }


    private void SetActiveButton(LP_Controller btn)
    {
        currentActiveButton = btn;
        SetHighlight(btn);
    }

    private void ClearActiveAndStop()
    {
        currentActiveButton = null;
        SetHighlight(null);

        // 如果键盘仍按着，则继续移动
        if (leftKeyPressed)
            Move(State.LEFT);
        else if (rightKeyPressed)
            Move(State.RIGHT);
        else
            StopMoving();
    }

    private void SetHighlight(LP_Controller target)
    {
        if (leftButton && leftButton.image)
            leftButton.image.color = (leftButton == target) ? changeColor : leftButton.color;

        if (rightButton && rightButton.image)
            rightButton.image.color = (rightButton == target) ? changeColor : rightButton.color;
    }


    private void Move(State dir)
    {
        var player = Singleton<PlayerManager>.Instance;
        if (player == null) return;

        if (dir == State.LEFT) player.AllPlayersLeftMove();// 防止玩家在空中撞墙之后而停止移动
        if (dir == State.RIGHT) player.AllPlayersRightMove();
    }

    private void StopMoving()
    {
        var player = Singleton<PlayerManager>.Instance;
        if (player == null) return;

        player.AllPlayersStopMoving();
    }
}
