using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum State
{
    Master, Blademan, Swordman, Assassin
}

//第三人称人物的简单控制
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
    //冲刺
    private float timeLost;
    private bool isRunning;
    public bool canGetPlayerInputValue = true;
    public bool canMove = true;

    [Header("Jump")]
    public float jumpForce;
    //最大连跳次数
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
    //0 大剑
    //1 黑暗之刃
    //2 长刀
    //3 右匕首
    //4 左匕首
    //5 右脚
    //6 左脚
    public BoxCollider[] weaponColliders;

    [Header("Transfiguration")]
    public ParticleSystem transfigurationEffect;
    public ParticleSystem transEffect;
    public SkinnedMeshRenderer sr;
    public State currentState;

    [Header("Biomech Master")]
    #region 法师
    //皮肤
    public Material[] mastermanMaterials;
    //动画控制器
    public RuntimeAnimatorController masterRA;

    //暗影魔法球
    public GameObject shadowProjectileGo;
    public Transform leftHandTrans;
    //public GameObject handBall;
    public GameObject leftHandBall;
    //暗影斩
    //public ParticleSystem[] particleSystems;
    public GameObject slashEffectGo;
    public Transform rightHandTrans;
    //暗影冲击
    public GameObject cleaveEffectGo;
    //多重暗影打击
    //暗影护盾
    public GameObject shadowShieldGo;
    //暗影轰击
    public GameObject rightHandBall;
    public GameObject bigShadowProjectileGo;

    #region 暗影魔法球
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

    #region 暗影斩
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

    #region 暗影冲击
    private void PlayCleaveParticals()
    {
        GameObject itemGO = Instantiate(cleaveEffectGo, transform.position + transform.forward, transform.rotation);
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }
    #endregion

    #region 暗影护盾
    private void PlayShadowShield()
    {
        GameObject itemGo = Instantiate(shadowShieldGo, transform.position, transform.rotation);
        itemGo.transform.SetParent(transform);
    }
    #endregion

    #region 暗影轰击
    private void CreateBigShadowProjectile()
    {
        GameObject itemGO = Instantiate(bigShadowProjectileGo, (leftHandTrans.position + rightHandTrans.position) * 0.5f, transform.rotation);
        itemGO.GetComponent<Weapon>().owner = this;
        itemGO.layer = gameObject.layer;
    }
    #endregion
    #endregion

    [Header("Biomech BladeMan")]
    #region 刀客
    //皮肤
    public Material[] blademanMaterials;
    //动画控制器
    public RuntimeAnimatorController blademanRA;

    public bool hasBlade;
    public GameObject equipBladeGo;
    public GameObject unEquipBladeGo;
    //是否装备武器
    public bool isEquip;

    //是否可以连击
    public bool startCombo;
    public int combo;
    //攻击时的脚部特效
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
            //普通刀
            bladeGo.SetActive(Convert.ToBoolean(animationEvent.stringParameter));
            //darkBladeGo.SetActive(false);
        }
        else
        {
            //黑暗之刃
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
    #region 剑客
    //皮肤
    public Material[] swordmanMaterials;
    //动画控制器
    public RuntimeAnimatorController swordmanRA;
    //剑气特效
    public ParticleSystem swordEffect;
    //获取剑的游戏物体
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
    #region 刺客
    //皮肤
    public Material[] assassinMaterials;
    //动画控制器
    public RuntimeAnimatorController assassinRA;
    public GameObject unEquipLeftDaggerGO;
    public GameObject unEquipRightDaggerGO;
    public GameObject leftDaggerGO;
    public GameObject rightDaggerGO;

    /// <summary>
    /// 显示与隐藏手上的匕首
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
    /// 显示与隐藏腰上的匕首
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

    #region 玩家输入
    /// <summary>
    /// 玩家输入
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
    /// 切换装备或职业形态
    /// </summary>
    private void PlayerEquipInput()
    {
        if (!canEquip)
            return;

        //切换装备形态
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

        //改变当前职业形态
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.CrossFade("Transfiguration", 0.1f);
        }
    }

    #region 攻击
    /// <summary>
    /// 玩家攻击输入
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

                Debug.Log("当前连击次数：" + combo);
            }
        }
    }

    /// <summary>
    /// 关闭可以进入攻击1的入口
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
    /// 检测玩家的技能输入
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

    #region 移动
    /// <summary>
    /// 玩家移动输入
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
                Debug.Log("第二次按下向前");
                isRunning = true;
            }
            Debug.Log("第一次按下向前");
            timeLost = Time.time;
        }
        else if (Input.GetButtonUp("Vertical"))
        {
            isRunning = false;
        }


        inputH = Input.GetAxis("Horizontal") * moveScale;
        inputV = Input.GetAxis("Vertical") * moveScale;
        //inputDir = new Vector3(inputH,0,inputV);

        //人物旋转
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
    /// 玩家跳跃输入
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
            //    //直接过渡到Jump动画
            //    anim.CrossFade("Jump_B", 0.1f);
            //}
            //else
            //{
            //    anim.CrossFade("Jump", 0.1f);
            //}
        }
    }

    /// <summary>
    /// 判断玩家在规定时间内是否按下两次向前按键
    /// </summary>
    /// <returns></returns>
    private bool DoubleClickForward()
    {
        return inputV > 0 && Input.GetAxis("Vertical") > 0 && currentState != State.Master;
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        //人物移动
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
                //不移动
                //anim.SetBool("Move", false);
                animator.SetFloat("InputV", inputV);
                animator.SetFloat("InputH", inputH);
            }
        }
    }
    #endregion
    #endregion

    #region 受到伤害
    public void TakeDamage(int damageValue, Vector3 hitPos)
    {
        currentHP -= damageValue;
        if (currentHP <= 0)
        {
            //死亡
            Die();
            animator.SetBool("Die", true);
            return;
        }
        if (currentHP <= maxHP / 3)
        {
            //残血
            animator.SetLayerWeight(1, 1);
        }

        //随HP减少逐渐变为重伤状态
        //animator.SetLayerWeight(1, (float)(maxHP - currentHP) / maxHP);

        float x = Vector3.Dot(transform.right, hitPos);
        float y = Vector3.Dot(transform.forward, hitPos - transform.position);
        animator.SetTrigger("Hit");
        if (ForwardBehindOrLeftRight(hitPos))
        {
            if (y > 0)
            {
                Debug.Log("伤害源在前方");
                animator.SetFloat("HitY", 1);
            }
            else
            {
                Debug.Log("伤害源在后方");
                animator.SetFloat("HitY", -1);
            }
        }
        else
        {
            if (x > 0)
            {
                Debug.Log("伤害源在右方");
                animator.SetFloat("HitX", 1);
            }
            else
            {
                Debug.Log("伤害源在左方");
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
        Debug.Log(gameObject.name + "死亡");
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
    /// 重置影响处决动画执行的条件
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

    #region 变身
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
    /// 切换状态属性，包括材质球、外观、控制器
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
    /// 重置所有职业的状态
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

    #region 事件函数
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
    /// 动画事件测试
    /// </summary>
    /// <param name="value">该值可在animation中输出</param>
    public void TestAnimationEvent(AnimationEvent value)
    {
        Debug.Log("魔法球被调用了！");
        //Debug.Log("当前动画剪辑中配置的INT值是：" + value);
        //Debug.Log("当前动画剪辑中的技能名称是：" + value);
        //AnimationClip animationClip = value as AnimationClip;
        //Debug.Log("当前动画剪辑中的对象是：" + animationClip);
        Debug.Log("当前动画剪辑中配置的int值是：" + value.intParameter);
        Debug.Log("当前动画剪辑中配置的float值是：" + value.floatParameter);
        Debug.Log("当前动画剪辑中的string值是：" + value.stringParameter);
        Debug.Log("当前动画剪辑中的对象值是：" + value.objectReferenceParameter);
        Debug.Log("当前配置的动画事件（调用的方法名）是：" + value.functionName);
    }

    /// <summary>
    /// 设置某个状态的播放速度（此状态已经设置为受动画参数影响的状态）
    /// </summary>
    /// <param name="speed"></param>
    private void SetAnimationPlaySpeed(float speed)
    {
        animator.SetFloat("AnimationPlaySpeed", speed);
    }
    #endregion
}
