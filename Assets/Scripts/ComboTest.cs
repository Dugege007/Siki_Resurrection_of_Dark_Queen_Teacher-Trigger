using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTest : MonoBehaviour
{
    //��ȡ�������
    private Animator anim;
    //�Ƿ�ʼ����
    private bool startCombo;
    //��ǰ��������
    private int combo;

    private void Start()
    {
        //��ȡ�������
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //�������������
        if (Input.GetMouseButtonDown(0))
        {
            //���ComboֵΪ0
            if (anim.GetInteger("Combo") == 0)
            {
                //��Comboֵ����1
                combo++;
                //����Attack1
                anim.SetInteger("Combo", combo);
            }

            //����ڿ���������ʱ�䷶Χ��
            if (startCombo)
            {
                //��Comboֵ����1
                combo++;
                //���comboֵ����3
                if (combo > 3)
                {
                    ResetCombo();
                }
                //���ź����Ĺ�������
                anim.SetInteger("Combo", combo);
                startCombo = false;
            }

            Debug.Log("��ǰ����������" + combo);
        }
    }

    //�����¼��� StartComboState() �� EndComboState() ֮���ʱ�������Խ�������
    private void StartComboState()
    {
        startCombo = true;
    }

    private void EndComboState()
    {
        startCombo = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void ResetCombo()
    {
        combo = 0;
        anim.SetInteger("Combo", combo);
        EndComboState();
    }
}
