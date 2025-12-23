using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    //1. 애니메이션 블렌드 트리
    //2. 애니메이션 블렌드 레이어

    //애니메이션 컴포넌트
    Animator anim;

    //이동관련
    float walkspeed = 2f;
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
        //애니메이터 컴포넌트
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    anim.Play("Idle");
        //    anim.CrossFade("Idle", 0.2f);
        //}
        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    anim.Play("Run");
        //    anim.CrossFade("Run", 0.2f);
        //}

        //키보드 입력받기
        HandlInput();

        //캐릭터 이동처리
        MovePlayer();

        //애니메이션 업데이트
        UpdateAnimation();

        //아바타 마스크 블렌드 레이어
        WaveAnimatoin();

    }

    /// <summary>
    /// 스페이스바를 누르면 마스크한 애니메이션 나오게(상체레이어)
    /// 이동중에도 가능
    /// </summary>
    private void WaveAnimatoin()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsRun", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("IsRun", false);
        }
    }

    /// <summary>
    /// 애니메이터의 파라미터를 업데이트 해서 애니메이션 재생
    /// </summary>
    private void UpdateAnimation()
    {
        float animationSpeed;

        if (currentSpeed < 0.1f)
        {
            animationSpeed = 0f;
        }
        else if (isRunning)
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
    /// 캐릭터 이동하기
    /// </summary>
    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0f, v);
        dir.Normalize();

        //쉬프트 눌렸는지 확인후 속도 변경
        float targetSpeed = isRunning ? runSpeed : walkspeed;//isRunning == ture 면 runSpeed, false면 walkSpeed;

        if (dir.magnitude > 0.1f)
        {
            //캐릭터를 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            //부드럽게 회전시키기
            transform.rotation = Quaternion.Lerp(
                transform.rotation,                 //현재 회전
                targetRotation,                     //목표회전
                rotSpeed * Time.deltaTime);         //회전 속도

            //현재 속도를 목표 속도쪽으로 부드럽게 이동
            //갑자기 빨라지면 이상
            currentSpeed = Mathf.Lerp(
                currentSpeed,               //현재속도
                targetSpeed,                //목표속도
                0.2f);                      //전환속도

            //실재로 이동처리
            //캐릭터 컨트롤러 사용하던, 아님 트랜스폼으로 이동하던 알아서
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
    /// 키보드 입력 처리
    /// </summary>
    void HandlInput()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

    }
}
