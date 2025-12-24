using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_1D2 : MonoBehaviour
{
    Animator anim;          //애니메이터 컴포넌트 참조 변수
    CharacterController cc; //캐릭터 컨트롤러 참조 변수
    Vector2 moveInput;          //키입력을 받아서 이동처리하기 위한 변수

    public float walkSpeed = 2.0f;  //걷기 속도
    public float runSpeed = 5.0f;   //달리기 속도
    public float jumpForce = 5.0f;  //점프 힘
    public float gravity = -9.8f;   //중력
    float velocityY = 0.0f;         //Y축 속도 변수
    bool isGrounded = false;        //땅에 닿아있는지 체크
    bool isShiftPressed = false;    //Shift키가 눌렸는 체크


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //이동처리
        UpdateMove();
        //애니메이션 파라미터 업데이트
        UpdateAnimator();
        //땅에 닿아있는지 체크
        CheckGrounded();
        //쉬프트키가 눌렸는지 체크
        CheckShiftKey();
    }

    void CheckShiftKey()
    {
        //shift키가 눌렸는지 체크
        isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
    }

    void CheckGrounded()
    {
        //캐릭터 컨트롤러가 땅에 닿아있는지 체크
        isGrounded = cc.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2;  //땅에 닿아있다면 Y축 속도를 0으로 초기화
        }
    }

    void UpdateAnimator()
    {
        //입력 계산 (0~1사이의 갑)
        float inputMagnitude = moveInput.magnitude;
        float speed = 0f;

        if (inputMagnitude > 0.1f)
        {
            if (isShiftPressed)
            {
                //달리기
                speed = Mathf.Lerp(0.5f, 1f, inputMagnitude);
                print("Speed" + speed);
            }
            else
            {
                //걷기
                speed = Mathf.Lerp(0f, 0.5f, inputMagnitude);
            }
        }

        //애니메이터 파라미터 설정
        anim.SetFloat("Speed", speed);
    }

    void UpdateMove()
    {
        //현재 속도 결정
        float currentSpeed = isShiftPressed ? runSpeed : walkSpeed;
        //수평이동 방향 계산
        Vector3 move = Vector3.zero;
        if (moveInput.magnitude > 0.1f)
        {
            move = new Vector3(moveInput.x, 0f, moveInput.y) * currentSpeed * Time.deltaTime;
        }

        //중려 적용
        if (!isGrounded)
        {
            velocityY += gravity * Time.deltaTime;
        }

        //최종 이동 = 수평이동 + 수직이동
        Vector3 finalMove = move + new Vector3(0f, velocityY, 0f);

        //캐릭터 컨트롤러로 이동처리
        cc.Move(finalMove);
    }

    /// <summary>
    /// Input System에서 Move액션이 발생 했을 때 호출되는 메서드
    /// 이동입력을 받는다
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //WASD, 방향키 입력 받았을 때 
        moveInput = context.ReadValue<Vector2>();
        print($"Input Vector : {moveInput}");
    }

    /// <summary>
    /// Input Sysyem에서 Jump액션이 발생했을 때 호출되는 메서드
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // context.started, context.performed, context.canceled
        //점프 버튼이 눌렸을 때
        if (context.performed && isGrounded)
        {
            print("Jump");

            //점프 로직구현
            velocityY = Mathf.Sqrt(jumpForce * -2f * gravity * Time.deltaTime);
            //점프애니메이션 재생
            anim.SetTrigger("Jump");
        }

    }
}
