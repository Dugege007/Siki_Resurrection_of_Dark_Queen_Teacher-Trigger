using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum State
{
    Master, Blademan, Swordman, Assassin
}

//�����˳�����ļ򵥿���
public class PlayerController : MonoBehaviour
{
    [Header("Base")]
    private Rigidbody rigid;
    private Animator animator;
    public Collider biomechCollider;
    public bool isClone;

    [Header("Attributes")]
    public int maxHP;
    private int currentHP;
    public bool isDead;

    [Header("Move")]
    private float inputH;
    private float inputV;
    public float moveSpeed;
    //private Vector3 inputDir;
    public float rotateSpeed;
    private bool isOnGround;
    private int moveScale;
    //���
    private float timeLost;
    private bool isRunning;
    public bool canGetPlayerInputValue = true;
    public bool canMove = true;

    [Header("Jump")]
    public float jumpForce;
    //�����������
    public int maxJumpTimes;
    private int jumpCount;
    public bool canJump = true;

    [Header("Equip")]
    public bool canEquip = true;

    [Header("Attack")]
    public bool canAttack = true;

    [Header("Equip")]
    public bool canUseSkill = true;

    [Header("Weapon")]
    //0 ��
    //1 �ڰ�֮��
    //2 ����
    //3 ��ذ��
    //4 ��ذ��
    //5 �ҽ�
    //6 ���
    public BoxCollider[] weaponColliders;

    [Header("Transfiguration")]
    public ParticleSystem transfigurationEffect;
    public ParticleSystem transEffect;
    public SkinnedMeshRenderer sr;
    public State currentState;

    [Header("Biomech Master")]
    #region ��ʦ
    //Ƥ��
    public Material[] mastermanMaterials;
    //����������
    public RuntimeAnimatorController masterRA;

    //��Ӱħ����
    public GameObject shadowProjectileGo;
    public Transform leftHandTrans;
    //public GameObject handBall;
    public GameObject leftHandBall;
    //��Ӱն
    //public ParticleSystem[] particleSystems;
    public GameObject slashEffectGo;
    public Transform rightHandTrans;
    //��Ӱ���
    public GameObject cleaveEffectGo;
    //���ذ�Ӱ���
    //��Ӱ����
    public GameObject shadowShieldGo;
    //��Ӱ���
    public GameObject rightHandBall;
    public GameObject bigShadowProjectileGo;

    #region ��Ӱħ����
    private void CreateShadowProjectile(int isLeft)
    {
        GameObject itemGO;
        if (isLeft == 1)
            itemGO = Instantiate(shadowProjectileGo, leftHandTrans.position, transform.rotation);
        else
            itemGO = Instantiate(shadowProjectileGo, rightHandTrans.position, transform.rotation);
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }

    private void ShowBall(int isLeft)
    {
        //handBall.SetActive(true);
        if (isLeft == 1)
            leftHandBall.SetActive(true);
        else if (isLeft == 0)
            rightHandBall.SetActive(true);
    }

    private void HideBall(int isLeft)
    {
        //handBall.SetActive(false);
        if (isLeft == 1)
            leftHandBall.SetActive(false);
        else if (isLeft == 0)
            rightHandBall.SetActive(false);
    }

    #endregion

