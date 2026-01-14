using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    //1. 애니메이션 블렌드 트리
    //2. 애니메이션 블렌드 레이어

    //애니메이터 컴포넌트
    Animator anim;

    //이동관련
    float walkSpeed = 2f;
    float runSpeed = 5f;
    //회전관련
    float rotSpeed = 10f;

    //현재 이동 속도 저장할 변수
    float currentSpeed = 0f;

    //달리기 상태를 저장하는 변수
    bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //애니메이터 컴포넌트 가져오기
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    //anim.Play("Idle");
        //    anim.CrossFade("Idle", 0.2f);
        //}
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    //anim.Play("Run");
        //    anim.CrossFade("Run", 0.2f);
        //}

        //키보드 입력 받기
        HandlInput();

        //캐릭터 이동처리
        MovePlayer();

        //애니메이션 업데이트
        UpdateAnimation();

        //아바타 마스트 블렌드 레이어
        WaveAnimation();

           
    }


    /// <summary>
    /// 키보드 입력 처리
    /// </summary>
    void HandlInput()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    /// <summary>
    /// 캐릭터 이동하기
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0f, v);
        dir.Normalize();

        //왼쪽 쉬프트 눌렸는지 확인 후 속도 변경
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        //이동 방향이 있을때 처리
        if(dir.magnitude > 0.1f)
        {
            //캐릭터를 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            //부드럽게 회전시키기
            transform.rotation = Quaternion.Lerp(
                transform.rotation,                 //현재 회전
                targetRotation,                     //목표 회전
                rotSpeed * Time.deltaTime);         //회전 속도

            //현재 속도를 목표 속도쪽으로 부드럽게 이동
            //갑자기 빨라지면 이상하니까
            currentSpeed = Mathf.Lerp(
                currentSpeed,                       //현재 속도
                targetSpeed,                        //목표 속도
                0.2f);                              //전환 속도

            //실제로 이동처리
            //캐릭터컨트롤러 사용하던, 아님 트렌스폼으로 이동하던
            //알아서 처리하기
        }
        else
        {
            //속도를 0으로 부드럽게 감소
            currentSpeed = Mathf.Lerp(
                currentSpeed,
                0,
                0.2f);
        }
    }

    /// <summary>
    /// 애니메이터의 파라미터를 업데이트해서 애니메이션 재생
    /// </summary>
    private void UpdateAnimation()
    {
        float animationSpeed;

        if(currentSpeed < 0.1f)
        {
            animationSpeed = 0f;
        }
        else if(isRunning)
        {
            animationSpeed = 2f;
        }
        else
        {
            animationSpeed = 1f;
        }

        //애니메이션 값 세팅
        anim.SetFloat("MoveSpeed", animationSpeed);

    }

    /// <summary>
    /// 스페이스바를 누르면 손을 살랑살랑 (상체 레이어)
    /// 이동 중에도 살랑살랑 가능
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void WaveAnimation()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsRunning", true);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("IsRunning", false);
        }
    }

}
