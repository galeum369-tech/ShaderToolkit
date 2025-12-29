using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class Player_2DSimple : MonoBehaviour
{
    Animator anim;
    /// <summary>
    /// PlayerInput 컴포넌트 (C# Events 방식을 사용할 때 필요)
    /// </summary>
    PlayerInput playerInput;    //PlayerInput 컴포넌트 참조 변수
    InputAction moveAction;     //Move액션 참조 변수
    InputAction jumpAction;     //Jump액션 참조 변수
    Vector2 moveInput;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        //PlayerInput 컴포넌트 참조 초기화 (requircomponent 어트리뷰트 덕분에 반드시 존재
        playerInput = GetComponent<PlayerInput>();

        //각각의 액션들 등록
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions["Jump"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //아래는 SendMassage 방식으로 호출되는 입력 처리 함수들
    //캐릭터 이동같은 경우는 매 프레임마다 값을 받아야 하므로 InputValue 파라미터를 받음
    //Input System의 Input Action에서 정의한 이름과 동일해야 함 / 함수명을 정확히 맞춰서 사용해야한다
    //하지만 SendMassage같은 경우 컴포넌트의 함수들을 전부 검색하기 때문에 성능이 약간 저하될 수 있다
    //그럼에도 별도의 성정없이 간단한 코드로 사용가능하기에 빠르게 프로토타입을 제작할 때 유용

    //BroadcastMessage => SendMassage와 완전히 동일하지만 자식 오브젝트들까지 모두 검색해서 호출
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {

    }

    //Input System의 Invoke Unity Events 방식은 유니티 이벤트 방식으로 UI버튼 클릭같은 이벤트 등록하는 것과 동일
    //단 파라미터는 반드시 InputAction.ColllbackContext 타입이어야 한다
    //context.started : 입력이 시작될 때 호출
    //context.performd : 입력이 수행될 때 호출됨 // 주로 사용
    //context.canceled : 입력이 취소될 때 호출 / 예: 버튼에서 손을 땔 때

    //Input System의 Invoke C# Events 방식
    // C# 이벤트(델리게이트) 방식으로 함수를 지정해서 사용할 수 있다
    // += 기호를 사용해서 이벤트 핸들러를 추가할 수 있다
    // -= 기호를 사용해서 이벤트 핸들러를 반드시 제거해야 한다
    //반드시 이벤트 등록을 해지해야 메모리 누수가 발생하지 않는다
    //일반적으로 void OnDisable() 함수 안에서 이벤트 해지를 수행한다

}