    #region ��Ӱն
    private void PlayParticals(float angle)
    {
        //for (int i = 0; i < particleSystems.Length; i++)
        //{
        //    particleSystems[i].Play();
        //}
        GameObject itemGO = Instantiate(slashEffectGo, transform.position + new Vector3(transform.forward.x, transform.forward.y + 1, transform.forward.z * 1.8f), Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, angle)));
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }
    #endregion

    #region ��Ӱ���
    private void PlayCleaveParticals()
    {
        GameObject itemGO = Instantiate(cleaveEffectGo, transform.position + transform.forward, transform.rotation);
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }
    #endregion

    #region ��Ӱ����
    private void PlayShadowShield()
    {
        GameObject itemGo = Instantiate(shadowShieldGo, transform.position, transform.rotation);
        itemGo.transform.SetParent(transform);
    }
    #endregion

    #region ��Ӱ���
    private void CreateBigShadowProjectile()
    {
        GameObject itemGO = Instantiate(bigShadowProjectileGo, (leftHandTrans.position + rightHandTrans.position) * 0.5f, transform.rotation);
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }
    #endregion
    #endregion

    [Header("Biomech BladeMan")]
    #region ����
    //Ƥ��
    public Material[] blademanMaterials;
    //����������
    public RuntimeAnimatorController blademanRA;

    public bool hasBlade;
    public GameObject equipBladeGo;
    public GameObject unEquipBladeGo;
    //�Ƿ�װ������
    public bool isEquip;

    //�Ƿ��������
    public bool startCombo;
    public int combo;
    //����ʱ�ĽŲ���Ч
    public ParticleSystem leftHitBallPS;
    public ParticleSystem rightHitBallPS;

    public GameObject bladeGo;
    public GameObject darkBladeGo;
    public ParticleSystem bladeChangeEffect;

    private void ShowOrHideEquipBlade(int show)
    {
        if (show == 0)
        {
            equipBladeGo.SetActive(false);
            unEquipBladeGo.SetActive(true);
        }
        else if (show == 1)
        {
            equipBladeGo.SetActive(true);
            unEquipBladeGo.SetActive(false);
        }
        else if (show == 2)
        {
            equipBladeGo.SetActive(false);
            unEquipBladeGo.SetActive(false);
        }
    }

    private void ShowHitBall(int isLeft)
    {
        //handBall.SetActive(true);
        if (isLeft == 1)
            leftHitBallPS.Play();
        else if (isLeft == 0)
            rightHitBallPS.Play();
    }

    private void HideHitBall(int isLeft)
    {
        //handBall.SetActive(false);
        if (isLeft == 1)
            leftHitBallPS.Stop();
        else if (isLeft == 0)
            rightHitBallPS.Stop();
    }

    private void ShowOrHideBlade(AnimationEvent animationEvent)
    {
        if (animationEvent.intParameter == 0)
        {
            //��ͨ��
            bladeGo.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
            //darkBladeGo.SetActive(false);
        }
        else
        {
            //�ڰ�֮��
            //bladeGo.SetActive(false);
            darkBladeGo.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
        }
    }

    private void PlayChangeBladeEffect()
    {
        bladeChangeEffect.Play();
    }

    public void HasNewBlade()
    {
        equipBladeGo.SetActive(true);
        hasBlade = true;
        animator.CrossFade("EquipState", 0.1f);
        animator.SetBool("Equip", true);
        isEquip = true;
    }
    #endregion

    [Header("Biomech Swordman")]
    #region ����
    //Ƥ��
    public Material[] swordmanMaterials;
    //����������
    public RuntimeAnimatorController swordmanRA;
    //������Ч
    public ParticleSystem swordEffect;
    //��ȡ������Ϸ����
    public GameObject swordGo;

    private void PlaySwordEffect()
    {
        swordEffect.Play();
    }

    private void ShowOrHideSword(bool show)
    {
        swordGo.SetActive(show);
    }
    #endregion

    [Header("Biomech Assassin")]
    #region �̿�
    //Ƥ��
    public Material[] assassinMaterials;
    //����������
    public RuntimeAnimatorController assassinRA;
    public GameObject unEquipLeftDaggerGO;
    public GameObject unEquipRightDaggerGO;
    public GameObject leftDaggerGO;
    public GameObject rightDaggerGO;

    /// <summary>
    /// ��ʾ���������ϵ�ذ��
    /// </summary>
    /// <param name="animationEvent"></param>
    private void ShowOrHideDagger(AnimationEvent animationEvent)
    {
        if (animationEvent.intParameter == 0)
            rightDaggerGO.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
        else
            leftDaggerGO.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
    }

    private void ShowOrHideDagger(int handID, bool show)
    {
        if (handID == 0)
            rightDaggerGO.SetActive(show);
        else
            leftDaggerGO.SetActive(show);
    }

    /// <summary>
    /// ��ʾ���������ϵ�ذ��
    /// </summary>
    /// <param name="animationEvent"></param>
    private void ShowOrHideUnEquipDagger(AnimationEvent animationEvent)
    {
        if (animationEvent.intParameter == 0)
            unEquipRightDaggerGO.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
        else
            unEquipLeftDaggerGO.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
    }

    private void ShowOrHideUnEquipDagger(int handID, bool show)
    {
        if (handID == 0)
            unEquipRightDaggerGO.SetActive(show);
        else
            unEquipLeftDaggerGO.SetActive(show);
    }

    #endregion

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        biomechCollider = GetComponent<Collider>();
        isOnGround = true;
        canGetPlayerInputValue = true;
        canMove = true;
        canJump = true;
        canEquip = true;
        canAttack = true;
        canUseSkill = true;

        currentHP = maxHP;
        combo = 0;

        if (isClone)
        {
            if (isEquip)
                animator.SetBool("Equip", true);
        }
    }

    private void Update()
    {
        if (!isClone && !isDead)
            PlayerInput();
    }

    private void FixedUpdate()
    {
        if (!canGetPlayerInputValue)
            return;
        Move();
    }

    #region �������
    /// <summary>
    /// �������
    /// </summary>
    private void PlayerInput()
    {
        if (!canGetPlayerInputValue)
            return;
        PlayerMoveInput();
        PlayerJumpInput();
        PlayerEquipInput();
        PlayerAttackInput();
        PlayerSkillInput();
    }

    public void UnLockAll()
    {
        canMove = canJump = canEquip = canAttack = canUseSkill = canGetPlayerInputValue = true;
        startCombo = false;
        ShowOrHideWeaponColliders(false);
    }

    /// <summary>
    /// �л�װ����ְҵ��̬
    /// </summary>
    private void PlayerEquipInput()
    {
        if (!canEquip)
            return;

        //�л�װ����̬
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentState == State.Master)
                return;

            if (currentState == State.Blademan)
            {
                if (hasBlade)
                {
                    isEquip = !isEquip;
                    animator.SetBool("Equip", isEquip);
                }
            }
            else if (currentState == State.Swordman)
            {
                isEquip = !isEquip;
                animator.SetBool("Equip", isEquip);
                if (isEquip)
                {
                    animator.CrossFade("EquipSword", 0.1f);
                }
                //SetAnimationPlaySpeed(-1);
            }
            else if (currentState == State.Assassin)
            {
                isEquip = !isEquip;
                animator.SetBool("Equip", isEquip);
            }
        }

        //�ı䵱ǰְҵ��̬
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.CrossFade("Transfiguration", 0.1f);
        }
    }

    #region ����
    /// <summary>
    /// ��ҹ�������
    /// </summary>
    private void PlayerAttackInput()
    {
        if (!canAttack || currentState == State.Master)
            return;

        if (isEquip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (animator.GetInteger("Combo") == 0)
                {
                    combo++;
                    animator.SetInteger("Combo", combo);
                }

                if (startCombo)
                {
                    combo++;
                    if (combo > 3)
                    {
                        ResetCombo();
                        combo = 0;
                    }
                    animator.SetInteger("Combo", combo);
                    startCombo = false;
                }

                Debug.Log("��ǰ����������" + combo);
            }
        }
    }

    /// <summary>
    /// �رտ��Խ��빥��1�����
    /// </summary>
    //private void EndAttackState()
    //{
    //    anim.SetBool("Attack", false);
    //    startCombo = true;
    //}

    private void StartComboState()
    {
        startCombo = true;
    }

    private void EndComboState()
    {
        startCombo = false;
    }

    private void ResetCombo()
    {
        combo = 0;
        animator.SetInteger("Combo", combo);
        EndComboState();
    }

    /// <summary>
    /// �����ҵļ�������
    /// </summary>
    private void PlayerSkillInput()
    {
        if (!canUseSkill)
            return;

        if (currentState == State.Blademan || currentState == State.Swordman || currentState == State.Assassin)
        {
            if (!isEquip)
                return;
        }

        if (currentState == State.Master)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    animator.CrossFade("Skill" + i.ToString(), 0.1f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                animator.CrossFade("Skill1", 0.1f);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    anim.CrossFade("Skill1", 0.1f);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    anim.CrossFade("Skill2", 0.1f);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    anim.CrossFade("Skill3", 0.1f);
        //}
    }
    #endregion

    #region �ƶ�
    /// <summary>
    /// ����ƶ�����
    /// </summary>
    private void PlayerMoveInput()
    {
        if (!canMove)
        {
            inputH = inputV = 0;
            return;
        }

        if (isRunning)
            moveScale = 3;
        else
        {
            if (Input.GetKey(KeyCode.LeftControl))
                moveScale = 1;
            else
                moveScale = 2;
        }

        if (Input.GetButtonDown("Vertical"))
        {
            if (Time.time - timeLost < 0.5f && DoubleClickForward())
            {
                Debug.Log("�ڶ��ΰ�����ǰ");
                isRunning = true;
            }
            Debug.Log("��һ�ΰ�����ǰ");
            timeLost = Time.time;
        }
        else if (Input.GetButtonUp("Vertical"))
        {
            isRunning = false;
        }


        inputH = Input.GetAxis("Horizontal") * moveScale;
        inputV = Input.GetAxis("Vertical") * moveScale;
        //inputDir = new Vector3(inputH,0,inputV);

        //������ת
        if (inputH != 0 && inputV != 0)
        {
            float targetRotation = rotateSpeed * inputH;
            transform.eulerAngles = Vector3.up * Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + targetRotation, Time.deltaTime);
        }
        if (currentState == State.Assassin)
        {
            if (inputH != 0 || inputV != 0)
                animator.SetFloat("MoveState", 1);
            else
                animator.SetFloat("MoveState", 0);
        }
    }

    /// <summary>
    /// �����Ծ����
    /// </summary>
    private void PlayerJumpInput()
    {
        if (!canJump)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpTimes)
        {
            if (isOnGround)
            {
                isOnGround = false;
                animator.SetBool("IsOnGround", isOnGround);
            }

            if (jumpCount == 0)
            {
                animator.CrossFade("Jump", 0.1f);
            }
            else
            {
                //rigid.AddForce(Vector3.up * jumpForce * 0.1f);
                animator.CrossFade("Double_Jump", 0.1f);
            }

            jumpCount++;
            rigid.AddForce(Vector3.up * jumpForce);
            //if (hasWeapon)
            //{
            //    //ֱ�ӹ��ɵ�Jump����
            //    anim.CrossFade("Jump_B", 0.1f);
            //}
            //else
            //{
            //    anim.CrossFade("Jump", 0.1f);
            //}
        }
    }

    /// <summary>
    /// �ж�����ڹ涨ʱ�����Ƿ���������ǰ����
    /// </summary>
    /// <returns></returns>
    private bool DoubleClickForward()
    {
        return inputV > 0 && Input.GetAxis("Vertical") > 0 && currentState != State.Master;
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    private void Move()
    {
        //�����ƶ�
        if (inputV != 0)
        {
            //rigid.MovePosition(transform.position + transform.TransformDirection(inputDir) * moveSpeed * Time.deltaTime);
            rigid.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime * inputV);
            //anim.SetBool("Move", true);
            animator.SetFloat("InputH", 0);
            //anim.SetFloat("MoveState", Mathf.Abs(inputV));
            animator.SetFloat("InputV", inputV);
        }
        else
        {
            if (inputH != 0)
            {
                rigid.MovePosition(transform.position + transform.right * moveSpeed * Time.deltaTime * inputH);
                //anim.SetBool("Move", true);
                //anim.SetFloat("MoveState", Mathf.Abs(inputH));
                animator.SetFloat("InputH", inputH);
            }
            else
            {
                //���ƶ�
                //anim.SetBool("Move", false);
                animator.SetFloat("InputV", inputV);
                animator.SetFloat("InputH", inputH);
            }
        }
    }
    #endregion
    #endregion

    #region �ܵ��˺�
    public void TakeDamage(int damageValue, Vector3 hitPos)
    {
        currentHP -= damageValue;
        if (currentHP <= 0)
        {
            //����
            Die();
            animator.SetBool("Die", true);
            return;
        }
        if (currentHP <= maxHP / 3)
        {
            //��Ѫ
            animator.SetLayerWeight(1, 1);
        }

        //��HP�����𽥱�Ϊ����״̬
        //animator.SetLayerWeight(1, (float)(maxHP - currentHP) / maxHP);

        float x = Vector3.Dot(transform.right, hitPos);
        float y = Vector3.Dot(transform.forward, hitPos - transform.position);
        animator.SetTrigger("Hit");
        if (ForwardBehindOrLeftRight(hitPos))
        {
            if (y > 0)
            {
                Debug.Log("�˺�Դ��ǰ��");
                animator.SetFloat("HitY", 1);
            }
            else
            {
                Debug.Log("�˺�Դ�ں�");
                animator.SetFloat("HitY", -1);
            }
        }
        else
        {
            if (x > 0)
            {
                Debug.Log("�˺�Դ���ҷ�");
                animator.SetFloat("HitX", 1);
            }
            else
            {
                Debug.Log("�˺�Դ����");
                animator.SetFloat("HitX", -1);
            }
        }
    }

    private bool ForwardBehindOrLeftRight(Vector3 targetPos)
    {
        float distanceZ = Mathf.Abs(transform.position.z - targetPos.z);
        float distanceX = Mathf.Abs(transform.position.x - targetPos.x);
        if (distanceZ >= distanceX)
            return true;
        else
            return false;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + "����");
        biomechCollider.enabled = false;
        rigid.isKinematic = true;
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
    }

    private void ShowOrHideWeaponColliders(AnimationEvent animationEvent)
    {
        weaponColliders[animationEvent.intParameter].enabled = Convert.ToBoolean(animationEvent.stringParameter);
    }

    private void ShowOrHideWeaponColliders(bool show)
    {
        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].enabled = show;
        }
    }

    public void Execute(int executeID)
    {
        animator.CrossFade("Execute" + executeID, 0.1f);
    }

    public void BeExecuted(int executeID, Transform executedTrans)
    {
        Die();
        ResetAnimatorParameters();
        executedTrans.LookAt(transform.position);
        if (executeID == 1)
            transform.forward = executedTrans.forward;
        else
            transform.forward = -executedTrans.forward;
        animator.CrossFade("Executed" + executeID, 0.1f);
    }

    public bool CanExecute()
    {
        if (isClone)
            return currentHP <= maxHP / 4;
        else
            return false;
    }

    /// <summary>
    /// ����Ӱ�촦������ִ�е�����
    /// </summary>
    private void ResetAnimatorParameters()
    {
        animator.SetBool("Attack", false);
        animator.ResetTrigger("AttackCombo");
        animator.ResetTrigger("Hit");
        animator.SetFloat("InputH", 0);
        animator.SetFloat("InputV", 0);
        canGetPlayerInputValue = false;
    }
    #endregion

    #region ����
    private void ShowTransfigurationParticals()
    {
        //transfigurationEffectGo.SetActive(System.Convert.ToBoolean(show));
        transfigurationEffect.gameObject.SetActive(true);
        transfigurationEffect.Play();
    }

    private void HideTransfigurationParticals()
    {
        transfigurationEffect.Stop();
        Instantiate(transEffect, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// �л�״̬���ԣ�������������ۡ�������
    /// </summary>
    private void ChangeStateProperties()
    {
        currentState++;
        if (Convert.ToInt32(currentState) > 3)
            currentState = State.Master;

        Debug.Log(currentState);

        ResetState();
        switch (currentState)
        {
            case State.Master:
                ShowBall(0);
                ShowBall(1);
                ChangeMaterials(mastermanMaterials);
                maxJumpTimes = 1;
                animator.runtimeAnimatorController = masterRA;
                isEquip = true;
                animator.SetBool("Equip", isEquip);
                break;
            case State.Blademan:
                if (hasBlade)
                    ShowOrHideEquipBlade(0);
                ChangeMaterials(blademanMaterials);
                animator.runtimeAnimatorController = blademanRA;
                break;
            case State.Swordman:
                ShowOrHideSword(true);
                ChangeMaterials(swordmanMaterials);
                animator.runtimeAnimatorController = swordmanRA;
                break;
            case State.Assassin:
                ShowOrHideUnEquipDagger(0, true);
                ShowOrHideUnEquipDagger(1, true);
                ChangeMaterials(assassinMaterials);
                animator.runtimeAnimatorController = assassinRA;
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// ��������ְҵ��״̬
    /// </summary>
    private void ResetState()
    {
        HideBall(0);
        HideBall(1);
        ShowOrHideEquipBlade(2);
        ShowOrHideSword(false);
        ShowOrHideUnEquipDagger(0, false);
        ShowOrHideUnEquipDagger(1, false);
        ShowOrHideDagger(0, false);
        ShowOrHideDagger(1, false);
        maxJumpTimes = 2;
        canGetPlayerInputValue = true;
        isEquip = false;
        animator.SetBool("Equip", isEquip);
        animator.SetFloat("MoveState", 0);
    }

    private void ChangeMaterials(Material[] materials)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            sr.materials[i].CopyPropertiesFromMaterial(materials[i]);
        }
    }
    #endregion

    #region �¼�����
    private void OnCollisionEnter(Collision collision)
    {
        if (!isOnGround)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
                animator.SetBool("IsOnGround", isOnGround);
                jumpCount = 0;
            }
        }
    }

    /// <summary>
    /// �����¼�����
    /// </summary>
    /// <param name="value">��ֵ����animation�����</param>
    public void TestAnimationEvent(AnimationEvent value)
    {
        Debug.Log("ħ���򱻵����ˣ�");
        //Debug.Log("��ǰ�������������õ�INTֵ�ǣ�" + value);
        //Debug.Log("��ǰ���������еļ��������ǣ�" + value);
        //AnimationClip animationClip = value as AnimationClip;
        //Debug.Log("��ǰ���������еĶ����ǣ�" + animationClip);
        Debug.Log("��ǰ�������������õ�intֵ�ǣ�" + value.intParameter);
        Debug.Log("��ǰ�������������õ�floatֵ�ǣ�" + value.floatParameter);
        Debug.Log("��ǰ���������е�stringֵ�ǣ�" + value.stringParameter);
        Debug.Log("��ǰ���������еĶ���ֵ�ǣ�" + value.objectReferenceParameter);
        Debug.Log("��ǰ���õĶ����¼������õķ��������ǣ�" + value.functionName);
    }

    /// <summary>
    /// ����ĳ��״̬�Ĳ����ٶȣ���״̬�Ѿ�����Ϊ�ܶ�������Ӱ���״̬��
    /// </summary>
    /// <param name="speed"></param>
    private void SetAnimationPlaySpeed(float speed)
    {
        animator.SetFloat("AnimationPlaySpeed", speed);
    }
    #endregion
}
